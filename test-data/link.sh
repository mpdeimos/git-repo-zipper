#!/bin/bash

for r in */
do
	if [[ -d "$r/dot_git" && !( -L "$r/.git" )]]
	then
		pushd $r
		echo "link $r"
		ln -s "dot_git" ".git"
		popd
	fi
done
