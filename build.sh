#!/bin/bash
#Add the dotnet path to the path
export PATH="$HOME/.dotnet":$PATH

if [ -d "./artifacts" ]
then
    rm -Rf "./artifacts"; 
fi

dotnet restore

commitHash=$(git rev-parse --short HEAD)
suffix="-ci-local"
buildSuffix="$suffix-$commitHash"

echo "build: Version suffix is $buildSuffix"

dotnet build -c Release --version-suffix "$buildSuffix"  -v q /nologo

echo "Running unit tests"
dotnet test ./tests/UnitTests/UnitTests.csproj

echo "Starting docker containers"
docker-compose -f build/docker-compose-infrastructure.yml up -d

echo "Runing functional tests SqlServer"
export Data__Store='SqlServer'
export Data__ConnectionString='Server=tcp:localhost,5433;Initial Catalog=Esquio.UI.Tests;User Id=sa;Password=Password12!'
dotnet test ./tests/FunctionalTests/FunctionalTests.csproj

echo "Runing functional tests Postgress"
export Data__Store='NpgSql'
export Data__ConnectionString='Host=localhost;Port=5434;Database=Esquio.UI.Tests;User Id=postgres;Password=Password12!'
dotnet test ./tests/FunctionalTests/FunctionalTests.csproj

echo "Finalizing docker containers"
docker-compose -f build/docker-compose-infrastructure.yml down

dotnet pack ./src/Esquio/Esquio.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
dotnet pack ./src/Esquio.AspNetCore/Esquio.AspNetCore.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
dotnet pack ./src/Esquio.Configuration.Store/Esquio.Configuration.Store.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
dotnet pack ./src/Esquio.Http.Store/Esquio.Http.Store.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
dotnet pack ./src/Esquio.AspNetCore.ApplicationInsightProcessor/Esquio.AspNetCore.ApplicationInsightProcessor.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
dotnet pack ./src/Esquio.Blazor.WebAssembly/Esquio.Blazor.WebAssembly.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
dotnet pack ./tools/MiniProfiler.Esquio/MiniProfiler.Esquio.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
dotnet pack ./tools/Esquio.Cli/Esquio.Cli.csproj -c Release -o ./artifacts --include-symbols --no-build --version-suffix=$buildSuffix
