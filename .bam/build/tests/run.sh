#!/bin/bash

# specify /Group:<testGroupName> to run tests for a specific test group

source ../common/init.sh

ensure_bamtest

printf "executing=>${BAMTEST} /Recipe:`pwd`/recipes/${RUNTIME}-bamtoolkit-tests.json $1 $2 $3"
${BAMTEST} /Recipe:`pwd`/recipes/${RUNTIME}-bamtoolkit-tests.json $1 $2 $3
