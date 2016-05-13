using LibGit2Sharp;

namespace Mpdeimos.GitRepoZipper.Scenario
{
	/// <summary>
	/// Describes a commit with parents.
	/// </summary>
	public class SimpleCommit
	{
		/// <summary>
		/// The commit sha.
		/// </summary>
		public string Sha { get; }

		/// <summary>
		/// The commit parents.
		/// </summary>
		public string[] Parents { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		public SimpleCommit(string sha, params string[] parents)
		{
			this.Sha = sha;
			this.Parents = parents;
		}

		//		public override bool Equals(object other)
		//		{
		//			if (this == other)
		//			{
		//				return true;
		//			}
		//
		//			var commit = other as SimpleCommit;
		//			if (commit == null)
		//			{
		//				return false;
		//			}
		//
		//			return this.Sha == commit.Sha &&
		//		}

		public override string ToString()
		{
			return string.Format("[SimpleCommit: Sha={0}, Parents={1}]", Sha, Parents);
		}
	}
}

