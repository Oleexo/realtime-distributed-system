FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/app/GrpcPusher/GrpcPusher.csproj", "GrpcPusher/"]
RUN dotnet restore "src/app/GrpcPusher/GrpcPusher.csproj"
COPY . .
WORKDIR "/src/GrpcPusher"
RUN dotnet build "GrpcPusher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcPusher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcPusher.dll"]
