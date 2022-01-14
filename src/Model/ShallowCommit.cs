using System;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace Mpdeimos.GitRepoZipper.Model
{
	public class ShallowCommit
	{
		private static Dictionary<string, ShallowCommit> Cache = new Dictionary<string, ShallowCommit>();

		public Signature Author { get; private set; }

		public string Sha
		{
			get;
			private set;
		}

		public IEnumerable<ShallowCommit> Parents
		{
			get;
			set;
		}

		private ShallowCommit(Commit commit)
		{
			this.Sha = commit.Sha;
			this.Author = commit.Author;
			this.Parents = commit.Parents.Select(ShallowCommit.FromCommit);
		}

		public override bool Equals(object obj)
		{
			ShallowCommit c = obj as ShallowCommit;
			if (c == null)
			{
				return false;
			}

			return this.Sha.Equals(c.Sha);
		}

		public override int GetHashCode()
		{
			return Sha.GetHashCode();
		}

		public static ShallowCommit FromCommit(Commit commit)
		{
            if (commit == null)
            {
				throw new ArgumentNullException(nameof(commit));
            }

			if (!Cache.ContainsKey(commit.Sha))
			{
				Cache[commit.Sha] = new ShallowCommit(commit);
			}

			return Cache[commit.Sha];
		}
	}
}

