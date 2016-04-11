using System;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace Mpdeimos.GitRepoMerge.Model
{
	public class Config
	{
		public string Target { get; private set; }

		public IEnumerable<Repository> Sources { get; private set; }

		public Config(string target, params string[] sources)
		{
			this.Target = target;
			this.Sources = sources?.Select(source => new Repository(source));
		}
	}
}

