# Build stage med .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Kopiera csproj från undermapp och gör restore
COPY TankGameServer/*.csproj ./TankGameServer/
RUN dotnet restore ./TankGameServer/TankGameServer.csproj

# Kopiera allt (hela projektet) för build
COPY . .

# Bygg och publicera projektet (ange sökväg till csproj)
RUN dotnet publish ./TankGameServer/TankGameServer.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Kopiera publicerad output från build steget
COPY --from=build /app/out ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "TankGameServer.dll"]
