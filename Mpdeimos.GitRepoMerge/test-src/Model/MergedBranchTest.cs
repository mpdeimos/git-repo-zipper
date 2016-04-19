using System;
using NUnit.Framework;
using Mpdeimos.GitRepoMerge.Util;
using System.Linq;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoMerge.Model
{
	[TestFixture]
	public class MergedBranchTest : RepoTestBase
	{
		public IEnumerable<TestCaseData> TestCaseData
		{
			get
			{
				foreach (var scenario in MergeScenario.Scenarios)
				{
					foreach (var branch in scenario.Branches.Keys)
					{
						yield return new TestCaseData(scenario.Sources, branch)
							.Returns(scenario.Branches[branch])
							.SetName(string.Join("+", scenario.Sources) + ":" + branch);
					}
				}
			}
		}

		[Test, TestCaseSource(nameof(TestCaseData))]
		public string[] TestGetMergedBranch(string[] repos, string branch)
		{
			var merged = new MergedBranch(branch);
			foreach (string repo in repos)
			{
				var gitBranch = GetTestRepo(repo).Branches[branch];
				if (gitBranch != null)
				{
					merged.AddBranch(gitBranch);
				}
			}

			return merged.GetMergedBranch().Select(c => c.Sha).ToArray();
		}
	}
}

