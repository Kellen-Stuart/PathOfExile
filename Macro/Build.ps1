$sln = Join-Path $PSScriptRoot Macro.sln
if(-not (Test-Path $sln))
{
    throw "Unable to find Macro.sln"
}

dotnet build $sln -c Release