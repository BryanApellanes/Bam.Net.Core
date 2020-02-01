#!/bin/bash

source ../common/init.sh

ensure_bake

${BAKE} /recipe:./recipes/${RUNTIME}-bamfx-lib.json
${BAKE} /nuget:./recipes/${RUNTIME}-bamfx-lib.json