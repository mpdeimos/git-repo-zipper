using System;
using System.Linq;
using LibGit2Sharp;

namespace Mpdeimos.GitRepoMerge
{
	class MainClass
	{
		const string RepoPath = "/home/mpdeimos/workspaces/dotnet/git-repo-merge/Mpdeimos.GitRepoMerge/test-data/gitD/dot_git";

		public static void Main(string[] args)
		{
			var repo = new Repository(RepoPath);
			Console.WriteLine(string.Join(",", repo.Network.Remotes));
		}
	}
}
