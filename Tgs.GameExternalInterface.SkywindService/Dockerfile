#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameExternalInterface.SkywindService/Tgs.GameExternalInterface.SkywindService.csproj", "Tgs.GameExternalInterface.SkywindService/"]
COPY ["Tgs.GameExternalInterface.SkywindService.Common/Tgs.GameExternalInterface.SkywindService.Common.csproj", "Tgs.GameExternalInterface.SkywindService.Common/"]
COPY ["Tgs.GameExternalInterface.SkywindService.Core/Tgs.GameExternalInterface.SkywindService.Core.csproj", "Tgs.GameExternalInterface.SkywindService.Core/"]
COPY ["Tgs.GameExternalInterface.SkywindService.Repositories/Tgs.GameExternalInterface.SkywindService.Repositories.csproj", "Tgs.GameExternalInterface.SkywindService.Repositories/"]
RUN dotnet restore "./Tgs.GameExternalInterface.SkywindService/./Tgs.GameExternalInterface.SkywindService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameExternalInterface.SkywindService"
RUN dotnet build "./Tgs.GameExternalInterface.SkywindService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameExternalInterface.SkywindService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameExternalInterface.SkywindService.dll"]