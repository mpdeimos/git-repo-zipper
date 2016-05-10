using System;
using NUnit.Framework;
using LibGit2Sharp;
using Mpdeimos.GitRepoZipper.Util;
using Mpdeimos.GitRepoZipper.Model;

namespace Mpdeimos.GitRepoZipper
{
	/// <summary>
	/// Test for the RepoZipper class.
	/// </summary>
	[TestFixture]
	public class RepoZipperTest : RepoTestBase
	{
		// TODO (MP) Remove this method and move test to other util
		// TODO (MP) Create GetTestRepoPaths test method
		[Test]
		public void TestGetMergedBranches()
		{
			var zipper = new RepoZipper(new Config { Sources = new []{ GetTestRepoPath(TestData.GitTwoSimpleBranchesA) } });
			Assert.That(zipper.GetZippedBranches(), Is.EquivalentTo(new [] {
				"master",
				"1"
			}));
		}

		[Test]
		public void TestInvalidRepos()
		{
			var zipper = new RepoZipper(new Config{ Sources = new []{ "/non/existing" } });
			Assert.Throws<RepositoryNotFoundException>(() => zipper.Zip());
		}

		[Test]
		public void TestFailOrphanedBranch()
		{
			var zipper = new RepoZipper(new Config { Sources = new []{ GetTestRepoPath(TestData.GitOrphanedBranch) } });
			Assert.Throws<ZipperException>(() => zipper.Zip());
		}

	}
}

