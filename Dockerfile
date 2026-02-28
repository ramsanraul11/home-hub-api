# ===============================
# Build stage
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj primero (cache de dependencias)
COPY HomeHub.Api/HomeHub.Api.csproj HomeHub.Api/
COPY HomeHub.Application/HomeHub.Application.csproj HomeHub.Application/
COPY HomeHub.Domain/HomeHub.Domain.csproj HomeHub.Domain/
COPY HomeHub.Infrastructure/HomeHub.Infrastructure.csproj HomeHub.Infrastructure/

RUN dotnet restore HomeHub.Api/HomeHub.Api.csproj

# Copiar todo el código
COPY . .

# Publicar
WORKDIR /src/HomeHub.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ===============================
# Runtime stage
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar artefactos publicados
COPY --from=build /app/publish .

# Render usa variable PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "HomeHub.Api.dll"]