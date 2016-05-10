using System;
using LibGit2Sharp;
using System.Linq;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoZipper.Util
{
	/// <summary>
	/// Utility class for managing git repositories.
	/// </summary>
	public static class RepoUtil
	{
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

		/// <summary>
		/// Returns the merges into a branch. The key is the merge source commit, the values are the merge target commits.
		/// </summary>
		public static Dictionary<Commit, HashSet<Commit>> GetMergesBySource(Repository repo)
		{
			var merges = new Dictionary<Commit, HashSet<Commit>>();
			foreach (Commit target in GetAllCommits(repo))
			{
				foreach (Commit source in target.Parents.Skip(1))
				{
					if (!merges.ContainsKey(source))
					{
						merges[source] = new HashSet<Commit>();
					}
					
					merges[source].Add(target);
				}
			}

			return merges;
		}

		/// <summary>
		/// Returns the merge commits of a repository.
		/// </summary>
		public static IEnumerable<Commit> GetMerges(Repository repo)
		{
			return GetAllCommits(repo).Where(c => c.Parents.Count() > 1);
		}

		public static HashSet<Commit> GetAllCommits(Repository repo)
		{
			var commits = new HashSet<Commit>();
			foreach (Commit rootRef in GetReferenccedCommits(repo))
			{
				CollectCommits(commits, rootRef);
			}

			return commits;
		}

		private static void CollectCommits(HashSet<Commit> commits, Commit commit)
		{
			if (commits.Add(commit))
			{
				foreach (Commit parent in commit.Parents)
				{
					CollectCommits(commits, parent);
				}
			}
		}

		public static IEnumerable<Commit> GetReferenccedCommits(Repository repo)
		{
			return repo.Refs.Select(r => r.ResolveToDirectReference()).Distinct().Select(o => o.Target).OfType<Commit>();
		}
	}
}

