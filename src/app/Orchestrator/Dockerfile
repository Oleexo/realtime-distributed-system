FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
RUN apt-get update && apt-get install -y curl 
ENV ASPNETCORE_URLS=http://*:8000/
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/app/Orchestrator/Orchestrator.csproj"
RUN dotnet build "src/app/Orchestrator/Orchestrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/app/Orchestrator/Orchestrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oleexo.RealtimeDistributedSystem.Orchestrator.Api.dll"]
