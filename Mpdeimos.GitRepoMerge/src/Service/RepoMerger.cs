using System;
using System.Collections.Generic;
using LibGit2Sharp;
using Mpdeimos.GitRepoMerge.Model;
using Mpdeimos.GitRepoMerge.Util;
using System.Linq;

namespace Mpdeimos.GitRepoMerge.Service
{
	/// <summary>
	/// Merges multiple Git repositories into a stream of MergedCommit objects.
	/// </summary>
	public class RepoMerger
	{
		/// <summary>
		/// The repositories to merge.
		/// </summary>
		private readonly IEnumerable<Repository> repositories;

		/// <summary>
		/// Constructor.
		/// </summary>
		public RepoMerger(IEnumerable<Repository> repositories)
		{
			this.repositories = repositories;
		}

		/// <summary>
		/// Returns the names of the merged branches.
		/// </summary>
		public IEnumerable<string> GetMergedBranches()
		{
			var branches = new HashSet<string>();
			foreach (var repo in this.repositories)
			{
				foreach (var branch in repo.Branches)
				{
					branches.Add(branch.FriendlyName);
				}
			}
			
			return branches;
		}

		/// <summary>
		/// Merges the provided repositories.
		/// </summary>
		public MergedRepo Merge()
		{
			var mergedRepo = new MergedRepo();
			foreach (var repo in this.repositories)
			{
				Commit commonRoot = null;
				foreach (var branch in repo.Branches.Where(b => b.IsRemote == false))
				{
					List<Commit> commits = RepoUtil.GetPrimaryParents(branch.Tip).Reverse().ToList();
					if (commonRoot == null)
					{
						commits.First();
					}

					if (commits.First() != commonRoot)
					{
						throw new MergeException("Cannot merge repositories with multiple roots. See https://github.com/mpdeimos/git-repo-merge/issues/1 for details.");
					}

					mergedRepo.AddBranch(branch.FriendlyName, commits);
					
				}
			}

			return mergedRepo;
		}
	}
}

