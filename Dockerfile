FROM ubuntu:rolling AS base
USER $APP_UID
RUN apt-get update && apt-get install -y ffmpeg dotnet-runtime-9.0
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["video_migration_tool/video_migration_tool.csproj", "video_migration_tool/"]
RUN dotnet restore "video_migration_tool/video_migration_tool.csproj"
COPY . .
WORKDIR "/src/video_migration_tool"
RUN dotnet build "video_migration_tool.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "video_migration_tool.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "video_migration_tool.dll"]
