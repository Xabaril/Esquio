# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

$suffix = "-ci-local"
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = "$($suffix)-$($commitHash)"

echo "build: Version suffix is $buildSuffix"

exec { & dotnet build Esquio.sln -c Release --version-suffix=$buildSuffix -v q /nologo }
	
echo "Running unit tests"

try {

Push-Location -Path .\tests\UnitTests
        exec { & dotnet test}
} finally {
        Pop-Location
}

echo "Starting docker containers"

exec { & docker-compose -f build\docker-compose-infrastructure.yml up -d }

echo "Running functional tests"

try {

Push-Location -Path .\tests\FunctionalTests
        exec { & dotnet test}
} finally {
        Pop-Location
}

echo "Finalizing docker containers"
exec { & docker-compose -f build\docker-compose-infrastructure.yml down }


exec { & dotnet pack .\src\Esquio\Esquio.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$buildSuffix }
exec { & dotnet pack .\src\Esquio.AspNetCore\Esquio.AspNetCore.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$buildSuffix }
exec { & dotnet pack .\src\Esquio.Configuration.Store\Esquio.Configuration.Store.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$buildSuffix }
exec { & dotnet pack .\src\Esquio.EntityFrameworkCore.Store\Esquio.EntityFrameworkCore.Store.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$buildSuffix }
exec { & dotnet pack .\src\Esquio.AspNetCore.ApplicationInsightProcessor\Esquio.AspNetCore.ApplicationInsightProcessor.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$buildSuffix }
exec { & dotnet pack .\tools\MiniProfiler.Esquio\MiniProfiler.Esquio.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$buildSuffix }
