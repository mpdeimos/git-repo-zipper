using System;
using NUnit.Framework;
using LibGit2Sharp;
using Mpdeimos.GitRepoZipper.Util;
using Mpdeimos.GitRepoZipper.Model;

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
	}
}