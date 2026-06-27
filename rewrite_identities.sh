#!/bin/sh

# Set correct values
CORRECT_NAME="Thanh Vu"
CORRECT_EMAIL="thanhauco@gmail.com"

git filter-branch --env-filter '
    export GIT_COMMITTER_NAME="Thanh Vu"
    export GIT_COMMITTER_EMAIL="thanhauco@gmail.com"
    export GIT_AUTHOR_NAME="Thanh Vu" 
    export GIT_AUTHOR_EMAIL="thanhauco@gmail.com"
' --tag-name-filter cat -- --branches --tags
