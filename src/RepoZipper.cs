using System;
using System.Collections.Generic;
using LibGit2Sharp;
using Mpdeimos.GitRepoZipper.Model;
using Mpdeimos.GitRepoZipper.Util;
using System.Linq;
using System.IO;

namespace Mpdeimos.GitRepoZipper
{
	/// <summary>
	/// Zips multiple Git repositories into a single Git repository.
	/// </summary>
	public class RepoZipper
	{
		/// <summary>
		/// The zipper configuration.
		/// </summary>
		private readonly Config config;

		/// <summary>
		/// The repositories to zip.
		/// </summary>
		private readonly IEnumerable<Repository> repositories;

		/// <summary>
		/// Maps original commits to zipped ones.
		/// </summary>
		private readonly Dictionary<Commit, Commit> commitMap = new Dictionary<Commit, Commit>();

		/// <summary>
		/// Constructor.
		/// </summary>
		public RepoZipper(Config config)
		{
			this.config = config;
			this.repositories = config.Sources?.Select(source => new Repository(source));
		}

		/// <summary>
		/// Zips the configured repositories.
		/// </summary>
		public Repository Zip()
		{
			Log("Reading repositories...");
			var zippedRepo = new ZippedRepo(this.repositories, this.config.Exclude);
			Log("Zipping the following branches: " + string.Join(", ", zippedRepo.GetBranches()));

			Log("Initialize target repository...");
			var targetRepo = InitTargetRepo();

			Log("Build target repository...");
			BuildRepository(targetRepo, zippedRepo);
			return targetRepo;
		}

		/// <summary>
		/// Initializes the target repository.
		/// </summary>
		private Repository InitTargetRepo()
		{
			var target = new DirectoryInfo(this.config.Target);
			if (target.Exists)
			{
				if (!this.config.Force)
				{
					throw new ZipperException("Target directory '" + target + "' already exists.");
				}

				foreach (var file in target.GetFiles())
				{
					file.Delete();
				}

				foreach (var dir in target.GetDirectories())
				{
					dir.Delete(true);
				}
			}

			Repository.Init(target.FullName);
			Repository targetRepo = new Repository(target.FullName);
			foreach (Repository repo in this.repositories)
			{
				string name = Path.GetFileName(repo.Info.WorkingDirectory.TrimEnd(Path.DirectorySeparatorChar));
				targetRepo.Network.Remotes.Add(name, repo.Info.Path);
				Log("Fetching " + name + "...");
				targetRepo.Fetch(name);
			}
			return targetRepo;
		}

		private void BuildRepository(Repository repo, ZippedRepo source)
		{
			foreach (string name in source.GetBranches())
			{
				var commits = source.GetBranch(name);
				CherryPickCommits(repo, commits.ToArray(), name);
			}

			// TODO (MP) Handle anon branches

			GraftMerges(repo, source);

			// TODO (MP) Test
		}

		private void CherryPickCommits(Repository repo, Commit[] commits, string branchName)
		{
			Log("Zipping branch " + branchName + "...");
			Log("");
			Commit previous = null;
			for (int i = 0; i < commits.Length; i++)
			{
				var original = commits[i];
				if (commitMap.ContainsKey(original))
				{
					previous = commitMap[original];
					continue;
				}

				Log((100 * (i + 1) / commits.Length) + "% Zipping commit " + original.Sha, replace: true);

				if (repo.Branches[branchName] == null)
				{
					repo.Checkout(repo.CreateBranch(branchName, previous ?? original));
					if (previous == null)
					{
						commitMap[original] = original;
						continue;
					}
				}

				var commit = CherryPickCommit(repo, original);
				if (commit != null)
				{
					previous = commit;
					commitMap[original] = previous;
				}
			}
		}

		private Commit CherryPickCommit(Repository repo, Commit original)
		{
			var commit = repo.Lookup(original.Sha) as Commit;
			var options = new CherryPickOptions();
			if (commit.Parents.Count() > 1)
			{
				options.Mainline = 1;
			}

			try
			{
				return repo.CherryPick(commit, new Signature(commit.Author.Name, commit.Author.Email, commit.Author.When), options).Commit;
			}
			catch (EmptyCommitException)
			{
				// TODO (MP) Test this scenario
				Log("... skipped (empty commit)");
				return null;
			}
			catch (Exception e)
			{
				if (!config.Retry)
				{
					throw;
				}

				Log("An error occurred: \n" + e);
				Log("Press any key after fixing conflicts manually.");
				Console.ReadKey();

				return repo.Commit(commit.Message, commit.Author, commit.Author, new CommitOptions {
					AllowEmptyCommit = true
				});
			}
		}

		void GraftMerges(Repository repo, ZippedRepo source)
		{
			Log("Grafting merges...");
			var merges = source.GetMerges().ToDictionary(merge => commitMap[merge]);

			repo.Refs.RewriteHistory(new RewriteHistoryOptions {
				CommitParentsRewriter = commit =>
				{
					if (!merges.ContainsKey(commit))
					{
						return commit.Parents;
					}

					Commit[] parents = merges[commit].Parents.Select(parent => commitMap[parent]).ToArray();

					// ensure to take first zipped parent
					parents[0] = commit.Parents.First();

					return parents;
				}
			}, RepoUtil.GetAllCommits(repo));

			// cleanup original refs
			foreach (var @ref in repo.Refs.FromGlob("refs/original/*"))
			{
				repo.Refs.Remove(@ref);
			}
		}

		private void Log(string message, bool replace = false)
		{
			if (!this.config.Silent)
			{
				if (replace)
				{
					Console.SetCursorPosition(0, Console.CursorTop - 1);
				}

				Console.WriteLine(message);
			}
		}
	}
}