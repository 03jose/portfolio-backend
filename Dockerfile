# Usa la imagen oficial de .NET 8 SDK para compilar y ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore JoseRicardoPortfolio.csproj
RUN dotnet publish JoseRicardoPortfolio.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JoseRicardoPortfolio.dll"]
