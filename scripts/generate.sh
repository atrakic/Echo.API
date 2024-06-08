out="${1}"
docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli generate \
    --skip-validate-spec \
    -i /local/echo.yaml \
    -g csharp \
    -o /local/out/"$out"
