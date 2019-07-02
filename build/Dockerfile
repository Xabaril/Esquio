# restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS restore-env

COPY . .

RUN dotnet restore src/Esquio.UI/Esquio.UI.csproj


# # build
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS build-env

COPY --from=restore-env . .

RUN dotnet build src/Esquio.UI/Esquio.UI.csproj -c Release --no-restore


# publish
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-bionic AS publish-env

COPY --from=build-env . .

RUN dotnet publish src/Esquio.UI/Esquio.UI.csproj -c Release -o out --no-build



# # # Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-bionic

WORKDIR /app

COPY --from=publish-env /out .

ENTRYPOINT ["dotnet", "Esquio.UI.dll"]
