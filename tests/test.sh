# curl -X POST http://localhost:5000/token -H "Content-Type: application/json" -d '{"Username":"your_username","Password":"your_password"}'
curl -v -H "Authorization: Bearer $BEARER" http://localhost:5121/api/messages/1
