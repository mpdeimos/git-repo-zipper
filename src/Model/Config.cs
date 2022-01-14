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

		[Option('i', "input", HelpText = "The input repositories to read from.")]
		public IEnumerable<string> Sources { get; set; }

		[Option('b', "include", HelpText = "The branches to include (by friendly name, regex).")]
		public IEnumerable<string> Include { get; set; }

		[Option('x', "exclude", HelpText = "The branches to exclude (by friendly name, regex).")]
		public IEnumerable<string> Exclude { get; set; }

		[Option("remote", HelpText = "Include remote branches.", Default = false)]
		public bool Remote { get; set; }

		[Option('g', "graft-merges", HelpText = "Graft merges instead of rewriting the history.", Default = false)]
		public bool GraftMerges { get; set; }

		[Option('f', "force", HelpText = "Forces overriding the output repository.", Default = false)]
		public bool Force { get; set; }

		[Option('s', "silent", HelpText = "Prevents printing to proggress information to the console.", Default = false)]
		public bool Silent { get; set; }

		[Option('r', "retry", HelpText = "Allows resuming an operation if an error occurrs by manually modifying the workspace.", Default = false)]
		public bool Retry { get; set; }

		[Option('n', "dry-run", HelpText = "Dry run of zipping the repositories (does not write anything to the specified output).", Default = false)]
		public bool DryRun { get; set; }

		[Option('k', "keep", HelpText = "Keeps the remotes to the input repositories.", Default = false)]
		public bool Keep { get; set; }

		[Option('t', "tags", HelpText = "Keep tags, they might not be correctly rewritten.", Default = false)]
		public bool Tags { get; set; }

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