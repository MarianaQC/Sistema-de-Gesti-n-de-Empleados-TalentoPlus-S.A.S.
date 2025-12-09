# ==========================================================
# ETAPA 1: BUILD (Compilación y Publicación)
# Se usa la imagen 9.0 SDK para compilar los proyectos net9.0.
# ==========================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos del proyecto para restaurar dependencias
COPY ["TalentoPlus.Api/TalentoPlus.Api.csproj", "TalentoPlus.Api/"]
COPY ["TalentoPlus.Application/TalentoPlus.Application.csproj", "TalentoPlus.Application/"]
COPY ["TalentoPlus.Domain/TalentoPlus.Domain.csproj", "TalentoPlus.Domain/"]
COPY ["TalentoPlus.Infrastructure/TalentoPlus.Infrastructure.csproj", "TalentoPlus.Infrastructure/"]

# Restaura las dependencias.
RUN dotnet restore "TalentoPlus.Api/TalentoPlus.Api.csproj"

# Copia el resto del código fuente.
COPY . .

# Compila y publica la aplicación.
WORKDIR /src/TalentoPlus.Api
RUN dotnet publish "TalentoPlus.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ==========================================================
# ETAPA 2: FINAL (Ejecución)
# Se usa la imagen 9.0 Runtime para la ejecución.
# ==========================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia los archivos publicados desde la etapa 'build'.
COPY --from=build /app/publish .

EXPOSE 80

# Comando de inicio de la aplicación.
ENTRYPOINT ["dotnet", "TalentoPlus.Api.dll"]
