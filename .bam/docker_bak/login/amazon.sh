#!/bin/bash

eval $(aws ecr get-login --no-include-email | sed 's|https://||')