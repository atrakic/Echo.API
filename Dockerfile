
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

# Copy project file and restore as distinct layers
COPY --link src/EchoApi/*.csproj .
RUN dotnet restore -a $TARGETARCH

FROM build AS publish
# Copy source code and publish app
COPY --link src/EchoApi/. .
ARG Configuration=Release
RUN dotnet publish -a $TARGETARCH --no-restore -c "${Configuration}" -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ARG APP_UID=1000
USER $APP_UID
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "EchoApi.dll"]