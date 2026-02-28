# Usa la imagen oficial de .NET 8 para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos de proyecto y restaura dependencias
COPY ["HomeHub.Api/HomeHub.Api.csproj", "HomeHub.Api/"]
COPY ["HomeHub.Application/HomeHub.Application.csproj", "HomeHub.Application/"]
COPY ["HomeHub.Infrastructure/HomeHub.Infrastructure.csproj", "HomeHub.Infrastructure/"]
RUN dotnet restore "HomeHub.Api/HomeHub.Api.csproj"

# Copia el resto del código y compila
COPY . .
WORKDIR "/src/HomeHub.Api"
RUN dotnet publish "HomeHub.Api.csproj" -c Release -o /app/publish

# Imagen final: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render.com expone el puerto 10000 por defecto
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Ejecuta la aplicación
ENTRYPOINT ["dotnet", "HomeHub.Api.dll"]