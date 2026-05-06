FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
USER app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /build

COPY ["Directory.Build.props", "."]
COPY ["src/Appointments.Domain/Appointments.Domain.csproj", "src/Appointments.Domain/"]
COPY ["src/Appointments.Domain/packages.lock.json", "src/Appointments.Domain/"]
COPY ["src/Appointments.Application/Appointments.Application.csproj", "src/Appointments.Application/"]
COPY ["src/Appointments.Application/packages.lock.json", "src/Appointments.Application/"]
COPY ["src/Appointments.Infrastructure/Appointments.Infrastructure.csproj", "src/Appointments.Infrastructure/"]
COPY ["src/Appointments.Infrastructure/packages.lock.json", "src/Appointments.Infrastructure/"]
COPY ["src/Appointments.Api/Appointments.Api.csproj", "src/Appointments.Api/"]
COPY ["src/Appointments.Api/packages.lock.json", "src/Appointments.Api/"]

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Appointments.Api/Appointments.Api.csproj" --locked-mode

COPY . .

WORKDIR "/build/src/Appointments.Api"

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet build "Appointments.Api.csproj" -c Release -o /app/build --no-restore
 
FROM build AS publish
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish "Appointments.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Appointments.Api.dll"]
