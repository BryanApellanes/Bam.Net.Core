#!/bin/bash

source ../common/init.sh

ensure_bake

$BAKE /clean:${OUTPUTRECIPES}${RUNTIME}-bamtoolkit.json
