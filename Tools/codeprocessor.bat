@echo off

set project=%1

cd ../Tools

REM We should only take this as a warning because if we exit with a non-zero code, Visual Studio will consider that the build failed.
if NOT EXIST "codeproc.exe" (
   echo WARNING: Couldn't find main executable 'codeproc.exe'
   goto end
)

if "%project%" == "client" (
   codeproc.exe --file ../Client/ClientInfo.cs --var Version     --type string --value $VERSION$ --projects "..\Client\Client.csproj"
   codeproc.exe --file ../Client/ClientInfo.cs --var BuildDate   --type string --value $DATE$
   codeproc.exe --file ../Client/ClientInfo.cs --var BuildNumber --type int    --value $INCREMENT$
) else if "%project%" == "server" (
   codeproc.exe --file ../Server.Modules.Common/ServerInfo.cs --var Version     --type string --value $VERSION$ --projects "..\Server\Server.csproj" "..\Server.Modules.Common\Server.Modules.Common.csproj" "..\Server.Modules.ReNote\Server.Modules.ReNote.csproj" "..\Server.Modules.Web\Server.Modules.Web.csproj"
   codeproc.exe --file ../Server.Modules.Common/ServerInfo.cs --var BuildDate   --type string --value $DATE$
   codeproc.exe --file ../Server.Modules.Common/ServerInfo.cs --var BuildNumber --type int    --value $INCREMENT$
) else (
   echo Usage: codeprocessor.bat [client | server]
)

:end
exit /b 0