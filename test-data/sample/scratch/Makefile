# Makefile for teamscale conversion using reposurgeon
#
# Steps to using this:
# 1. Make sure reposurgeon and repotool are on your $PATH.
# 2. For svn, set REMOTE_URL to point at the remote repository
#    you want to convert.
# 3. For cvs, set CVS_HOST to the repo hostname and CVS_MODULE to the module,
#    then uncomment the line that builds REMOTE_URL 
#    Note: for CVS hosts other than Sourceforge or Savannah you will need to 
#    include the path to the CVS modules directory after the hostname.
# 4. Set any required read options, such as --user-ignores or --nobranch,
#    by setting READ_OPTIONS.
# 5. Run 'make stubmap' to create a stub author map.
# 6. (Optional) set REPOSURGEON to point at a faster cython build of the tool.
# 7. Run 'make' to build a converted repository.
#
# The reason both first- and second-stage stream files are generated is that,
# especially with Subversion, making the first-stage stream file is often
# painfully slow. By splitting the process, we lower the overhead of
# experiments with the lift script.
#
# For a production-quality conversion you will need to edit the map
# file and the lift script.  During the process you can set EXTRAS to
# name extra metadata such as a comments mailbox.
#
# Afterwards, you can use the headcompare and tagscompare productions
# to check your work.
#

EXTRAS = 
REMOTE_URL = svn://svn.debian.org/teamscale
#REMOTE_URL = https://teamscale.googlecode.com/svn/
CVS_HOST = teamscale.cvs.sourceforge.net
#CVS_HOST = cvs.savannah.gnu.org
CVS_MODULE = teamscale
#REMOTE_URL = cvs://$(CVS_HOST)/teamscale\#$(CVS_MODULE)
READ_OPTIONS =
VERBOSITY = "verbose 1"
REPOSURGEON = reposurgeon

# Configuration ends here

.PHONY: local-clobber remote-clobber gitk gc compare clean dist stubmap
# Tell make not to auto-remove tag directories, because it only tries rm 
# and hence fails
.PRECIOUS: teamscale-%-checkout teamscale-%-git

default: teamscale-git

# Build the converted repo from the second-stage fast-import stream
teamscale-git: teamscale.fi
	rm -fr teamscale-git; $(REPOSURGEON) "read <teamscale.fi" "prefer git" 
"rebuild teamscale-git"

# Build the second-stage fast-import stream from the first-stage stream dump
teamscale.fi: teamscale.git teamscale.opts teamscale.lift teamscale.map $(EXTRAS)
	$(REPOSURGEON) $(VERBOSITY) "script teamscale.opts" "read $(READ_OPTIONS) 
<teamscale.git" "authors read <teamscale.map" "sourcetype git" "prefer git" 
"script teamscale.lift" "legacy write >teamscale.fo" "write >teamscale.fi"

# Build the first-stage stream dump from the local mirror
teamscale.git: teamscale-mirror
	(cd teamscale-mirror/ >/dev/null; repotool export) >teamscale.git

# Build a local mirror of the remote repository
teamscale-mirror:
	repotool mirror $(REMOTE_URL) teamscale-mirror

# Make a local checkout of the source mirror for inspection
teamscale-checkout: teamscale-mirror
	cd teamscale-mirror >/dev/null; repotool checkout ../teamscale-checkout

# Make a local checkout of the source mirror for inspection at a specific tag
teamscale-%-checkout: teamscale-mirror
	cd teamscale-mirror >/dev/null; repotool checkout ../teamscale-$*-checkout $*

# Force rebuild of first-stage stream from the local mirror on the next make
local-clobber: clean
	rm -fr teamscale.fi teamscale-git *~ .rs* teamscale-conversion.tar.gz 
teamscale-*-git

# Force full rebuild from the remote repo on the next make.
remote-clobber: local-clobber
	rm -fr teamscale.git teamscale-mirror teamscale-checkout teamscale-*-checkout

# Get the (empty) state of the author mapping from the first-stage stream
stubmap: teamscale.git
	$(REPOSURGEON) "read $(READ_OPTIONS) <teamscale.git" "authors write 
>teamscale.map"

# Compare the histories of the unconverted and converted repositories at head
# and all tags.
EXCLUDE = -x CVS -x .git -x .git
EXCLUDE += -x .gitignore -x .gitignore
headcompare: teamscale-mirror teamscale-git
	repotool compare $(EXCLUDE) teamscale-mirror teamscale-git
tagscompare: teamscale-mirror teamscale-git
	repotool compare-tags $(EXCLUDE) teamscale-mirror teamscale-git
branchescompare: teamscale-mirror teamscale-git
	repotool compare-branches $(EXCLUDE) teamscale-mirror teamscale-git
allcompare: teamscale-mirror teamscale-git
	repotool compare-all $(EXCLUDE) teamscale-mirror teamscale-git

# General cleanup and utility
clean:
	rm -fr *~ .rs* teamscale-conversion.tar.gz *.git *.fi *.fo

# Bundle up the conversion metadata for shipping
SOURCES = Makefile teamscale.lift teamscale.map $(EXTRAS)
teamscale-conversion.tar.gz: $(SOURCES)
	tar --dereference --transform 's:^:teamscale-conversion/:' -czvf 
teamscale-conversion.tar.gz $(SOURCES)

dist: teamscale-conversion.tar.gz

#
# The following productions are git-specific
#

# Browse the generated git repository
gitk: teamscale-git
	cd teamscale-git; gitk --all

# Run a garbage-collect on the generated git repository.  Import doesn't.
# This repack call is the active part of gc --aggressive.  This call is
# tuned for very large repositories.
gc: teamscale-git
	cd teamscale-git; time git -c pack.threads=1 repack -AdF --window=1250 --depth=250
