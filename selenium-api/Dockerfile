FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build-env
COPY . /app
WORKDIR /app
RUN dotnet publish -c Release -r linux-musl-x64 -o publish-folder
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine as runtime
COPY --from=build-env /app/publish-folder ./app
RUN apk add --no-cache \
    icu-libs \
    && rm -rf /var/cache/apk/*
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:80;http://localhost:1905
WORKDIR /app
ENTRYPOINT ["./selenium-api"]