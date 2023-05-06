@echo off

set CONTENT_DIR=%1
set OUTPUT_DIR=%2
set OLD_EXECUTABLE_NAME=%3
set NEW_EXECUTABLE_NAME=%4

if "%CONTENT_DIR%" == "" (
    goto usage
) else if "%OUTPUT_DIR%" == "" (
    goto usage
) else if "%OLD_EXECUTABLE_NAME%" == "" (
    goto usage
) else if "%NEW_EXECUTABLE_NAME%" == "" (
    goto usage
) else (
   echo Moving output from '%CONTENT_DIR%' to '%OUTPUT_DIR%'
   xcopy /E /Y /C "%CONTENT_DIR%\*" "%OUTPUT_DIR%"
   rd /S /Q "%CONTENT_DIR%"

   echo Renaming main executable from '%OLD_EXECUTABLE_NAME%' to '%NEW_EXECUTABLE_NAME%
   move /Y "%OUTPUT_DIR%\%OLD_EXECUTABLE_NAME%" "%OUTPUT_DIR%\%NEW_EXECUTABLE_NAME%"

   goto :end
)

:usage
echo Usage: move_output.bat <CONTENT_DIR> <OUTPUT_DIR> <OLD_EXECUTABLE_NAME> <NEW_EXECUTABLE_NAME>
exit /b 1

:end