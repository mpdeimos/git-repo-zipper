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
				foreach (var scenario in MergeScenarios)
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
		public IEnumerable<string> TestGetMergedBranch(string[] repos, string branch)
		{
			var merged = new MergedBranch(branch);
			foreach (string repo in repos)
			{
				var git = GetTestRepo(GitTwoSimpleBranchesA);
				// TODO (MP) accept a branch object here
				merged.AddBranch(RepoUtil.GetPrimaryParents(git.Branches[branch].Tip).Reverse().ToList());
			}

			return merged.GetMergedBranch().Select(c => c.Sha);
		}
	}
}

