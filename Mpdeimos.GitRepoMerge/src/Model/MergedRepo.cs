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

		public void AddCommit(string branch, Commit commit)
		{
			
		}

		public void RecordMerge(Commit from, Commit to)
		{
			
		}
	}
}

