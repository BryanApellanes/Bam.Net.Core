#!/bin/bash

source ../common/init.sh

./discover-lib-recipe.sh
./discover-tools-recipe.sh

./write-version-code.sh