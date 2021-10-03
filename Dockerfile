FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Vue.Splash-API/*.csproj ./Vue.Splash-API/
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true

# copy everything else and build app
COPY Vue.Splash-API/. ./Vue.Splash-API/
WORKDIR /source/Vue.Splash-API
RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-amd64
WORKDIR /app
COPY --from=build /app ./


ENTRYPOINT ["./Vue.Splash-API"]
