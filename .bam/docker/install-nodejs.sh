#!/bin/bash

NODEVERSION=$(<nodeversion)
DISTRO=linux-x64

curl https://nodejs.org/dist/v$NODEVERSION/node-v$NODEVERSION-$DISTRO.tar.xz --output ./node-v$NODEVERSION-$DISTRO.tar.xz
mkdir -p /usr/local/lib/nodejs
tar -xvf ./node-v$NODEVERSION-$DISTRO.tar.xz -C /usr/local/lib/nodejs
ln -s /usr/local/lib/nodejs/node-v$NODEVERSION-$DISTRO/bin/node /usr/local/bin/node
ln -s /usr/local/lib/nodejs/node-v$NODEVERSION-$DISTRO/bin/npm /usr/local/bin/npm
ln -s /usr/local/lib/nodejs/node-v$NODEVERSION-$DISTRO/bin/npx /usr/local/bin/npx