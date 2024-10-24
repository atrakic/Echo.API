all: build
	dotnet run --project ./src/EchoAPI/EchoAPI.csproj

build:
	dotnet restore
	dotnet build --configuration Release --no-restore
	dotnet test --no-restore --verbosity normal

test:
	dotnet test

clean:
	dotnet clean
