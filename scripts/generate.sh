#!/usr/bin/env bash

out="${1}"

# https://github.com/OpenAPITools/openapi-generator/blob/master/docs/generators/aspnetcore.md
# docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli help config-help
# docker run --rm -v "${PWD}:/local" openapitools/openapi-generator-cli config-help -g aspnetcore

spec=openapi.yaml
image=openapitools/openapi-generator-cli
docker pull -q "$image"

docker run --rm -e CSHARP_POST_PROCESS_FILE=${CSHARP_POST_PROCESS_FILE} -v "${PWD}:/local" openapitools/openapi-generator-cli generate \
    --skip-validate-spec \
    --enable-post-process-file \
    -c /local/config.yaml \
    -i /local/"$spec" \
    -g aspnetcore \
    -o /local/"$out"

dos2unix $(find . -type f -exec grep -I -q . {} \; -print)
pre-commit run -a
