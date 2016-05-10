using System;
using NUnit.Framework;
using LibGit2Sharp;
using System.Linq;
using System.IO;

namespace Mpdeimos.GitRepoZipper.Model
{
	[TestFixture]
	public class ConfigTest : RepoTestBase
	{
		// TODO Add tests for default values
		// TODO Add tests for commandline parsing

		[Test]
		public void TestAllNull()
		{
			var config = new Config();

			Assert.IsNull(config.Target);
			Assert.IsNull(config.Sources);
		}

		[Test]
		public void TestWithOneRepo()
		{
			var config = new Config { Sources = new [] { GetTestRepoPath(TestData.GitTwoSimpleBranchesA) } };

			var repo = config.Sources.First();
			Assert.AreEqual(GetTestRepoPath(TestData.GitTwoSimpleBranchesA), GetRepoPath(repo));
		}

		[Test]
		public void TestWithTwoRepos()
		{
			var config = new Config {
				Sources = new [] {GetTestRepoPath(TestData.GitTwoSimpleBranchesA),	GetTestRepoPath(TestData.GitTwoSimpleBranchesB)
				}
			};
			Assert.That(config.Sources.Select(GetRepoPath), Is.EquivalentTo(new [] {
				GetTestRepoPath(TestData.GitTwoSimpleBranchesA),
				GetTestRepoPath(TestData.GitTwoSimpleBranchesB)
			}));
		}

		private static string GetRepoPath(string repo)
		{
			return new Repository(repo).Info.Path.TrimEnd(Path.DirectorySeparatorChar);
		}
	}
}

