%1 mshta vbscript:CreateObject("Shell.Application").ShellExecute("cmd.exe","/c %~s0 ::","","runas",1)(window.close)&&exit cd /d "%~dp0"

@echo off
setlocal
cd /d %~dp0

AsZero.WebApi.Launcher.exe install
AsZero.WebApi.Launcher.exe start
AsZero.WebApi.Launcher.exe status

echo "install completed"
echo.  
<nul set /p "=press any key to continue..."
pause >nul