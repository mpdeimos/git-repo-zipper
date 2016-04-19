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
		/// Adds a branch to the merged repository. Returns the commits in oldes-to-newest order.
		/// </summary>
		public List<Commit> AddBranch(string name, Branch branch)
		{
			if (!this.Branches.ContainsKey(name))
			{
				this.Branches[name] = new MergedBranch(name);
			}

			return this.Branches[name].AddBranch(branch);
		}

		public void RecordMerge(Commit from, Commit to)
		{
			
		}
	}
}

