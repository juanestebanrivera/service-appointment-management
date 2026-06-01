FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /build
COPY ["Directory.Build.props", "."]
COPY ["src/Catalog.Domain/Catalog.Domain.csproj", "src/Catalog.Domain/"]
COPY ["src/Catalog.Application/Catalog.Application.csproj", "src/Catalog.Application/"]
COPY ["src/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "src/Catalog.Infrastructure/"]
COPY ["src/Catalog.Api/Catalog.Api.csproj", "src/Catalog.Api/"]

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Catalog.Api/Catalog.Api.csproj"

COPY . .
WORKDIR "/build/src/Catalog.Api"

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet build "Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish "Catalog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]