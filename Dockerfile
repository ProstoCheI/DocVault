# Docker builds the Windows application; its interactive UI runs on Windows.
FROM mcr.microsoft.com/dotnet/sdk:9.0.317@sha256:f190d2dd9eef2899c91ac323caa0bd2b39334a5400ba93013e5199da39dad940 AS build
WORKDIR /src
COPY global.json DocVaultLocal.csproj packages.lock.json ./
RUN dotnet restore DocVaultLocal.csproj -r win-x64 --locked-mode
COPY . .
RUN dotnet publish DocVaultLocal.csproj -c Release -r win-x64 --self-contained true --no-restore -p:PublishSingleFile=true -o /app/publish

# One-shot export used by Compose; no fake background service.
FROM build AS export
WORKDIR /out
ENTRYPOINT ["/bin/sh", "-c", "cp -a /app/publish/. /out/"]

# BuildKit exports only the published files without loading an image.
FROM scratch AS artifacts
COPY --from=build /app/publish/ /
