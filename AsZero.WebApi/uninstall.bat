%1 mshta vbscript:CreateObject("Shell.Application").ShellExecute("cmd.exe","/c %~s0 ::","","runas",1)(window.close)&&exit cd /d "%~dp0"

@echo off
setlocal
cd /d %~dp0

AsZero.WebApi.Launcher.exe stop
AsZero.WebApi.Launcher.exe uninstall
mshta javascript:alert("uninstall completed");close();
