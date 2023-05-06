@echo off

set PROJECT=%1

if "%PROJECT%" == "" (
   goto usage
)

if NOT "%PROJECT%" == "client" (
   if NOT "%PROJECT%" == "server" (
      goto usage
   )
)

set BUILD_ARCH=x64
set DOTNET_VERSION=net7.0
set CONFIGURATION=Debug

REM Sets the configuration
if NOT "%2" == "" (
   set CONFIGURATION=%2
)

REM Sets the architecture
if NOT "%3" == "" (
   set BUILD_ARCH=%3
)

REM Sets the dotnet version
if NOT "%4" == "" (
   set DOTNET_VERSION=%4
)

REM Builds the Code Preprocessor
dotnet build ..\ReNote.CodeProcessor\ReNote.CodeProcessor.csproj --no-self-contained --runtime win-%BUILD_ARCH%

REM Creates the processor directory
mkdir processor

REM Moves output of the Code Preprocessor to the Tools\processor directory
call move_output.bat ..\ReNote.CodeProcessor\bin\%CONFIGURATION%\%DOTNET_VERSION%\win-%BUILD_ARCH% processor ReNote.CodeProcessor.exe codeproc.exe

REM Runs the Code Preprocessor
call codeprocessor.bat %PROJECT%

REM Builds the Project
if "%PROJECT%" == "client" (
   dotnet build ..\Client\Client.csproj -c %CONFIGURATION% --no-self-contained --runtime win-%BUILD_ARCH%
) else (
   dotnet build ..\Server\Server.csproj -c %CONFIGURATION% --no-self-contained --runtime win-%BUILD_ARCH%
)

goto end

:usage
echo Usage: build_project.bat ^<client ^| server^> [Configuration] [Architecture] [.NET Version]
exit /b 1

:end