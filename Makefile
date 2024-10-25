export IMAGE?=docker pull ghcr.io/atrakic/echo.api:latest
export CONTAINERAPP_NAME?=echo-api
export CONTAINERAPPS_ENVVARS?=USERNAME=admin,PASSWORD=admin123

.PHONY: all build infra-up infra-down test clean

all: build
	dotnet run --project ./src/EchoAPI/EchoAPI.csproj

build:
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal


infra-up infra-down:
	./infra/setup.sh $@

test:
	dotnet test

clean:
	dotnet clean
	rm -rf ./src/*/*.db
	rm -rf ./src/*/{bin,obj}
	rm -rf ./tests/*/{bin,obj}
