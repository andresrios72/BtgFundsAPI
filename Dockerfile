# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln . 
COPY BtgFundsApi/*.csproj ./BtgFundsApi/
COPY BtgFundsApi.Tests/*.csproj ./BtgFundsApi.Tests/
RUN dotnet restore

COPY . .
WORKDIR /app/BtgFundsApi
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS runtime
WORKDIR /app

# Instalar certificados actualizados
RUN apt-get update && apt-get install -y --no-install-recommends ca-certificates curl && rm -rf /var/lib/apt/lists/*
RUN update-ca-certificates

COPY --from=build /app/BtgFundsApi/out ./

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "BtgFundsApi.dll"]
