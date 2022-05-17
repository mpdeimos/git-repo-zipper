using System;
using NUnit.Framework;
using Mpdeimos.GitRepoZipper.Util;
using Mpdeimos.GitRepoZipper.Scenario;
using System.Collections.Generic;
using System.Linq;

namespace Mpdeimos.GitRepoZipper.Model
{
	/// <summary>
	/// Tests the ZippedRepo class.
	/// </summary>
	[TestFixture]
	public class ZippedRepoTest : RepoTestBase
	{
		/// <summary>
		/// Tests that orphaned cannot be zipped (yet).
		/// </summary>
		[Test]
		public void TestFailOrphanedBranch()
		{
			Assert.Throws<ZipperException>(() => new ZippedRepo(GetTestRepos(TestData.GitOrphanedBranch)));
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
		/// Tests getting merges of a zipped repo.
		/// </summary>
		[Test, TestCaseSource(nameof(TestCaseData))]
		public void TestGetMerges(ZipScenario scenario)
		{
			var merges = new ZippedRepo(GetTestRepos(scenario.Sources)).GetMerges().ToList();
			Assert.That(merges.Select(Sha), Is.EquivalentTo(scenario.Merges.Select(c => c.Sha)));
			Assert.That(merges.Select(c => c.Parents.Select(Sha).ToArray()), Is.EqualTo(scenario.Merges.Select(c => c.Parents)));
		}
	}
}

