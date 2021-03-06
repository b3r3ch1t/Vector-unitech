#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Vector-unitech-api/Vector-unitech-api.csproj", "Vector-unitech-api/"]
COPY ["ErroSentry/vector-unitech-log.csproj", "ErroSentry/"]
COPY ["vector-unitech-core/vector-unitech-core/vector-unitech-core.csproj", "vector-unitech-core/vector-unitech-core/"]
COPY ["vector-unitech-service/vector-unitech-service.csproj", "vector-unitech-service/"]
COPY ["vector-unitech-data/vector-unitech-data/vector-unitech-data.csproj", "vector-unitech-data/vector-unitech-data/"]
COPY ["vector-unitech-application/vector-unitech-application.csproj", "vector-unitech-application/"]
RUN dotnet restore "Vector-unitech-api/Vector-unitech-api.csproj"
COPY . .
WORKDIR "/src/Vector-unitech-api"
RUN dotnet build "Vector-unitech-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vector-unitech-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vector-unitech-api.dll"]
