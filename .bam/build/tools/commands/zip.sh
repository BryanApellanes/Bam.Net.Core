#!/bin/bash

source ../common/init.sh

ensure_bake
set_git_commit

echo ${GITCOMMIT} > ${OUTPUTBIN}/${GITCOMMIT}
echo ${RUNTIME} > ${OUTPUTBIN}/${RUNTIME}
${BAKE} /zip:${RUNTIME}-bamtoolkit-${GITCOMMIT}.zip /zipRecipe:./recipes/${RUNTIME}-bamtoolkit.json /output:${OUTPUTBIN}

# copy for artifact upload
if [[ -f ${OUTPUTBIN}/../bamtoolkit.zip ]]; then
    rm ${OUTPUTBIN}/../bamtoolkit.zip
fi
mv ${OUTPUTBIN}/../${RUNTIME}-bamtoolkit-${GITCOMMIT}.zip ${OUTPUTBIN}/../bamtoolkit.zip
ln -s ${OUTPUTBIN}/../bamtoolkit.zip ${OUTPUTBIN}/../${RUNTIME}-bamtoolkit.zip