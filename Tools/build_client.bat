@echo off

REM Builds the Code Preprocessor
dotnet build ../ReNote.CodeProcessor/ReNote.CodeProcessor.csproj

REM Runs the Code Preprocessor
call codeprocessor.bat client

REM Builds the Client
dotnet build ../Client/Client.csproj