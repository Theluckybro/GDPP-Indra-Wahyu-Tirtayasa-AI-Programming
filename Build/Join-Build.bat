@echo off
setlocal
cd /d "%~dp0"

set "TARGET=House of Silence_Data\sharedassets1.assets.resS"

if exist "%TARGET%" (
    echo Build is already assembled.
    echo You can run "House of Silence.exe".
    echo.
    pause
    exit /b 0
)

if not exist "%TARGET%.001" (
    echo ERROR: part files are missing.
    echo.
    echo They are stored with Git LFS. Install Git LFS, then run
    echo from the repository root:
    echo.
    echo     git lfs install
    echo     git lfs pull
    echo.
    pause
    exit /b 1
)

echo Joining the split player data, this takes a moment...
copy /b "%TARGET%.001" + "%TARGET%.002" + "%TARGET%.003" "%TARGET%" >nul
if errorlevel 1 (
    echo ERROR: could not join the parts.
    echo.
    pause
    exit /b 1
)

echo Done. You can now run "House of Silence.exe".
echo.
pause
