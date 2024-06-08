
all:
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal

help-generator:
	docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli help config-help

bootstrap:
	dotnet new tool-manifest --force
	dotnet tool install Microsoft.dotnet-openapi
	dotnet tool list
	#dotnet new solution
	#dotnet new gitignore --force
	#dotnet new xunit -o tests/DotnetOpenapi.Tests
	#dotnet sln add tests/DotnetOpenapi.Tests/DotnetOpenapi.Tests.csproj
	#pushd tests/DotnetOpenapi.Tests
	#dotnet add reference ../../src/src.csproj
	#popd
