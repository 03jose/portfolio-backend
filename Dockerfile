# Etapa base: ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Render espera que la app escuche en el puerto 10000
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Etapa de build: .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore JoseRicardoPortfolio.csproj
RUN dotnet publish JoseRicardoPortfolio.csproj -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JoseRicardoPortfolio.dll"]
