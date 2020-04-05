#!/bin/bash

printf "Current directory is `pwd`\r\n"
printf "Available contexts are:\r\n"
printf "\r\n"
ls -d */ | sed 's#/##'
printf "\r\n"