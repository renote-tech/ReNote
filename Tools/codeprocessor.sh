#!/bin/bash

PROJECT=$1

# We should only take this as a warning because if we exit with a non-zero code, Visual Studio will consider that the build failed.
if [[ ! -e "./processor/codeproc" ]]; then
   echo "WARNING: Couldn't find main executable 'codeproc'"
   exit
fi

if [[ $PROJECT == "client" ]]; then
   ./processor/codeproc --file ../Client/ClientInfo.cs --var Version     --type string --value '$VERSION$' --projects "../Client/Client.csproj"
   ./processor/codeproc --file ../Client/ClientInfo.cs --var BuildDate   --type string --value '$DATE$'
   ./processor/codeproc --file ../Client/ClientInfo.cs --var BuildNumber --type int    --value '$INCREMENT$'
elif [[ $PROJECT == "server" ]]; then
   ./processor/codeproc --file ../Server.Modules.Common/ServerInfo.cs --var Version     --type string --value '$VERSION$' --projects "../Server/Server.csproj" "../Server.Modules.Common/Server.Modules.Common.csproj" "../Server.Modules.ReNote/Server.Modules.ReNote.csproj" "../Server.Modules.Web/Server.Modules.Web.csproj"
   ./processor/codeproc --file ../Server.Modules.Common/ServerInfo.cs --var BuildDate   --type string --value '$DATE$'
   ./processor/codeproc --file ../Server.Modules.Common/ServerInfo.cs --var BuildNumber --type int    --value '$INCREMENT$'
else
    echo "Usage: ./codeprocessor.sh [client | server]"
fi