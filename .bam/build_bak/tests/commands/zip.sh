#!/bin/bash

source ../common/init.sh

ensure_bake
set_git_commit


echo ${GITCOMMIT} > ${TESTBIN}/${GITCOMMIT}
echo ${RUNTIME} > ${TESTBIN}/${RUNTIME}
${BAKE} /zip:${RUNTIME}-bamtoolkit-${GITCOMMIT}-tests.zip /zipRecipe:${OUTPUTRECIPES}${RUNTIME}-bamtoolkit-tests.json /output:${TESTBIN}

if [[ -f ${TESTBIN}/../bamtoolkit-tests.zip ]]; then
    rm ${TESTBIN}/../bamtoolkit-tests.zip
fi
mv ${TESTBIN}/../${RUNTIME}-bamtoolkit-${GITCOMMIT}-tests.zip ${TESTBIN}/../bamtoolkit-tests.zip