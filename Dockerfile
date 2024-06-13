
# docker buildx build -t foo .
# docker run -it --rm -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development foo

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
LABEL org.opencontainers.image.description="A simple API to echo messages"
WORKDIR /app
EXPOSE 8080
EXPOSE 8443


FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src
COPY src/EchoAPI/EchoAPI.csproj .
RUN dotnet restore -a $TARGETARCH
COPY src/Echo.API/. .


FROM build AS publish
RUN dotnet publish -a $TARGETARCH --no-restore -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["./Echo.API"]
