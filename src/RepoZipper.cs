﻿using System;
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
		private readonly IEnumerable<string> repositories;

		/// <summary>
		/// Maps original commits to zipped ones.
		/// </summary>
		private readonly Dictionary<string, ShallowCommit> commitMap = new Dictionary<string, ShallowCommit>();

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
			this.logger = new Logger { Silent = config.Silent };
			this.repositories = config.Sources;
		}

		/// <summary>
		/// Zips the configured repositories.
		/// </summary>
		public IRepository Zip()
		{
			this.logger.Log("Reading repositories...");
			var zippedRepo = new ZippedRepo(this.repositories, this.config);
			this.logger.Log("Zipping the following branches: " + string.Join(", ", zippedRepo.GetBranches()));

			this.logger.Log("Initialize target repository...");
			IRepository targetRepo = InitTargetRepo();

			this.logger.Log("Building target repository...");
			BuildRepository(targetRepo, zippedRepo);

			if (!config.Keep)
			{
				foreach (string repoPath in this.repositories)
				{
					using (var repo = new Repository(repoPath))
					{
						string name = Path.GetFileName(repo.Info.WorkingDirectory.TrimEnd(Path.DirectorySeparatorChar));
						targetRepo.Network.Remotes.Remove(name);
					}
				}
			}

			if (!config.Tags)
			{
				DeleteTags(targetRepo);
			}

			return targetRepo;
		}

		/// <summary>
		/// Initializes the target repository.
		/// </summary>
		private IRepository InitTargetRepo()
		{
			if (this.config.DryRun)
			{
				return new DryRepository();
			}

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
			foreach (string repoPath in this.repositories)
			{
				using (var repo = new Repository(repoPath))
				{
					string name = Path.GetFileName(repo.Info.WorkingDirectory.TrimEnd(Path.DirectorySeparatorChar));
					targetRepo.Network.Remotes.Add(name, repo.Info.Path);
					var remote = targetRepo.Network.Remotes[name];
					this.logger.Log("Fetching " + name + "...");
					var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
					var options = new FetchOptions();
					Commands.Fetch(targetRepo, remote.Name, refSpecs, options, "Fetching " + name + "...");
				}
			}
			return targetRepo;
		}

		private void BuildRepository(IRepository repo, ZippedRepo source)
		{
			foreach (string name in source.GetBranches())
			{
				var commits = source.GetBranch(name);
				CherryPickCommits(repo, commits.ToArray(), name);
			}

			// TODO (MP) Handle anon branches

			RecordMerges(repo, source);
		}

		private void DeleteTags(IRepository repo)
		{
			foreach (var refs in repo.Refs)
			{
                if (refs.IsTag)
                {
					logger.Log($"Removing tag {refs.CanonicalName}...");
					repo.Refs.Remove(refs);
                }
			}
		}

		private void CherryPickCommits(IRepository repo, ShallowCommit[] commits, string branchName)
		{
			this.logger.Log("Zipping branch " + branchName + "...");
			ShallowCommit previous = null;
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
					var branch = repo.CreateBranch(branchName, (previous ?? original).Sha);
					Commands.Checkout(repo, branchName);

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

		private ShallowCommit CherryPickCommit(IRepository repo, ShallowCommit original)
		{
			if (this.config.DryRun)
			{
				return original;
			}

			var commit = repo.Lookup(original.Sha) as Commit;
			var options = new CherryPickOptions();
			if (commit.Parents.Count() > 1)
			{
				options.Mainline = 1;
			}

			options.FileConflictStrategy = CheckoutFileConflictStrategy.Ours;
			options.FailOnConflict = false;
			options.CommitOnSuccess = true;
			options.MergeFileFavor = MergeFileFavor.Ours;

			try
			{ 
				var cherrypick = repo.CherryPick(commit, new Signature(commit.Author.Name, commit.Author.Email, commit.Author.When), options);
				if (cherrypick.Status == CherryPickStatus.Conflicts)
				{
					this.logger.Log("There was a conflict when cherry picking " + commit.Message);
					
					foreach(var conflict in repo.Index.Conflicts)
                    {
						this.logger.Log("Conflict: " + conflict.ToString());
					}
				}
				return ShallowCommit.FromCommit(cherrypick.Commit);
			}
			catch (EmptyCommitException)
			{
				return ShallowCommit.FromCommit(this.CommitWorktree(repo, commit));
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

				return ShallowCommit.FromCommit(this.CommitWorktree(repo, commit));
			}
		}

		/// <summary>
		/// Commits the worktree with the commit meta data from the given commit.
		/// This allows creating an empty commit.
		/// </summary>
		private Commit CommitWorktree(IRepository repo, Commit commit)
		{
			return repo.Commit(commit.Message, commit.Author, commit.Author, new CommitOptions {
				AllowEmptyCommit = true
			});
		}

		void RecordMerges(IRepository repo, ZippedRepo source)
		{
			this.logger.Log("Recording merges...");
			var allMerges = source.GetMerges().ToList();
			var knownMerges = allMerges.Where(m => commitMap.ContainsKey(m.Sha)).ToList();
			this.logger.Log("Unknown merges: " + string.Join(", ", allMerges.Except(knownMerges).Select(m =>$"{m.Sha} {m.Author}")));
			var originalMerges = knownMerges.ToDictionary(merge => commitMap[merge.Sha]);

			if (this.config.DryRun)
			{
				return;
			}

			if (this.config.GraftMerges)
			{
				GraftMerges(repo, originalMerges);
			}
			else
			{
				RewriteMerges(repo, originalMerges);
			}
		}

		private void GraftMerges(IRepository repo, Dictionary<ShallowCommit, ShallowCommit> originalMerges)
		{
			int count = 0;
			this.logger.Log("0% Grafting " + originalMerges.Keys.Count + " commits", replace: true);
			var grafts = originalMerges.Keys.Select(commit =>
			{
				this.logger.Log((100 * ++count / originalMerges.Keys.Count) + "% Grafting commit " + commit.Sha, replace: true);
				var parents = new List<ShallowCommit> { commit };
				parents.AddRange(TranslateMergeParents(commit, originalMerges));
				return string.Join(" ", parents.Select(c => c.Sha));
			
			});

			var graftsFile = Path.Combine(this.config.Target, ".git/info/grafts");
			File.WriteAllLines(graftsFile, grafts);
		}


		private void RewriteMerges(IRepository repo, Dictionary<ShallowCommit, ShallowCommit> originalMerges)
		{
			//int count = 0;
			//this.logger.Log("0% Rewriting " + originalMerges.Keys.Count + " commits", replace: true);
			//repo.Refs.RewriteHistory(new RewriteHistoryOptions {
			//	CommitParentsRewriter = commit =>
			//	{
			//		this.logger.Log((100 * ++count / originalMerges.Keys.Count) + "% Rewriting commit " + commit.Sha, replace: true);
			//		return TranslateMergeParents(ShallowCommit.FromCommit(commit), originalMerges).Select(p => repo.Lookup(p.Sha) as Commit);
			//	}
			//}, originalMerges.Keys.Select(p => repo.Lookup(p.Sha) as Commit));

			//// cleanup original refs
			//foreach (var @ref in repo.Refs.FromGlob("refs/original/*"))
			//{
			//	repo.Refs.Remove(@ref);
			//}
		}

		private IEnumerable<ShallowCommit> TranslateMergeParents(ShallowCommit commit, Dictionary<ShallowCommit, ShallowCommit> originalMerges)
		{

			ShallowCommit[] parents = originalMerges[commit].Parents.Where(m => commitMap.ContainsKey(m.Sha)).Select(parent => commitMap[parent.Sha]).ToArray();
			if (!parents.Any())
			{
				return commit.Parents;
			}
			// ensure to take first zipped parent
			parents[0] = commit.Parents.First();
			return parents;
		}
	}
}