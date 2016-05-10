using System;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace Mpdeimos.GitRepoMerge.Model
{
	public class Config
	{
		[Option('o', "output", Required = true, HelpText = "The output repository to write to.")]
		public string Target { get; set; }

		[OptionArray('i', "input", HelpText = "The input repositories to read from.")]
		public string[] Sources { get; set; }

		[Option('f', "force", HelpText = "Forces overriding the output repository.", DefaultValue = false)]
		public bool Force { get; set; }

		[Option('k', "keep", HelpText = "Keeps the remotes to the input repositories.", DefaultValue = false)]
		public bool Clean { get; set; }

		/// <summary>
		/// The usage help provided from annotated options.
		/// </summary>
		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		/// <summary>
		/// Constructor for testing purposes.
		/// </summary>
		public Config()
		{
		}

		/// <summary>
		/// Constructor for parsing the commandline.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public Config(params string[] args)
		{
			Parser.Default.ParseArgumentsStrict(args, this);
		}
	}
}

