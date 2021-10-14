#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["vector-unitech-api-gateway/vector-unitech-api-gateway.csproj", "vector-unitech-api-gateway/"]
RUN dotnet restore "vector-unitech-api-gateway/vector-unitech-api-gateway.csproj"
COPY . .
WORKDIR "/src/vector-unitech-api-gateway"
RUN dotnet build "vector-unitech-api-gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vector-unitech-api-gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vector-unitech-api-gateway.dll"]