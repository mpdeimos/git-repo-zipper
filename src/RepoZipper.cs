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
			var zippedRepo = ZipRepository();
			var targetRepo = InitRepository();
			BuildRepository(targetRepo, zippedRepo);
			return targetRepo;
		}

		private ZippedRepo ZipRepository()
		{
			var zippedRepo = new ZippedRepo();
			foreach (var repo in this.repositories)
			{
				Commit commonRoot = null;
				foreach (var branch in repo.Branches.Where(b => b.IsRemote == false))
				{
					List<Commit> commits = zippedRepo.AddBranch(branch.FriendlyName, branch);
					if (commonRoot == null)
					{
						commonRoot = commits.First();
					}
					if (commits.First() != commonRoot)
					{
						throw new ZipperException("Cannot zip repositories with multiple roots. See https://github.com/mpdeimos/git-repo-merge/issues/1 for details.");
					}
				}
			}
			return zippedRepo;
		}

		Repository InitRepository()
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
				targetRepo.Fetch(name);
			}
			return targetRepo;
		}

		private void BuildRepository(Repository repo, ZippedRepo source)
		{
			foreach (string name in source.GetBranches())
			{
				var commits = source.GetBranch(name);
				CherryPickCommits(repo, commits, name);
			}

			// TODO (MP) Handle anon branches

			GraftMerges(repo, source);

			// TODO (MP) Test
		}

		private void CherryPickCommits(Repository repo, IEnumerable<Commit> commits, string branchName)
		{
			Commit branchPoint = null;
			foreach (Commit original in commits)
			{
				if (commitMap.ContainsKey(original))
				{
					branchPoint = commitMap[original];
					continue;
				}


				if (repo.Branches[branchName] == null)
				{
					repo.Checkout(repo.CreateBranch(branchName, branchPoint ?? original));
					if (branchPoint == null)
					{
						commitMap[original] = original;
						continue;
					}
				}

				var commit = repo.Lookup(original.Sha) as Commit;
				var options = new CherryPickOptions();
				if (commit.Parents.Count() > 1)
				{
					options.Mainline = 1;
				}
				var result = repo.CherryPick(commit, 
					             new Signature(commit.Author.Name, commit.Author.Email, commit.Author.When),
					             options);
				commitMap[original] = result.Commit;
			}
		}

		void GraftMerges(Repository repo, ZippedRepo source)
		{
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
	}
}

