using System;
using NUnit.Framework;
using LibGit2Sharp;
using Mpdeimos.GitRepoZipper.Util;
using Mpdeimos.GitRepoZipper.Model;
using System.Collections.Generic;
using Mpdeimos.GitRepoZipper.Scenario;
using System.Linq;

namespace Mpdeimos.GitRepoZipper
{
	/// <summary>
	/// Test for the RepoZipper class.
	/// </summary>
	[TestFixture]
	public class RepoZipperTest : RepoTestBase
	{
		/// <summary>
		/// Tests loading an invalid repository throws and exception.
		/// </summary>
		[Test]
		public void TestInvalidRepos()
		{
			var zipper = new RepoZipper(new Config{ Sources = new []{ "/non/existing" } });
			Assert.Throws<RepositoryNotFoundException>(() => zipper.Zip());
		}

		static public IEnumerable<TestCaseData> TestCaseData
		{
			get
			{
				foreach (var scenario in ZipScenario.Scenarios)
				{
					yield return new TestCaseData(scenario).SetName(string.Join("+", scenario.Sources));
				}
			}
		}

		/// <summary>
		/// Tests that orphaned cannot be zipped (yet).
		/// </summary>
		[Test, TestCaseSource(nameof(TestCaseData))]
		public void TestZipScenarios(ZipScenario scenario)
		{
			var config = new Config {
				Sources = GetTestRepoPaths(scenario.Sources),
				Target = TestData.GetCleanTempDir(),
				Force = true,
				Silent = true
			};
			var zipper = new RepoZipper(config);
			var repo = zipper.Zip();

			var branches = repo.Branches.Where(b => !b.IsRemote).ToList();
			Assert.That(branches.Select(b => b.FriendlyName),
				Is.EquivalentTo(scenario.Branches.Keys));

			foreach (var branch in branches)
			{
				var commits = RepoUtil.GetPrimaryParents(branch.Tip).Select(c => c.Message).Reverse();
				Assert.That(commits,
					Is.EqualTo(scenario.Branches[branch.FriendlyName].Select(sha => (repo.Lookup(sha) as Commit).Message)),
					"Commits do not match for branch: " + branch.FriendlyName
				);

			}

			// TODO this may be unified with ZippedRepoTest
			var merges = RepoUtil.GetMerges(branches.Select(b => b.Tip)).ToList(); // just retrieve merges for zipped branches
			Assert.That(merges.Select(m => m.Message), Is.EquivalentTo(scenario.Merges.Select(c => Lookup(repo, c.Sha).Message)));

			// We have to skip the 1st parent as this one may be different
			var actual = merges.Select(c => c.Parents.Skip(1).Select(p => p.Message));
			var expected = scenario.Merges.Select(c => c.Parents.Skip(1).Select(p => Lookup(repo, p).Message));
			Assert.That(actual, Is.EquivalentTo(expected));
		}

		private static Commit Lookup(IRepository repo, string sha)
		{
			return repo.Lookup(sha) as Commit;
		}
	}
}