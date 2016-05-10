using System;
using System.Linq;
using LibGit2Sharp;
using Mpdeimos.GitRepoMerge.Util;
using Mpdeimos.GitRepoMerge.Model;
using Mpdeimos.GitRepoMerge.Service;

namespace Mpdeimos.GitRepoMerge
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			try
			{
				var config = ParseCommandline(args);
				ZipRepositories(config);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);	
			}
		}

		static Config ParseCommandline(string[] args)
		{
			if (args.Length < 2)
			{
				throw new MergeException("Need to specify target and at least one source repository.");
			}
			var config = new Config(args[0], args.Skip(1).ToArray());
			return config;
		}

		static void ZipRepositories(Config config)
		{
			var merger = new RepoMerger(config.Sources, config.Target);
			merger.Merge();
		}
	}
}
