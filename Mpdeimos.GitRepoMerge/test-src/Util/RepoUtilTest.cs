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

		[TestCase(TestData.GitTwoSimpleBranchesA, new [] {
			"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8",
			"9f9d6e3068f26f00b076b10380764a3519490486"
		})]
		[TestCase(TestData.GitTwoSimpleBranchesB, new [] {
			"0033408fbb0366dbda1f6a86ecef041021378e8b",
			"c3eaaec75fb1dd7e1dcab7f8d2efe8bef83f7100",
		})]
		[TestCase(TestData.GitTwoSimpleBranchesC, new [] {
			"fb3e1a686cbbbdbd633e0ef703602c836b578647",
		})]
		[TestCase(TestData.GitOrphanedBranch, new [] {
			"c331256dda21a6954278fbcc777b51db4aa2af87",
			"1218ce23a03dc0b1f3f0a14dfa49e2841dd4c955"
		})]
		[TestCase(TestData.GitUnnamedBranchA, new [] {
			"264b5f79cba87583bf430e0682a733d5c4eebb2a",
		})]
		[TestCase(TestData.GitTaggedBranchA, new [] {
			"2bd7867d910fbcf28cbc7653096a0bc26a29b37e",
			"f898f7ee4608e6a86dccaa86f2e6af8467dd7be9",
			"b826257750e44f67b21d8a4c8709cab422e4592c"
		})]
		public void TestGetReferencedCommits(string path, string[] expected)
		{
			var repo = GetTestRepo(path);
			Assert.That(RepoUtil.GetReferenccedCommits(repo).Select(c => c.Sha).ToArray(), Is.EquivalentTo(expected));
		}

		[TestCase(TestData.GitTwoSimpleBranchesA, new [] {
			"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8",
			"9f9d6e3068f26f00b076b10380764a3519490486",
			"34fbde4c98cc29773cd3483aa44c26d5cca816f5"
		})]
		[TestCase(TestData.GitTwoSimpleBranchesB, new [] {
			"0033408fbb0366dbda1f6a86ecef041021378e8b",
			"c3eaaec75fb1dd7e1dcab7f8d2efe8bef83f7100",
			"26074c825ec2a948d19ffd69e71b333ee5e62f70",
			"4168bb0842f025ddb7975ebbd9a9bd7081555a6a",
			"9590d59b0d639f5cd8d1e2da03ae52161ec02b2b"
		})]
		[TestCase(TestData.GitTwoSimpleBranchesC, new [] {
			"fb3e1a686cbbbdbd633e0ef703602c836b578647",
			"296eb9d44fa9e525b60d874cf1fa2c35c60b7668"
		})]
		[TestCase(TestData.GitOrphanedBranch, new [] {
			"c331256dda21a6954278fbcc777b51db4aa2af87",
			"1218ce23a03dc0b1f3f0a14dfa49e2841dd4c955"
		})]
		[TestCase(TestData.GitUnnamedBranchA, new [] {
			"264b5f79cba87583bf430e0682a733d5c4eebb2a",
			"47a7a7703062ef41bd2a27bbceb85ba510f9267e",
			"e9ccb9674340730641fd14242a2014a25e3cae37",
			"e5bcc3fa55d952c9057d4e92a0054b2280ac2330"
		})]
		[TestCase(TestData.GitTaggedBranchA, new [] {
			"2bd7867d910fbcf28cbc7653096a0bc26a29b37e",
			"f898f7ee4608e6a86dccaa86f2e6af8467dd7be9",
			"b826257750e44f67b21d8a4c8709cab422e4592c",
			"86e97e95f374460455f39ef51ed57eda771c9ba7"
		})]
		public void TestGetAllCommits(string path, string[] expected)
		{
			var repo = GetTestRepo(path);
			Assert.That(RepoUtil.GetAllCommits(repo).Select(c => c.Sha).ToArray(), Is.EquivalentTo(expected));
		}
	}
}

