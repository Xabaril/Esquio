$useSqlServer = $true
$useNpgsql = $true
$migrationName = "Next"


if ($useSqlServer){
    Write-Host "Running Migration for Sql Server with ($migrationName)"
    dotnet ef migrations add $migrationName --project src/Esquio.UI.SqlServer -v -o Infrastructure/Data/Migrations
}

if ($useNpgsql) {
    
    Write-Host "Running Migration for NpgSql with ($migrationName)"
    dotnet ef migrations add $migrationName --project src/Esquio.UI.Npgsql -v -o Infrastructure/Data/Migrations
}