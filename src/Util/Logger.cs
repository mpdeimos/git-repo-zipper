using System;

namespace Mpdeimos.GitRepoZipper.Util
{
	public class Logger
	{
		/// <summary>
		/// Whether the logger just logs errors.
		/// </summary>
		public bool Silent { get; set; }

		/// <inheritdoc/>
		public void Log(string message, bool replace = false)
		{
			if (!this.Silent)
			{
				if (replace)
				{
					Console.SetCursorPosition(0, Console.CursorTop - 1);
				}

				Console.WriteLine(message);
			}
		}
	}
}

