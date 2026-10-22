
# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["TodoAppAPI/TodoAppAPI.csproj", "TodoAppAPI/"]
COPY ["TodoAppApplication/TodoAppApplication.csproj", "TodoAppApplication/"]
COPY ["TodoAppCore/TodoAppCore.csproj", "TodoAppCore/"]
COPY ["TodoAppDTOs/TodoAppDTOs.csproj", "TodoAppDTOs/"]
COPY ["TodoAppInfrastructure/TodoAppInfrastructure.csproj", "TodoAppInfrastructure/"]

# Restore dependencies
RUN dotnet restore "TodoAppAPI/TodoAppAPI.csproj"

# Copy source code
COPY . .

# Publish
RUN dotnet publish "TodoAppAPI/TodoAppAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# 2. Migration stage
FROM build AS migrate

RUN dotnet tool install \
    --tool-path /tools \
    dotnet-ef \
    --version 10.0.13

ENTRYPOINT ["/tools/dotnet-ef", "database", "update", \
    "--project", "TodoAppInfrastructure/TodoAppInfrastructure.csproj", \
    "--startup-project", "TodoAppAPI/TodoAppAPI.csproj", \
    "--configuration", "Release", \
    "--no-build"]


# 3. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

USER $APP_UID

ENTRYPOINT ["dotnet", "TodoAppAPI.dll"]
