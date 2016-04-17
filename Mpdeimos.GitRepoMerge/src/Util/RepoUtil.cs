using System;
using LibGit2Sharp;
using System.Linq;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoMerge.Util
{
	/// <summary>
	/// Utility class for managing git repositories.
	/// </summary>
	public static class RepoUtil
	{
		/// <summary>
		/// Returns the branch for the given name or <c>null</c>.
		/// </summary>
		public static Branch GetBranch(Repository repo, string name)
		{
			return repo.Branches.FirstOrDefault(branch => branch.FriendlyName == name);
		}

		/// <summary>
		/// Recursively returns all parents of the given commit. Each returned commit will
		/// be the first parent of the previous commit. This will not return commits that
		/// are not the first parent (e.g. merged commits).
		/// </summary>
		public static IEnumerable<Commit> GetPrimaryParents(Commit commit)
		{
			while (commit != null)
			{
				yield return commit;
				commit = commit.Parents?.FirstOrDefault();
			}
		}
	}
}

