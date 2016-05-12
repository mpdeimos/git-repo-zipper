using System;
using System.Linq;
using LibGit2Sharp;
using Mpdeimos.GitRepoZipper.Util;
using Mpdeimos.GitRepoZipper.Model;

namespace Mpdeimos.GitRepoZipper
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			try
			{
				var config = Config.FromCommandline(args);
				ZipRepositories(config);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);	
			}
		}

		static void ZipRepositories(Config config)
		{
			var zipper = new RepoZipper(config);
			zipper.Zip();
		}
	}
}
