using System;
using NUnit.Framework;
using LibGit2Sharp;
using Mpdeimos.GitRepoMerge.Util;
using Mpdeimos.GitRepoMerge.Model;

namespace Mpdeimos.GitRepoMerge
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
			var merger = new RepoZipper(new Config { Sources = new []{ GetTestRepoPath(TestData.GitTwoSimpleBranchesA) } });
			Assert.That(merger.GetMergedBranches(), Is.EquivalentTo(new [] {
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
			var merger = new RepoZipper(new Config { Sources = new []{ GetTestRepoPath(TestData.GitOrphanedBranch) } });
			Assert.Throws<MergeException>(() => merger.Zip());
		}

	}
}

