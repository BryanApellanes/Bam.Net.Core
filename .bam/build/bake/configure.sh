#!/bin/bash

printf "Configuring build...\r\n"

source ../common/init.sh

./discover-tools-recipe.sh
./discover-lib-recipe.sh

./write-version-code.sh