FROM ubuntu:latest AS base
USER $APP_UID
RUN apt-get update && apt-get install -y ffmpeg dotnet-runtime-8.0
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["encouraging_bot/encouraging_bot.csproj", "encouraging_bot/"]
RUN dotnet restore "encouraging_bot/encouraging_bot.csproj"
COPY . .
WORKDIR "/src/encouraging_bot"
RUN dotnet build "encouraging_bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "encouraging_bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "encouraging_bot.dll"]
