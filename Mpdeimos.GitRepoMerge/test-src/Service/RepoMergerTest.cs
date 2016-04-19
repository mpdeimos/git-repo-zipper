using System;
using NUnit.Framework;
using LibGit2Sharp;
using Mpdeimos.GitRepoMerge.Util;

namespace Mpdeimos.GitRepoMerge.Service
{
	/// <summary>
	/// Test for the RepoMeger class.
	/// </summary>
	[TestFixture]
	public class RepoMergerTest : RepoTestBase
	{
		[Test]
		public void TestGetMergedBranches()
		{
			var merger = new RepoMerger(new []{ GetTestRepo(TestData.GitTwoSimpleBranchesA) });
			Assert.That(merger.GetMergedBranches(), Is.EquivalentTo(new [] {
				"master",
				"1"
			}));
		}

		[Test]
		public void TestFailOrphanedBranch()
		{
			var merger = new RepoMerger(new []{ GetTestRepo(TestData.GitOrphanedBranch) });
			Assert.Throws<MergeException>(() => merger.Merge());
		}
	}
}

