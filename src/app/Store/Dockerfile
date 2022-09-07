FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/app/Store/Store.csproj", "Store/"]
RUN dotnet restore "src/app/Store/Store.csproj"
COPY . .
WORKDIR "/src/Store"
RUN dotnet build "Store.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Store.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Store.dll"]
