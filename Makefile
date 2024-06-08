all:
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal
	dotnet run --project src/Echo.OpenAPI/Echo.OpenAPI.csproj
