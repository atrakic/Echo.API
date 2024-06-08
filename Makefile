all:
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal
	dotnet user-jwts list
	dotnet run --project src/Echo.OpenAPI/Echo.OpenAPI.csproj
