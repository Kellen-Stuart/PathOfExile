$process = Get-Process -Name Macro -ErrorAction SilentlyContinue

if($process -ne $null)
{
    throw "Macro is already running"
}

# Find the exe
$executable = ls -Recurse -File Macro.exe

if(-not (Test-Path $executable))
{
    throw "Unable to find Macro.exe. Please build the project before attempting to run."
}

Start-Process $executable

Get-Process -Name Macro