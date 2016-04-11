using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Mpdeimos.GitRepoMerge
{
	public class RepoTestBase
	{
		protected readonly string GitA = GetTestData("gitA/dot_git");
		protected readonly string GitB = GetTestData("gitB/dot_git");
		protected readonly string GitC = GetTestData("gitC/dot_git");
		protected readonly string GitD = GetTestData("gitD/dot_git");

		protected static DirectoryInfo TestData
		{
			get
			{
				var testData = new DirectoryInfo(Environment.CurrentDirectory);

				while (testData != null)
				{
					var dir = testData.GetDirectories("test-data").FirstOrDefault(d => d.Name == "test-data");
					if (dir != null)
					{
						return dir;
					}

					testData = testData.Parent;
				}

				Assert.Fail("No test-data directory found.");
				return testData;
			}
		}

		protected static string GetTestData(string name)
		{
			string path = Path.Combine(TestData.FullName, name);
			Assert.IsTrue(Directory.Exists(path), $"No valid directory: {path}");
			return path;
		}
	}
}

