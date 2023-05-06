#!/bin/bash

PROJECT=$1

if { [[ $PROJECT == "" ]] || [[ $PROJECT != "client" ]]; } then
    if [[ $PROJECT != "server" ]]; then
        echo "Usage: build_project.bat <client | server> [Configuration] [Architecture] [.NET Version]"
        exit 1
    fi
fi

BUILD_ARCH=x64
DOTNET_VERSION=net7.0
CONFIGURATION=Debug

# Sets the configuration
if [[ $2 != "" ]]; then
    CONFIGURATION=$2
fi

# Sets the architecture
if [[ $3 != "" ]]; then
    BUILD_ARCH=$3
fi

# Sets the dotnet version
if [[ $4 != "" ]]; then
    DOTNET_VERSION=$4
fi

# Builds the Code Preprocessor
dotnet build ../ReNote.CodeProcessor/ReNote.CodeProcessor.csproj --no-self-contained --runtime linux-$BUILD_ARCH

# Creates the processor directory
mkdir processor

# Moves output of the Code Preprocessor to the Tools\processor directory
./move_output.sh ../ReNote.CodeProcessor/bin/$CONFIGURATION/$DOTNET_VERSION/linux-$BUILD_ARCH ./processor ReNote.CodeProcessor codeproc

# Runs the Code Preprocessor
./codeprocessor.sh $PROJECT

# Builds the Project
if [[ $PROJECT == "client" ]]; then
    dotnet build ../Client/Client.csproj -c $CONFIGURATION --no-self-contained --runtime linux-$BUILD_ARCH
else
    dotnet build ../Server/Server.csproj -c $CONFIGURATION --no-self-contained --runtime linux-$BUILD_ARCH
fi