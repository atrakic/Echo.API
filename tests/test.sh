#!/bin/bash

set -ex

BEARER=$(curl -s -X POST http://localhost:5000/token \
  -H "Content-Type: application/json" \
  -d '{"Username":"admin","Password":"admin123"}' | jq -r ".token")

curl -v -H "Authorization: Bearer $BEARER" http://localhost:5000/api/message/1

curl -v -X "DELETE" -H "Authorization: Bearer $BEARER" http://localhost:5000/api/message/1

curl -v -H "Authorization: Bearer $BEARER" -H "Content-Type: application/json" \
  -d '{"message":"Hello World"}' http://localhost:5000/api/message
