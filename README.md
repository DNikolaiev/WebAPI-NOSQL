# WebAPI-NOSQL

#### This is Demo .NET Core WebApi Application that uses MongoDb for data storage and Redis to provide distributed caching mechanism. API is documented with Swagger. Navigate to */swagger* Url to test it

![UI](https://user-images.githubusercontent.com/23034890/133496984-d1f5c9f7-2260-43ed-ac10-542f84fc1aab.png)
![Getproduct](https://user-images.githubusercontent.com/23034890/133497343-70ff4862-b160-49df-b483-a41b273d8b72.png)

# Prerequisites
- Docker Desktop installed on local machine

# Quickstart
- start Powershell as Administrator
- start Docker Desktop and switch to Linux containers
- run **docker-comose up -d**
- per default App will be running on port 5001, navigate to following Url to run the app *localhost:5001/swagger*. If port is already in use, stop process on port 5001 or change port variable in docker-compose.yml file to map container's default port to the other port locally
