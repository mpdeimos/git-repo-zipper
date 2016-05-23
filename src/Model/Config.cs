using System;
using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using System.Text.RegularExpressions;

namespace Mpdeimos.GitRepoZipper.Model
{
	public class Config
	{
		[Option('o', "output", Required = true, HelpText = "The output repository to write to.")]
		public string Target { get; set; }

		[OptionArray('i', "input", HelpText = "The input repositories to read from.")]
		public string[] Sources { get; set; }

		[OptionArray('b', "include", HelpText = "The branches to include (by friendly name, regex).")]
		public string[] Include { get; set; }

		[OptionArray('x', "exclude", HelpText = "The branches to exclude (by friendly name, regex).")]
		public string[] Exclude { get; set; }

		[Option('r', "remote", HelpText = "Include remote branches.", DefaultValue = false)]
		public bool Remote { get; set; }

		[Option('f', "force", HelpText = "Forces overriding the output repository.", DefaultValue = false)]
		public bool Force { get; set; }

		[Option('k', "keep", HelpText = "Keeps the remotes to the input repositories.", DefaultValue = false)]
		public bool Clean { get; set; }

		[Option('s', "silent", HelpText = "Prevents printing to proggress information to the console.", DefaultValue = false)]
		public bool Silent { get; set; }

		[Option('r', "retry", HelpText = "Allows resuming an operation if an error occurrs by manually modifying the workspace.", DefaultValue = false)]
		public bool Retry { get; set; }

		/// <summary>
		/// The usage help provided from annotated options.
		/// </summary>
		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		/// <summary>
		/// Constructor for parsing the commandline.
		/// </summary>
		public static Config FromCommandline(params string[] args)
		{
			var config = new Config();
			Parser.Default.ParseArgumentsStrict(args, config);
			return config;			
		}

		/// <summary>
		/// Returns whether a branch is included by this configuration.
		/// </summary>
		public bool IsBranchIncluded(Branch branch)
		{
			if (!this.Remote && branch.IsRemote)
			{
				return false;
			}


			if (this.Include != null && !this.Include.Any(include => Regex.IsMatch(branch.FriendlyName, include)))
			{
				return false;				
			}

			if (this.Exclude != null && this.Exclude.Any(exclude => Regex.IsMatch(branch.FriendlyName, exclude)))
			{
				return false;
			}
				
			return true;
		}
	}
}