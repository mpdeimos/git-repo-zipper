using System;
using System.Linq;
using LibGit2Sharp;
using System.Collections.Generic;
using Mpdeimos.GitRepoMerge.Util;

namespace Mpdeimos.GitRepoMerge.Model
{
	/// <summary>
	/// Abstraction of a merged repository.
	/// </summary>
	public class MergedRepo
	{
		/// <summary>
		/// The known commits of the merged repository.
		/// </summary>
		private HashSet<Commit> Commits = new HashSet<Commit>();

		/// <summary>
		/// List of all merge commits.
		/// </summary>
		private List<Commit> Merges = new List<Commit>();

		/// <summary>
		/// The named branches in the merged repository.
		/// </summary>
		private Dictionary<string, MergedBranch> Branches = new Dictionary<string, MergedBranch>();

		/// <summary>
		/// Adds a branch to the merged repository. Returns the commits in oldes-to-newest order.
		/// </summary>
		public List<Commit> AddBranch(string name, Branch branch)
		{
			if (!this.Branches.ContainsKey(name))
			{
				this.Branches[name] = new MergedBranch(name);
			}

			var mergedBranch = this.Branches[name].AddBranch(branch);
			RecordCommits(mergedBranch);

			return mergedBranch;
		}

		/// <summary>
		/// Records a list of commits.
		/// </summary>
		private void RecordCommits(IEnumerable<Commit> commits)
		{
			foreach (var commit in commits)
			{
				if (this.Commits.Contains(commit))
				{
					continue;
				}

				this.Commits.Add(commit);
				if (commit.Parents.Count() > 1)
				{
					this.Merges.Add(commit);
					foreach (var parent in commit.Parents.Skip(1))
					{
						this.RecordCommits(RepoUtil.GetPrimaryParents(parent));
					}
				}
			}
		}

		public IEnumerable<string> GetBranches()
		{
			return this.Branches.Keys;
		}

		public IEnumerable<Commit> GetBranch(string name)
		{
			return this.Branches[name].GetMergedBranch();
		}

		// TODO Test
		public IEnumerable<Commit> GetAnonymousBranchCommits()
		{
			var mergeParents = new HashSet<Commit>(this.Merges.SelectMany(merge => merge.Parents));
			foreach (var branchCommit in this.Branches.SelectMany(entry => entry.Value.GetMergedBranch()))
			{
				mergeParents.Remove(branchCommit);
			}

			return mergeParents;
		}

		public IEnumerable<Commit> GetMerges()
		{
			return this.Merges;
		}
	}
}

