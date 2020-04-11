#!/bin/bash

source ../common/init.sh

ensure_bake
set_git_commit

echo ${GITCOMMIT} > ${OUTPUTBIN}/${GITCOMMIT}
${BAKE} /zip:${RUNTIME}-bamtoolkit-${GITCOMMIT}.zip /zipRecipe:./recipes/${RUNTIME}-bamtoolkit.json /output:${OUTPUTBIN}

# add a symlink for artifact upload
if [[ -f ${OUTPUTBIN}/../${RUNTIME}-bamtoolkit.zip ]]; then
    rm ${OUTPUTBIN}/../${RUNTIME}-bamtoolkit.zip
fi
ln -s ${OUTPUTBIN}/../${RUNTIME}-bamtoolkit-${GITCOMMIT}.zip ${OUTPUTBIN}/../${RUNTIME}-bamtoolkit.zip
ln -s ${OUTPUTBIN}/../${RUNTIME}-bamtoolkit-${GITCOMMIT}.zip ${OUTPUTBIN}/../latest.zip