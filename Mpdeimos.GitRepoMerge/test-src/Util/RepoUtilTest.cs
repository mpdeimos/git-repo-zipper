using System;
using NUnit.Framework;
using LibGit2Sharp;
using System.Linq;

namespace Mpdeimos.GitRepoMerge.Util
{
	[TestFixture]
	public class RepoUtilTest : RepoTestBase
	{
		[TestCase(TestData.GitTwoSimpleBranchesA, "9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8",
			Result = new [] {
				"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8",
				"34fbde4c98cc29773cd3483aa44c26d5cca816f5"
			})]
		[TestCase(TestData.GitTwoSimpleBranchesA, "9f9d6e3068f26f00b076b10380764a3519490486",
			Result = new [] {
				"9f9d6e3068f26f00b076b10380764a3519490486",
				"34fbde4c98cc29773cd3483aa44c26d5cca816f5"
			})]
		[TestCase(TestData.GitTwoSimpleBranchesA, "34fbde4c98cc29773cd3483aa44c26d5cca816f5",
			Result = new []{ "34fbde4c98cc29773cd3483aa44c26d5cca816f5" })]
		public string[] TestGetPrimaryParents(string path, string commitSha)
		{
			var repo = GetTestRepo(path);
			var commit = repo.Lookup<Commit>(commitSha);
			return RepoUtil.GetPrimaryParents(commit).Select(c => c.Sha).ToArray();
		}
	}
}

