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
		private readonly Dictionary<string, Commit> commitMap = new Dictionary<string, Commit>();

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly Logger logger;

		/// <summary>
		/// Constructor.
		/// </summary>
		public RepoZipper(Config config)
		{
			this.config = config;
			this.logger = new Logger{ Silent = config.Silent };
			this.repositories = config.Sources?.Select(source => new Repository(source));
		}

		/// <summary>
		/// Zips the configured repositories.
		/// </summary>
		public Repository Zip()
		{
			this.logger.Log("Reading repositories...");
			var zippedRepo = new ZippedRepo(this.repositories, this.config);
			this.logger.Log("Zipping the following branches: " + string.Join(", ", zippedRepo.GetBranches()));

			this.logger.Log("Initialize target repository...");
			var targetRepo = InitTargetRepo();

			this.logger.Log("Building target repository...");
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
				this.logger.Log("Fetching " + name + "...");
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

			if (!this.config.NoMerges)
			{
				GraftMerges(repo, source);
			}
		}

		private void CherryPickCommits(Repository repo, Commit[] commits, string branchName)
		{
			this.logger.Log("Zipping branch " + branchName + "...");
			Commit previous = null;
			for (int i = 0; i < commits.Length; i++)
			{
				var original = commits[i];
				this.logger.Log((100 * (i + 1) / commits.Length) + "% Zipping commit " + original.Sha, replace: true);

				if (commitMap.ContainsKey(original.Sha))
				{
					if (repo.Branches[branchName] == null)
					{
						previous = commitMap[original.Sha];
					}
					else
					{
						// FIXME This should ideally be done by rearranging the history
						this.logger.Log("... cherry-picked");
						previous = CherryPickCommit(repo, original);
					}
					continue;
				}


				if (repo.Branches[branchName] == null)
				{
					repo.Checkout(repo.CreateBranch(branchName, previous ?? original));
					if (previous == null)
					{
						commitMap[original.Sha] = original;
						continue;
					}
				}

				previous = CherryPickCommit(repo, original);
				commitMap[original.Sha] = previous;
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
				return this.CommitWorktree(repo, commit);
			}
			catch (Exception e)
			{
				if (!config.Retry)
				{
					throw;
				}

				this.logger.Log("An error occurred: \n" + e);
				this.logger.Log("Press any key after fixing conflicts manually.", true);
				Console.ReadKey();

				return this.CommitWorktree(repo, commit);
			}
		}

		/// <summary>
		/// Commits the worktree with the commit meta data from the given commit.
		/// This allows creating an empty commit.
		/// </summary>
		private Commit CommitWorktree(Repository repo, Commit commit)
		{
			return repo.Commit(commit.Message, commit.Author, commit.Author, new CommitOptions {
				AllowEmptyCommit = true
			});
		}

		void GraftMerges(Repository repo, ZippedRepo source)
		{
			this.logger.Log("Grafting merges...");
			var allMerges = source.GetMerges().ToList();
			var knownMerges = allMerges.Where(m => commitMap.ContainsKey(m.Sha)).ToList();
			this.logger.Log("Unknown merges: " + string.Join(", ", allMerges.Except(knownMerges)));
			var originalMerges = knownMerges.ToDictionary(merge => commitMap[merge.Sha]);

			int count = 0;
			this.logger.Log("0% Grafting " + originalMerges.Keys.Count + " commits", replace: true);
			repo.Refs.RewriteHistory(new RewriteHistoryOptions {
				CommitParentsRewriter = commit =>
				{
					this.logger.Log((100 * ++count / originalMerges.Keys.Count) + "% Grafting commit " + commit.Sha, replace: true);

					Commit[] parents = originalMerges[commit].Parents
						.Where(m => commitMap.ContainsKey(m.Sha))
						.Select(parent => commitMap[parent.Sha]).ToArray();
					if (!parents.Any())
					{
						return commit.Parents;
					}

					// ensure to take first zipped parent
					parents[0] = commit.Parents.First();

					return parents;
				}
			}, originalMerges.Keys);

			// cleanup original refs
			foreach (var @ref in repo.Refs.FromGlob("refs/original/*"))
			{
				repo.Refs.Remove(@ref);
			}
		}
	}
}