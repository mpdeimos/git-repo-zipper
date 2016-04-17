using System;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace Mpdeimos.GitRepoMerge.Model
{
	/// <summary>
	/// Represents a branch for multiple repositories.
	/// </summary>
	public class MultiRepoBranch
	{
		/// <summary>
		/// The name of the branch.
		/// </summary>
		private string name;

		/// <summary>
		/// The 
		/// </summary>
		private List<IEnumerable<Commit>> branches = new List<IEnumerable<Commit>>();

		/// <summary>
		/// Constructor.
		/// </summary>
		public MultiRepoBranch(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Adds the branch.
		/// </summary>
		/// <param name="branch">Branch.</param>
		public void AddBranch(Branch branch)
		{
//			MergedCommit child = new MergedCommit(branch.Tip, null);
//			while (child.Commit.Parents?.FirstOrDefault() != null)
//			{
//				child.Commit.Parents?.FirstOrDefault()
//			}
//
//			foreach (var commit in branch.Tip)
//			{
//				;
//			}
		}
	}
}

