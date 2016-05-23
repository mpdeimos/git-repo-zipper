using System;

namespace Mpdeimos.GitRepoZipper.Util
{
	public class Logger
	{
		/// <summary>
		/// Whether the logger just logs errors.
		/// </summary>
		public bool Silent { get; set; }

		private bool replacing = false;

		/// <inheritdoc/>
		public void Log(string message, bool replace = false)
		{
			if (!this.Silent)
			{
				if (replace && this.replacing)
				{
					Console.SetCursorPosition(0, Math.Max(0, Console.CursorTop - 1));
				}

				this.replacing = replace;
				Console.WriteLine(message);
			}
		}
	}
}

