using System;
using LibGit2Sharp;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoMerge.Model
{
	/// <summary>
	/// Abstraction of a merged repository.
	/// </summary>
	public class MergedRepo
	{
		private Dictionary<Commit, Commit> Merges = new Dictionary<Commit, Commit>();

		private Dictionary<string, MergedBranch> Branches = new Dictionary<string, MergedBranch>();

		/// <summary>
		/// Adds a branch to the merged repository. The commits need to be in oldes-to-newest order.
		/// </summary>
		public void AddBranch(string name, List<Commit> commits)
		{
			if (!this.Branches.ContainsKey(name))
			{
				this.Branches[name] = new MergedBranch(name);
			}

			this.Branches[name].AddBranch(commits);
		}

		public void RecordMerge(Commit from, Commit to)
		{
			
		}
	}
}

