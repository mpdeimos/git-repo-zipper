using System;
using System.Collections.Generic;
using LibGit2Sharp;
using Mpdeimos.GitRepoZipper.Model;
using Mpdeimos.GitRepoZipper.Util;
using System.Linq;
using System.IO;

namespace Mpdeimos.GitRepoZipper.Model
{
	class DryRepository : IRepository
	{
		public Branch Checkout(string committishOrBranchSpec, CheckoutOptions options)
		{
			throw new NotImplementedException();
		}

		public Branch Checkout(Commit commit, CheckoutOptions options)
		{
			throw new NotImplementedException();
		}

		public void CheckoutPaths(string committishOrBranchSpec, IEnumerable<string> paths, CheckoutOptions checkoutOptions)
		{
			throw new NotImplementedException();
		}

		public GitObject Lookup(ObjectId id)
		{
			throw new NotImplementedException();
		}

		public GitObject Lookup(string objectish)
		{
			throw new NotImplementedException();
		}

		public GitObject Lookup(ObjectId id, ObjectType type)
		{
			throw new NotImplementedException();
		}

		public GitObject Lookup(string objectish, ObjectType type)
		{
			throw new NotImplementedException();
		}

		public Commit Commit(string message, Signature author, Signature committer, CommitOptions options)
		{
			throw new NotImplementedException();
		}

		public void Reset(ResetMode resetMode, Commit commit)
		{
			throw new NotImplementedException();
		}

		public void Reset(ResetMode resetMode, Commit commit, CheckoutOptions options)
		{
			throw new NotImplementedException();
		}

		public void Reset(Commit commit, IEnumerable<string> paths, ExplicitPathsOptions explicitPathsOptions)
		{
			throw new NotImplementedException();
		}

		public void RemoveUntrackedFiles()
		{
			throw new NotImplementedException();
		}

		public RevertResult Revert(Commit commit, Signature reverter, RevertOptions options)
		{
			throw new NotImplementedException();
		}

		public MergeResult Merge(Commit commit, Signature merger, MergeOptions options)
		{
			throw new NotImplementedException();
		}

		public MergeResult Merge(Branch branch, Signature merger, MergeOptions options)
		{
			throw new NotImplementedException();
		}

		public MergeResult Merge(string committish, Signature merger, MergeOptions options)
		{
			throw new NotImplementedException();
		}

		public MergeResult MergeFetchedRefs(Signature merger, MergeOptions options)
		{
			throw new NotImplementedException();
		}

		public CherryPickResult CherryPick(Commit commit, Signature committer, CherryPickOptions options)
		{
			throw new NotImplementedException();
		}

		public BlameHunkCollection Blame(string path, BlameOptions options)
		{
			throw new NotImplementedException();
		}

		public void Stage(string path, StageOptions stageOptions)
		{
			throw new NotImplementedException();
		}

		public void Stage(IEnumerable<string> paths, StageOptions stageOptions)
		{
			throw new NotImplementedException();
		}

		public void Unstage(string path, ExplicitPathsOptions explicitPathsOptions)
		{
			throw new NotImplementedException();
		}

		public void Unstage(IEnumerable<string> paths, ExplicitPathsOptions explicitPathsOptions)
		{
			throw new NotImplementedException();
		}

		public void Move(string sourcePath, string destinationPath)
		{
			throw new NotImplementedException();
		}

		public void Move(IEnumerable<string> sourcePaths, IEnumerable<string> destinationPaths)
		{
			throw new NotImplementedException();
		}

		public void Remove(string path, bool removeFromWorkingDirectory, ExplicitPathsOptions explicitPathsOptions)
		{
			throw new NotImplementedException();
		}

		public void Remove(IEnumerable<string> paths, bool removeFromWorkingDirectory, ExplicitPathsOptions explicitPathsOptions)
		{
			throw new NotImplementedException();
		}

		public FileStatus RetrieveStatus(string filePath)
		{
			throw new NotImplementedException();
		}

		public RepositoryStatus RetrieveStatus(StatusOptions options)
		{
			throw new NotImplementedException();
		}

		public string Describe(Commit commit, DescribeOptions options)
		{
			throw new NotImplementedException();
		}

		public Branch Head
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Configuration Config
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Index Index
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ReferenceCollection Refs
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IQueryableCommitLog Commits
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public TagCollection Tags
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public RepositoryInformation Info
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Diff Diff
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ObjectDatabase ObjectDatabase
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public NoteCollection Notes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public SubmoduleCollection Submodules
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Rebase Rebase
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Ignore Ignore
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Network Network
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public StashCollection Stashes
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void Dispose()
		{
			// do nothing
		}

		public BranchCollection Branches { get; } = new DryBranchCollection();

		public Branch Checkout(Branch branch, CheckoutOptions options)
		{
			return branch;
		}

		public class DryBranchCollection : BranchCollection
		{
			private Dictionary<string, Branch> branches = new Dictionary<string, Branch>();

			public override Branch this[string index]
			{
				get
				{
					if (branches.ContainsKey(index))
					{
						return branches[index];
					}
					return null;
				}
			}

			public override Branch Add(string name, Commit commit)
			{
				var branch = new DryBranch();
				this.branches.Add(name, branch);
				return branch;
			}
		}


		public class DryBranch : Branch
		{
		}
	}

}