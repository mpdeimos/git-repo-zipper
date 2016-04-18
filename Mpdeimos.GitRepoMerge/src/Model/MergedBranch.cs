using System;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace Mpdeimos.GitRepoMerge.Model
{
	/// <summary>
	/// Represents a branch for multiple repositories.
	/// </summary>
	public class MergedBranch
	{
		/// <summary>
		/// The name of the branch.
		/// </summary>
		private string name;

		/// <summary>
		/// The 
		/// </summary>
		private List<List<Commit>> branches = new List<List<Commit>>();

		/// <summary>
		/// Constructor.
		/// </summary>
		public MergedBranch(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Adds the commits of a branch in oldest-to-newest order.
		/// </summary>
		public void AddBranch(List<Commit> commits)
		{
			this.branches.Add(commits);
		}

		/// <summary>
		/// Returns the merged commits in oldest-to-newest-oder.
		/// </summary>
		public IEnumerable<Commit> GetMergedBranch()
		{
			var queue = this.branches.Select(commits => new Queue<Commit>(commits)).ToList();
			while (queue.Count > 0)
			{
				//Aggregate((curMin, x) => (curMin == null || (x.DateOfBirth ?? DateTime.MaxValue) < curMin.DateOfBirth ? x : curMin))
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

