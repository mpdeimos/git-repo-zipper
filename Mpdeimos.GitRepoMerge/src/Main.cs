using System;
using System.Linq;
using LibGit2Sharp;
using Mpdeimos.GitRepoMerge.Util;
using Mpdeimos.GitRepoMerge.Model;

namespace Mpdeimos.GitRepoMerge
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			try
			{
				var config = new Config(args);
				ZipRepositories(config);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);	
			}
		}

		static void ZipRepositories(Config config)
		{
			var merger = new RepoZipper(config);
			merger.Zip();
		}
	}
}
