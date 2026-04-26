FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Service/Catalog/Catalog.API/Catalog.API.csproj", "Service/Catalog/Catalog.API/"]
COPY ["Service/Catalog/Catalog.Application/Catalog.Application.csproj", "Service/Catalog/Catalog.Application/"]
COPY ["Service/Catalog/Catalog.Core/Catalog.Core.csproj", "Service/Catalog/Catalog.Core/"]
COPY ["Service/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Service/Catalog/Catalog.Infrastructure/"]
RUN dotnet restore "Service/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Service/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Catalog.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
