FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Palworld.Discord.Bot.Net.App/Palworld.Discord.Bot.Net.csproj", "Palworld.Discord.Bot.Net.App/"]
RUN dotnet restore "Palworld.Discord.Bot.Net.App/Palworld.Discord.Bot.Net.csproj"
COPY . .
WORKDIR "/src/Palworld.Discord.Bot.Net.App"
RUN dotnet build "Palworld.Discord.Bot.Net.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Palworld.Discord.Bot.Net.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Palworld.Discord.Bot.Net.dll"]
