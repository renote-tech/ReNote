@echo off

REM Builds the Code Preprocessor
dotnet build ../ReNote.CodeProcessor/ReNote.CodeProcessor.csproj

REM Runs the Code Preprocessor
call codeprocessor.bat server

REM Builds the Server
dotnet build ../Server/Server.csproj