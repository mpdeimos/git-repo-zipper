using System;
using System.Collections.Generic;

namespace Mpdeimos.GitRepoMerge
{
	public class MergeScenario
	{
		public string[] Sources { get; private set; }

		public Dictionary<string, string[]> Branches { get; } = new Dictionary<string, string[]>();

		public string[] this[string name] { set { this.Branches[name] = value; } }

		public MergeScenario(params string[] sources)
		{
			this.Sources = sources;
		}
	}
}

