#!/bin/bash

function git_setup(){
    USERNAME=$1
    EMAIL=$2
    git config --global user.name ${USERNAME}
    git config --global user.email ${EMAIL}
}

function git_checkout(){
    BRANCHNAME=$1
    COMMAND="git checkout ${BRANCHNAME}"
    printf "executing =>${GREEN}${COMMAND}${NOCOLOR}"
    ${COMMAND}
}

function git_merge(){
    OTHERBRANCH=$1
    git_current_branch
    
}

function git_add(){
    PATTERN=$1
    COMMAND="git add ${PATTERN}"
    echo "executing =>${GREEN}${COMMAND}${NOCOLOR}"
    ${COMMAND}
}

function git_tag(){
    TAG=$1
    MESSAGE=$2
    COMMAND="git tag -a ${TAG} -m \"${MESSAGE}\""
    echo "${GREEN}Tagging current branch: ${TAG}${NOCOLOR}"
    git tag -a ${TAG} -m "${MESSAGE}"
}

function git_commit(){
    MESSAGE=$1
    AUTHOR=$2
    OPTIONS=$3
    COMMAND="git commit -m '${MESSAGE}' --author='${AUTHOR}' ${OPTIONS}"
    printf "executing =>${GREEN}${COMMAND}${NOCOLOR}"
    ${COMMAND}
}

function git_current_branch(){
    if [[ -z ${GITCURRENTBRANCH} ]]; then
        GITCURRENTBRANCH=`git rev-parse --abbrev-ref HEAD`
    fi
}

function git_push_help(){
    echo 'git_push {$1:ACTOR} {$2:GITHUBTOKEN} {$3:OWNERSLASHREPONAME} {$4:BRANCHNAME} {$5:force}'
}

function git_push(){
    ACTOR=$1
    GITHUBTOKEN=$2
    OWNERSLASHREPONAME=$3
    BRANCHNAME=$4
    FORCE=$5
    if [[ -z ${ACTOR} || -z ${GITHUBTOKEN} || -z ${OWNERSLASHREPONAME} ]]; then
        printf "${RED}FAILED TO PUSH BRANCH${NOCOLOR}: call git_push function with the following arguments; any 5th argument will be converted to '--force'\r\n"
        git_push_help
        return
    fi
    if [[ !(-z ${FORCE}) ]]; then # if a 5th argument is specified then set force option
        FORCE="--force"
    fi
    REMOTEREPO="https://${ACTOR}:${GITHUBTOKEN}@github.com/${OWNERSLASHREPONAME}.git"
    COMMAND="git push ${REMOTEREPO} HEAD:${BRANCHNAME} --follow-tags ${FORCE} --tags"
    ${COMMAND}
}
