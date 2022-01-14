using System;
using System.Linq;
using Mpdeimos.GitRepoZipper.Util;
using Mpdeimos.GitRepoZipper.Model;
using CommandLine;
using CommandLine.Text;

namespace Mpdeimos.GitRepoZipper
{
	static class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				var parser = new Parser();
				var parserResult = parser.ParseArguments<Config>(args);
				parserResult.WithParsed<Config>(options => ZipRepositories(options));
				parserResult.WithNotParsed(errs =>
				{
					var helpText = HelpText.AutoBuild(parserResult, h =>
					{
						// Configure HelpText here  or create your own and return it 
						h.AdditionalNewLineAfterOption = false;
						return HelpText.DefaultParsingErrorsHandler(parserResult, h);
					}, e =>
					{
						return e;
					});
					Console.Error.Write(helpText);
				}
					);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);	
			}
		}

		static void ZipRepositories(Config config)
		{
			var zipper = new RepoZipper(config);
			zipper.Zip();
		}
	}
}
