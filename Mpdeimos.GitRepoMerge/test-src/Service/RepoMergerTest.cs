using System;
using NUnit.Framework;
using LibGit2Sharp;

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
			var merger = new RepoMerger(new []{ new Repository(GitA) });
			Assert.That(merger.GetMergedBranches(), Is.EquivalentTo(new [] {
				"master",
				"1"
			}));
		}
	}
}

