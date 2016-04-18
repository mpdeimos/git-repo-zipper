using System.IO;
using LibGit2Sharp;
using System.Collections.Generic;
using NUnit.Framework;

namespace Mpdeimos.GitRepoMerge
{
	public class RepoTestBase
	{
		/// <summary>
		/// The test data manager.
		/// </summary>
		protected static TestData TestData { get; } = new TestData();

		protected const string GitTwoSimpleBranchesA = "twoSimpleBranchesA";
		protected const string GitTwoSimpleBranchesB = "twoSimpleBranchesB";
		protected const string GitTwoSimpleBranchesC = "twoSimpleBranchesC";
		protected const string GitUnnamedBranchA = "unnamedBranchA";

		public MergeScenario[] MergeScenarios
		{
			get
			{ 
				return new [] { new MergeScenario(GitTwoSimpleBranchesA) { 
						["master" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9d46d63c3345bab7f9fafaa5fdf13f3c04bbe2b8"
						},
						["1" ] = new[] {
							"34fbde4c98cc29773cd3483aa44c26d5cca816f5",
							"9f9d6e3068f26f00b076b10380764a3519490486"
						}
					}
				};
			}
		}

		/// <summary>
		/// Gets the path to a test repository.
		/// </summary>
		protected static string GetTestRepoPath(string name)
		{
			return TestData.GetPath(Path.Combine(name, "dot_git"));
		}

		/// <summary>
		/// Gets the test repository.
		/// </summary>
		protected static Repository GetTestRepo(string name)
		{
			return new Repository(GetTestRepoPath(name));
		}
	}
}