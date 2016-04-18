using System;
using NUnit.Framework;
using Mpdeimos.GitRepoMerge.Util;
using System.Linq;

namespace Mpdeimos.GitRepoMerge.Model
{
	[TestFixture]
	public class MergedBranchTest : RepoTestBase
	{
		[Test]
		public void TestGetMergedBranch()
		{
			var repo = GetTestRepo(GitTwoSimpleBranchesA);
			var merged = new MergedBranch("foo");
			// TODO (MP) accept a branch object here
			merged.AddBranch(RepoUtil.GetPrimaryParents(repo.Branches["master"].Tip).Reverse().ToList());
			Assert.That(merged.GetMergedBranch().Select(c => c.Sha), Is.EqualTo(new [] {
				"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
				"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8"
			}));
		}
	}
}

