using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Mpdeimos.GitRepoMerge
{
	/// <summary>
	/// Manages test data files.
	/// </summary>
	public class TestData
	{
		public const string GitTwoSimpleBranchesA = "twoSimpleBranchesA";
		public const string GitTwoSimpleBranchesB = "twoSimpleBranchesB";
		public const string GitTwoSimpleBranchesC = "twoSimpleBranchesC";
		public const string GitUnnamedBranchA = "unnamedBranchA";
		public const string GitOrphanedBranch = "orphanedBranch";

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
	}
}

