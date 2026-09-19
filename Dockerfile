# Stage 1: Build stage using official .NET 9 SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["DocVaultLocal.csproj", "./"]
RUN dotnet restore "DocVaultLocal.csproj" /p:TargetFramework=net9.0

# Copy full source and publish
COPY . .
RUN dotnet publish "DocVaultLocal.csproj" -c Release -f net9.0 -o /app/publish

# Stage 2: Minimal runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

# Create storage directory for documents
RUN mkdir -p /app/Storage

# Copy published artifacts from build stage
COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Production

# Container Entrypoint
ENTRYPOINT ["dotnet", "DocVaultLocal.dll"]
