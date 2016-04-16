using System.IO;

namespace Mpdeimos.GitRepoMerge
{
	public class RepoTestBase
	{
		/// <summary>
		/// The test data manager.
		/// </summary>
		protected static TestData TestData { get; } = new TestData();

		protected readonly string GitA = GetTestRepoPath("gitA");
		protected readonly string GitB = GetTestRepoPath("gitB");
		protected readonly string GitC = GetTestRepoPath("gitC");
		protected readonly string GitD = GetTestRepoPath("gitD");

		/// <summary>
		/// Gets the path to a test repository.
		/// </summary>
		protected static string GetTestRepoPath(string name)
		{
			return TestData.GetPath(Path.Combine(name, "dot_git"));
		}
	}
}