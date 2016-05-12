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
		/// Gets the path to a test repository.
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
	}
}