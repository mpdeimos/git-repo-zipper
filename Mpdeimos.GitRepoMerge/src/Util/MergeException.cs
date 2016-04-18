using System;

namespace Mpdeimos.GitRepoMerge.Util
{
	/// <summary>
	/// Merge exception that indicates a problem during merging (e.g. invalid states).
	/// </summary>
	public class MergeException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public MergeException(String message) : base(message)
		{
			// Chained constructor.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public MergeException(String message, Exception exception) : base(message, exception)
		{
			// Chained constructor.
		}
	}
}

