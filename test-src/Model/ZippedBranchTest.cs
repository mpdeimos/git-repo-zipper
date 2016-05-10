using System;
using NUnit.Framework;
using Mpdeimos.GitRepoZipper.Util;
using System.Linq;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoZipper.Model
{
	[TestFixture]
	public class ZippedBranchTest : RepoTestBase
	{
		public IEnumerable<TestCaseData> TestCaseData
		{
			get
			{
				foreach (var scenario in ZipScenario.Scenarios)
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
		public string[] TestGetZippedBranch(string[] repos, string branch)
		{
			var zipped = new ZippedBranch(branch);
			foreach (string repo in repos)
			{
				var gitBranch = GetTestRepo(repo).Branches[branch];
				if (gitBranch != null)
				{
					zipped.AddBranch(gitBranch);
				}
			}

			return zipped.GetZippedBranch().Select(c => c.Sha).ToArray();
		}
	}
}

