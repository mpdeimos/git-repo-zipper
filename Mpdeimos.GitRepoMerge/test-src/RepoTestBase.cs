using System.IO;
using LibGit2Sharp;

namespace Mpdeimos.GitRepoMerge
{
	public class RepoTestBase
	{
		/// <summary>
		/// The test data manager.
		/// </summary>
		protected static TestData TestData { get; } = new TestData();

		protected const string GitTwoSimpleBranchesA = "twoSimpleBranchesA";
		protected const string GitTwoSimpleBranchesB = "twoSimpleBranchesB";
		protected const string GitTwoSimpleBranchesC = "twoSimpleBranchesC";
		protected const string GitUnnamedBranchA = "unnamedBranchA";

		/// <summary>
		/// Gets the path to a test repository.
		/// </summary>
		protected static string GetTestRepoPath(string name)
		{
			return TestData.GetPath(Path.Combine(name, "dot_git"));
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