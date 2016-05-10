using System;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
using Mpdeimos.GitRepoZipper.Util;

namespace Mpdeimos.GitRepoZipper.Model
{
	/// <summary>
	/// Represents a branch for multiple repositories.
	/// </summary>
	public class ZippedBranch
	{
		/// <summary>
		/// The name of the branch.
		/// </summary>
		private string name;

		/// <summary>
		/// The branches of single repositories.
		/// </summary>
		private List<List<Commit>> branches = new List<List<Commit>>();

		/// <summary>
		/// Constructor.
		/// </summary>
		public ZippedBranch(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Adds the commits of a branch in oldest-to-newest order. and returns the list of commits.
		/// </summary>
		public List<Commit> AddBranch(Branch branch)
		{
			var commits = RepoUtil.GetPrimaryParents(branch.Tip).Reverse().ToList();
			this.branches.Add(commits);
			return commits;
		}

		/// <summary>
		/// Returns the zipped commits in oldest-to-newest-oder.
		/// </summary>
		public IEnumerable<Commit> GetZippedBranch()
		{
			var queue = this.branches.Select(commits => new Queue<Commit>(commits)).ToList();
			while (queue.Count > 0)
			{
				var next = queue.Aggregate((acc, cur) => acc.Peek().Author.When < cur.Peek().Author.When ? acc : cur);
				yield return next.Dequeue();

				if (next.Count == 0)
				{
					queue.Remove(next);
				}
			}
		}
	}
}

