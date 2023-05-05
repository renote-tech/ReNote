@echo off

set content_dir=%1
set output_dir=%2
set old_executable_name=%3
set new_executable_name=%4

if "%content_dir%" == "" (
    goto usage
) else if "%output_dir%" == "" (
    goto usage
) else if "%old_executable_name%" == "" (
    goto usage
) else if "%new_executable_name%" == "" (
    goto usage
) else (
   echo Moving output from '%content_dir%' to '%output_dir%'
   xcopy /E /Y /C "%content_dir%\*" "%output_dir%"
   del "%content_dir%\*"

   echo Renaming main executable from '%old_executable_name%' to '%new_executable_name%
   move /Y "%output_dir%\%old_executable_name%" "%output_dir%\%new_executable_name%"

   goto :end
)

:usage
echo Usage: move_output.bat CONTENT_DIR OUTPUT_DIR OLD_EXECUTABLE_NAME NEW_EXECUTABLE_NAME
exit /b 1

:end