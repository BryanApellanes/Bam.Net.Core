#!/bin/bash

source ../common/init.sh

build_bake

$BAKE /clean:${OUTPUTRECIPES}${RUNTIME}-bamfx-lib.json 
$BAKE /clean:${OUTPUTRECIPES}${RUNTIME}-bamtoolkit.json
