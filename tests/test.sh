#!/bin/bash

set -euo pipefail

API_URL="http://localhost:5000"

BEARER=$(curl -s -X POST "$API_URL"/token \
  -H "Content-Type: application/json" \
  -d '{"Username":"admin","Password":"admin123"}' | jq -r ".token")

curl -v -H "Authorization: Bearer $BEARER" "$API_URL"/api/v1/message/1

curl -v -X "DELETE" -H "Authorization: Bearer $BEARER" "$API_URL"/api/v1/message/1

curl -v -H "Authorization: Bearer $BEARER" -H "Content-Type: application/json" \
  -d '{"message":"Hello World"}' "$API_URL"/api/v1/message
