#!/bin/bash

for r in */
do
	if [[ -d "$r/dot_git" && -L "$r/.git" ]]
	then
		echo "unlink $r"
		rm "$r/.git"
	fi
done
