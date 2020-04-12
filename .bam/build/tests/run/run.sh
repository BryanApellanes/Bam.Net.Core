#!/bin/bash

# specify /Group:<testGroupName> to run tests for a specific test group

cd ..
source ../common/init.sh
ensure_bamtest
set_git_commit
cd ./run

if [[ -d ${GITCOMMIT} ]]; then
    rm -fr ${GITCOMMIT}
fi

mkdir ${GITCOMMIT}
cd ${GITCOMMIT}
printf "executing=>${BAMTEST} /Recipe:`pwd`/../../recipes/${RUNTIME}-bamtoolkit-tests.json $1 $2 $3"
${BAMTEST} /Recipe:`pwd`/../../recipes/${RUNTIME}-bamtoolkit-tests.json /tag:${GITCOMMIT} $1 $2 $3
cd ..
