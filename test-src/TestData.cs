using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Mpdeimos.GitRepoZipper
{
	/// <summary>
	/// Manages test data files.
	/// </summary>
	public class TestData
	{
		public const string GitTwoSimpleBranchesA = "twoSimpleBranchesA";
		public const string GitTwoSimpleBranchesB = "twoSimpleBranchesB";
		public const string GitTwoSimpleBranchesC = "twoSimpleBranchesC";
		public const string GitThreeBranchesA = "threeBranchesA";
		public const string GitThreeBranchesB = "threeBranchesB";
		public const string GitUnnamedBranchA = "unnamedBranchA";
		public const string GitOrphanedBranch = "orphanedBranch";
		public const string GitTaggedBranchA = "taggedBranchA";

		private DirectoryInfo directory;

		/// <summary>
		/// Returns the directory containing test data.
		/// </summary>
		public DirectoryInfo Directory
		{
			get
			{
				if (this.directory == null)
				{
					var searchDir = new DirectoryInfo(Environment.CurrentDirectory);
					while (searchDir != null)
					{
						var dir = searchDir.GetDirectories("test-data").FirstOrDefault(d => d.Name == "test-data");
						if (dir != null)
						{
							this.directory = dir;
						}
						searchDir = searchDir.Parent;
					}
				}

				Assert.NotNull(this.directory, "No test-data directory found.");
				return this.directory;
			}
		}

		/// <summary>
		/// Gets the path to a test directory of file.
		/// </summary>
		public string GetPath(string name)
		{
			string path = Path.Combine(Directory.FullName, name);
			Assert.IsTrue(System.IO.Directory.Exists(path), $"No valid directory: {path}");
			return path;
		}

		/// <summary>
		/// Gets the path to a named test directory. Ensures that the directory exists and is empty.
		/// </summary>
		public string GetCleanTempDir(string name)
		{
			string path = Path.Combine(Directory.FullName, "test-tmp", name);
			if (System.IO.Directory.Exists(path))
			{
				System.IO.Directory.Delete(path, true);
			}
			System.IO.Directory.CreateDirectory(path);
			return path;
		}

		/// <summary>
		/// Gets the path to a test directory named according to the test case. Ensures that the directory exists and is empty.
		/// </summary>
		public string GetCleanTempDir()
		{
			return GetCleanTempDir(TestContext.CurrentContext.Test.FullName);
		}
	}
}

