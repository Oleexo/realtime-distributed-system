FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/app/WebPusher/WebPusher.csproj", "WebPusher/"]
RUN dotnet restore "src/app/WebPusher/WebPusher.csproj"
COPY . .
WORKDIR "/src/WebPusher"
RUN dotnet build "WebPusher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebPusher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebPusher.dll"]
