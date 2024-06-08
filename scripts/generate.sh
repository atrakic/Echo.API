
out="${1}"

# https://github.com/OpenAPITools/openapi-generator/blob/master/docs/generators/aspnetcore.md
# docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli help config-help
# docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli config-help -g aspnetcore

docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli generate \
    --skip-validate-spec \
    -c /local/config.yaml \
    -i /local/echo.yaml \
    -g aspnetcore \
    -o /local/out/"$out"
