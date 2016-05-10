using System;
using NUnit.Framework;
using LibGit2Sharp;
using System.Linq;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoZipper.Util
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

		public static readonly object[] TestGetMergesBySourceTestData = {
			new object [] {
				TestData.GitTwoSimpleBranchesA, 
				new Dictionary<string, HashSet<string>> {
					["9f9d6e3068f26f00b076b10380764a3519490486" ] = new HashSet<string>{ "9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8" }
				}
			},
			new object [] {
				TestData.GitTwoSimpleBranchesB, 
				new Dictionary<string, HashSet<string>> {
					["c3eaaec75fb1dd7e1dcab7f8d2efe8bef83f7100" ] = new HashSet<string>{ "0033408fbb0366dbda1f6a86ecef041021378e8b" }
				}
			},
			new object [] {
				TestData.GitThreeBranchesA, 
				new Dictionary<string, HashSet<string>> {
					["7f45647c57d3389a0928fdec132a500f78a798db" ] = new HashSet<string>{ "fab18bed5eca98b1c49d388dce0981b6e387332e" },
					["69defa1e1bde3add954bc04ef761b61f29823844" ] = new HashSet<string>{ "7f45647c57d3389a0928fdec132a500f78a798db" },
					["5659f607934bff01a850be292c95988a2ddaf07e" ] = new HashSet<string>{ "69defa1e1bde3add954bc04ef761b61f29823844" },
					["0be3f9fae7b4f1f16671e831550534189d932d59" ] = new HashSet<string>{ "2d666410e00e328dfb520e2ccb6c104a1e060323" },
					["124a3545c3848acd8fa227281dfa761f918976cc" ] = new HashSet<string>{ "5659f607934bff01a850be292c95988a2ddaf07e" },
					["6e8de15646dcdab9cbf2643a5c623ed5585f1978" ] = new HashSet<string>{ "5659f607934bff01a850be292c95988a2ddaf07e" },
					["355c7a69feb2d53c42bf08b51ab0732e8d8daa19" ] = new HashSet<string>{ "0be3f9fae7b4f1f16671e831550534189d932d59" },
					["8b1a2f42123b686e86fb5cea1fa75bfe5103050a" ] = new HashSet<string>{ "e78a63af4538a1e20ba50fcdc5aa33c1b212b5f7" },
				}
			},
			new object [] {
				TestData.GitThreeBranchesB, 
				new Dictionary<string, HashSet<string>> {
					["e59d978033861cd034c33b41d0866f973e275a2c" ] = new HashSet<string>{ "1411cd804254b1a94ae17dd114b06e09404faae0" },
					["4d07cfc4ab0cca825202ca1d6f04d50a37047d2b" ] = new HashSet<string>{ "1411cd804254b1a94ae17dd114b06e09404faae0" },
					["a7f47d6ccc0be64a75a1c06f716f15e03584b55c" ] = new HashSet<string> {
						"4d07cfc4ab0cca825202ca1d6f04d50a37047d2b",
						"e59d978033861cd034c33b41d0866f973e275a2c"
					},
					["06e8c24210cb2151793be89852398fb29cdaa92c" ] = new HashSet<string>{ "36b2931e54afa1e1b1a300b7855d8d867be56b9c" },
					["c6a637be29111a85d306209a1aabf20459df3f36" ] = new HashSet<string>{ "227d283a809450cbc03268be7355203799f6ffab" },
					["e4375536bce5d065ca702793bba955f252394521" ] = new HashSet<string>{ "0b148c4b67e9874c8d40cc501735936f01fb2897" },
					["46301bc5beead1a012d5fa62711cc7c41054edde" ] = new HashSet<string>{ "06e8c24210cb2151793be89852398fb29cdaa92c" },
					["3f09856b3b565919c9aeba7e8342ee18cedf8929" ] = new HashSet<string>{ "b4b4001f27ce2e6e04e964a293e7e0d358dfb4e1" },
					["776a4ab8a98293b39523c04d3b901121ac244138" ] = new HashSet<string>{ "b4b4001f27ce2e6e04e964a293e7e0d358dfb4e1" },
				}
			},
			new object [] {
				TestData.GitTwoSimpleBranchesC, new Dictionary<string, HashSet<string>>()
			},
			new object [] {
				TestData.GitUnnamedBranchA, 
				new Dictionary<string, HashSet<string>> {
					["47a7a7703062ef41bd2a27bbceb85ba510f9267e" ] = new HashSet<string>{ "264b5f79cba87583bf430e0682a733d5c4eebb2a" }
				}
			},
			new object [] {
				TestData.GitTaggedBranchA, 
				new Dictionary<string, HashSet<string>> {
					["f898f7ee4608e6a86dccaa86f2e6af8467dd7be9" ] = new HashSet<string>{ "2bd7867d910fbcf28cbc7653096a0bc26a29b37e" }
				}
			},
			new object [] {
				TestData.GitOrphanedBranch, new Dictionary<string, HashSet<string>>()
			}
		};

		[Test, TestCaseSource(nameof(TestGetMergesBySourceTestData))]
		public void TestGetMergesBySource(string path, Dictionary<string, HashSet<string>> expected)
		{
			var merges = RepoUtil.GetMergesBySource(GetTestRepo(path));
			Assert.That(merges.Keys.Select(c => c.Sha), Is.EquivalentTo(expected.Keys), "Merge sources do not match");
			foreach (Commit source in merges.Keys)
			{
				Assert.That(merges[source].Select(c => c.Sha), Is.EquivalentTo(expected[source.Sha]), $"Merge target does not match for source {source.Sha}");
			}
		}
	}
}

