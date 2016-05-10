using System;

namespace Mpdeimos.GitRepoZipper.Util
{
	/// <summary>
	/// Zipper exception that indicates a problem during merging (e.g. invalid states).
	/// </summary>
	public class ZipperException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public ZipperException(String message) : base(message)
		{
			// Chained constructor.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public ZipperException(String message, Exception exception) : base(message, exception)
		{
			// Chained constructor.
		}
	}
}

