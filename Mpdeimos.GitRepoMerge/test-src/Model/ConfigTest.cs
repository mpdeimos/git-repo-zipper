﻿using System;
using NUnit.Framework;
using LibGit2Sharp;
using System.Linq;
using System.IO;

namespace Mpdeimos.GitRepoMerge.Model
{
	[TestFixture]
	public class ConfigTest : RepoTestBase
	{
		[Test]
		public void TestAllNull()
		{
			var config = new Config(null, null);

			Assert.IsNull(config.Target);
			Assert.IsNull(config.Sources);
		}

		[Test]
		public void TestWithOneRepo()
		{
			var config = new Config(null, GetTestRepoPath(TestData.GitTwoSimpleBranchesA));

			var repo = config.Sources.First();
			Assert.AreEqual(GetTestRepoPath(TestData.GitTwoSimpleBranchesA), GetRepoPath(repo));
		}

		[Test]
		public void TestWithTwoRepos()
		{
			var config = new Config(null, GetTestRepoPath(TestData.GitTwoSimpleBranchesA), GetTestRepoPath(TestData.GitTwoSimpleBranchesB));
			Assert.That(config.Sources.Select(GetRepoPath), Is.EquivalentTo(new [] {
				GetTestRepoPath(TestData.GitTwoSimpleBranchesA),
				GetTestRepoPath(TestData.GitTwoSimpleBranchesB)
			}));
		}

		[Test]
		public void TestInvalidRepos()
		{
			var config = new Config(null, "/non/existing");
			Assert.Throws<RepositoryNotFoundException>(() => config.Sources.First());
		}

		private static string GetRepoPath(Repository repo)
		{
			return repo.Info.Path.TrimEnd(Path.DirectorySeparatorChar);
		}
	}
}
