#!/bin/bash

set -eu

_main() {
    _switch_to_repository

    if _git_is_dirty; then

        _setup_git

        _switch_to_branch

        _add_files

        _local_commit

        _push_to_github
    else
        echo "Working tree clean. Nothing to commit."
    fi
}


_switch_to_repository() {
    echo "BAMSRCROOT value: ${BAMSRCROOT}";
    cd ${BAMSRCROOT}
}

_git_is_dirty() {
    [[ -n "$(git status -s)" ]]
}

# Set up git user configuration
_setup_git ( ) {
    git config --global user.name "${CI_USERNAME}"
    git config --global user.email "${CI_EMAIL}}"
}

_switch_to_branch() {
    echo "BRANCH value: ${COMMIT_TO_BRANCH}";

    # Switch to branch from current Workflow run
    git checkout ${COMMIT_TO_BRANCH}
}

_add_files() {
    echo "INPUT_FILE_PATTERN: ${GIT_ADD_FILE_PATTERN}"
    git add "${GIT_ADD_FILE_PATTERN}"
}

_local_commit() {
    echo "INPUT_COMMIT_OPTIONS: ${GIT_COMMIT_OPTIONS}"
    git commit -m "${GIT_COMMIT_MESSAGE}" --author="${GIT_COMMIT_AUTHOR}" ${GIT_COMMIT_OPTIONS:+"$GIT_COMMIT_OPTIONS"}
}

_push_to_github() {
    if [ -z "${COMMIT_TO_BRANCH}" ]
    then
        git push origin
    else
        git push --set-upstream origin "HEAD:${COMMIT_TO_BRANCH}"
    fi
}

_main
