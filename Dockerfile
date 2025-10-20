# syntax=docker/dockerfile:1

# Build Stage (Server ve Test Projeleri)
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# EF Core araçlarını yükle
RUN apk add --no-cache bash
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Projeleri kopyala (hem server hem test)
COPY Todo_App/ /source/Todo_App/
COPY Todo_Test/ /source/Todo_Test/

WORKDIR /source/Todo_App
ARG TARGETARCH

# Restore ve Build Server
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore Todo_App.csproj --no-cache

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet build -c Release Todo_App.csproj

# Test Stage
FROM build AS test

WORKDIR /source/Todo_Test

# Testleri çalıştır
RUN dotnet test Todo_Test.csproj --verbosity normal

# Publish Stage
FROM build AS publish

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish Todo_App.csproj -c Release -o /app --no-self-contained

# Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

WORKDIR /app
COPY --from=publish /app .

USER $APP_UID
ENTRYPOINT ["dotnet", "Todo_App.dll"]
