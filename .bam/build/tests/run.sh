#!/bin/bash

source ../common/init.sh

ensure_bamtest

${BAMTEST} /Recipe:./recipes/${RUNTIME}-bamtoolkit-tests.json
