#!/bin/bash

for r in */
do
	if [[ -d "$r/dot_git" && !( -L "$r/.git" )]]
	then
		echo "link $r"
		ln -s "$r/dot_git" "$r/.git"
	fi
done
