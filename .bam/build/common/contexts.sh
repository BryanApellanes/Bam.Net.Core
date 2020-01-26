#!/bin/bash

printf "Available contexts are:\r\n"
printf "\r\n"
ls -d */ | sed 's#/##'
printf "\r\n"