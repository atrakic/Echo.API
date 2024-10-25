#!/usr/bin/env bash

set -e
set -o pipefail

# Deploy Azure Container Apps infrastructure.

COMMAND="$1"
CONTAINERAPP_NAME=${CONTAINERAPP_NAME:?'You need to configure the CONTAINERAPP_NAME environment variable; eg. app-testaaaay'}
IMAGE=${IMAGE:?'You need to configure IMAGE environment variable; eg. docker.io/nginx:latest'}
LOCATION=${LOCATION:-northeurope}
RESOURCE_GROUP=${RESOURCE_GROUP:-rg-$RANDOM}
CONTAINERAPPS_ENVIRONMENT=${CONTAINERAPPS_ENVIRONMENT:-env-$CONTAINERAPP_NAME}
TARGET_PORT=${TARGET_PORT:-80}
CONTAINERAPPS_ENVVARS=${CONTAINERAPPS_ENVVARS:-} # eg. FOO=bar

# split env vars into array and surround element with single quotes
IFS=',' read -r -a CONTAINERAPPS_ENVVARS <<< "$CONTAINERAPPS_ENVVARS"
#echo "CONTAINERAPPS_ENVVARS: ${CONTAINERAPPS_ENVVARS[@]}"

case "$COMMAND" in
  infra-up)
    if [ $(az group exists --name "$RESOURCE_GROUP") = false ]; then
      az group create --name "$RESOURCE_GROUP" --location "$LOCATION"
    fi
    az containerapp env create --name "$CONTAINERAPPS_ENVIRONMENT" --resource-group "$RESOURCE_GROUP" --location "$LOCATION"
    az containerapp create --name "$CONTAINERAPP_NAME" \
      --resource-group "$RESOURCE_GROUP" \
      --environment "$CONTAINERAPPS_ENVIRONMENT" \
      --image "$IMAGE" \
      --env-vars "${CONTAINERAPPS_ENVVARS[@]}" \
      --target-port "$TARGET_PORT" \
      --ingress external \
      --query properties.configuration.ingress.fqdn
    ;;
  infra-down)
    az group delete --name "$RESOURCE_GROUP"
    ;;
  status)
    az container show --resource-group "$RESOURCE_GROUP" --name "$CONTAINERAPP_NAME" --query 'containers[].environmentVariables'
    ;;
  enter)
    az container exec --resource-group "$RESOURCE_GROUP" --name "$CONTAINERAPP_NAME" --exec-command "/bin/bash"
    ;;
  *)
    echo -n "Usage: $0 <infra-up|infra-down>"
esac
