
all:
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal

HELP_ARGS ?= "help"

help: ### make help HELP_ARGS="help generate"
	docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli $(HELP_ARGS)

bootstrap:
	dotnet new tool-manifest --force
	dotnet tool install Microsoft.dotnet-openapi
	dotnet tool list
