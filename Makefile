
all: generate
	dotnet sln add src/Message.DAL/Message.DAL.csproj
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal
	dotnet run -p src/Echo.OpenAPI/Echo.OpenAPI.csproj

generate:
	./scripts/generate.sh

HELP_ARGS ?= config-help -g aspnetcore

help: ### make help HELP_ARGS=help
	docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli $(HELP_ARGS)

bootstrap:
	dotnet new tool-manifest --force
	dotnet tool install Microsoft.dotnet-openapi
	dotnet tool list
