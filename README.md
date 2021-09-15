# WebAPI-NOSQL

#### This is Demo WebApi Application that uses MongoDb for data storage and Redis to provide distributed caching mechanism. API is documented with Swagger

# Prerequisites
- Docker Desktop installed on local machine

# Quickstart
- start Powershell as Administrator
- start Docker Desktop and switch to Linux containers
- run **docker-comose up -d**
- per default App will be running on port 5001, navigate to following Url to run the app *localhost:5001/swagger*. If port is already in use, stop process on port 5001 or change port variable in docker-compose.yml file to map container's default port to the other port locally
