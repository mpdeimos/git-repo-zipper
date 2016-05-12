# git-repo-zipper [![Build Status](https://travis-ci.org/mpdeimos/git-repo-zipper.svg?branch=master)](https://travis-ci.org/mpdeimos/git-repo-zipper)

Merges multiple Git repositories in a zipper-like style.

## How it works

Given you have multiple repositories with roughly the same branch structure, `git-repo-zipper` will merge the repositories by cherry-picking the commits on each other (determined by commit date) to produce a single repository. Think of it like a zipper for git repositories:

Repository A:
```
       A4----A5----A7 topic
      /             \
A1---A2---A3---A6---A8 master
```

Repository B:
```
       B4----B5----B7 topic
      /        \    \
B1---B2---B3---B6---B8 master
```

Merged Repository:
```
       B4-----A4----A5-----B5-------B7---A7 topic
      /                      \        \    \
B1---A1---A2---B2---B3---A3---B6---A6---B8---A8 master
```

Zipping also works with more than two input repositories.

## Usage

`git-repo-zipper` is written in C# and should run on Linux, Max and Window using either Mono or the original Microsoft .NET Framework.

### Synopsis

```
  -o, --output    Required. The output repository to write to.

  -i, --input     The input repositories to read from.

  -f, --force     (Default: False) Forces overriding the output repository.

  -k, --keep      (Default: False) Keeps the remotes to the input repositories.

  --help          Display this help screen.
```

If you get an error like `System.TypeInitializationException: The type initializer for 'LibGit2Sharp.Core.NativeMethods' threw an exception. ---> System.DllNotFoundException: NativeBinaries/linux/amd64/libgit2-785d8c4.so` (especially on Linux), ensure to preload an older version of `libcurl`:

       LD_PRELOAD=libcurl.so.3

This is acurrent limitation of `libgit` and has to be fixed upstream.

## Prerequisites

* All repositories have a similar branch structure, essentially the same branch names in order to get zipped
* Branches should have been created at the same time for all repositories

## Limitations

The zipper works best repositories exported directly from Subversion (this was my use-case), this implies these limitations:

* The input repositories must start from one root commit
* Anonymous branches are not migrated (yet)
* Tags will not be converted (yet)

The zipper might still work in these cases, but please validate the result. However, this holds in general. If you encounter a limitation or bug, feel free to open an issue or pull request.

## Build & Test

The project is built using the provided solution in the repository root.
Use either an IDE (MonoDevelop, XamarinStudio, VisualStudio) or XBuild/MSBuild on the terminal and ensure nuget packages are restored beforehand.
Tests are written with NUnit2 and should run from the terminal as well as from the IDE.
See `.travis.yml` for further information.
