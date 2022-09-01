FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/app/Distributor/Distributor.csproj", "Distributor/"]
RUN dotnet restore "src/app/Distributor/Distributor.csproj"
COPY . .
WORKDIR "/src/Distributor"
RUN dotnet build "Distributor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Distributor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Distributor.dll"]
