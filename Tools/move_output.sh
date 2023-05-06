#!/bin/bash

CONTENT_DIR=$1
OUTPUT_DIR=$2
OLD_EXECUTABLE_NAME=$3
NEW_EXECUTABLE_NAME=$4

if { [[ $CONTENT_DIR == "" ]] || [[ $OUTPUT_DIR == "" ]] || [[ $OLD_EXECUTABLE_NAME == "" ]] || [[ $NEW_EXECUTABLE_NAME == "" ]]; } then
    echo "Usage: move_output.bat <CONTENT_DIR> <OUTPUT_DIR> <OLD_EXECUTABLE_NAME> <NEW_EXECUTABLE_NAME>"
    exit
fi

echo Moving output from \'$CONTENT_DIR\' to \'$OUTPUT_DIR\'
cp -rv $CONTENT_DIR/* $OUTPUT_DIR
rm -rvd $CONTENT_DIR/*

echo Renaming main executable from \'$OLD_EXECUTABLE_NAME\' to \'$NEW_EXECUTABLE_NAME\'
mv -v $OUTPUT_DIR/$OLD_EXECUTABLE_NAME $OUTPUT_DIR/$NEW_EXECUTABLE_NAME