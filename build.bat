:: Generated by: https://openapi-generator.tech
::

@echo off

dotnet restore src\Echo.OpenAPI
dotnet build src\Echo.OpenAPI
echo Now, run the following to start the project: dotnet run -p src\Echo.OpenAPI\Echo.OpenAPI.csproj --launch-profile web.
echo.
