using System;
using LibGit2Sharp;

namespace Mpdeimos.GitRepoMerge.Model
{
	/// <summary>
	/// Abstraction of a merged commit
	/// </summary>
	public class MergedCommit
	{
		public Commit Commit { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public MergedCommit(Commit commit, MergedCommit child)
		{
			this.Commit = commit;
		}
	}
}

