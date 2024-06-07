
all:
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal

bootstrap:
	dotnet new solution
	dotnet new xunit -o tests/DotnetOpenapi.Tests
	dotnet sln add tests/DotnetOpenapi.Tests/DotnetOpenapi.Tests.csproj
	pushd tests/DotnetOpenapi.Tests
	dotnet add reference ../../src/src.csproj
	popd
	dotnet new gitignore --force
	dotnet new tool-manifest --force
	dotnet tool install Microsoft.dotnet-openapi
	dotnet tool list
        
