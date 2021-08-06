$process = Get-Process -Name Macro -ErrorAction SilentlyContinue

if($process -eq $null)
{
    throw "Macro is not running"
}

Stop-Process -Name Macro
Write-Host "Stopped Macro"