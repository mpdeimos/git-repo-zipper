using System.IO;
using LibGit2Sharp;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace Mpdeimos.GitRepoZipper
{
	public class RepoTestBase
	{
		/// <summary>
		/// The test data manager.
		/// </summary>
		protected static TestData TestData { get; } = new TestData();

		/// <summary>
		/// Gets the path to a test repository.
		/// </summary>
		protected static string GetTestRepoPath(string name)
		{
			return TestData.GetPath(Path.Combine(name, "dot_git"));
		}

		/// <summary>
		/// Gets the paths to the test repositories.
		/// </summary>
		protected static string[] GetTestRepoPaths(params string[] names)
		{
			return names.Select(GetTestRepoPath).ToArray();
		}

		/// <summary>
		/// Gets the test repository.
		/// </summary>
		protected static Repository GetTestRepo(string name)
		{
			return new Repository(GetTestRepoPath(name));
		}

		/// <summary>
		/// Gets the test repositories.
		/// </summary>
		protected static IEnumerable<Repository> GetTestRepos(params string[] names)
		{
			return names.Select(GetTestRepo);
		}

		/// <summary>
		/// Returns the Sha of a commit.
		/// </summary>
		protected static string Sha(Commit commit)
		{
			return commit.Sha;
		}
	}
}