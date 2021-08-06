$stop = Join-Path $PSScriptRoot Stop.ps1
$build = Join-Path $PSScriptRoot Build.ps1
$run = Join-Path $PSScriptRoot Run.ps1

if(-not (Test-Path $stop))
{
    throw "Unable to find Stop.ps1"
}

if(-not (Test-Path $build))
{
    throw "Unable to find Build.ps1"
}

if(-not (Test-Path $run))
{
    throw "Unable to find Run.ps1"
}

try {
    & $stop
}catch{}

& $build

& $run