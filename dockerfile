# Docker File contains instructions to build our images

# FROM Keyword imports the baseimage to build our image upon
# In our case, we are using .NET SDK as our initial base image 
FROM mcr.microsoft.com/dotnet/sdk:6.0 As build

WORKDIR /app

# Copy the whole folder (minus the dockerignored files/folders) in local machine to the work directory in my image
COPY . .

# For running any commands you would run in terminal
RUN dotnet clean WebAPI
RUN dotnet publish WebAPI --configuration Release -o ./publish

# Multi Stage build
FROM mcr.microsoft.com/dotnet/aspnet:6.0 As run

WORKDIR /app

COPY --from=build /app/publish .

# When user runs our image in their container, execute dotnet WebAPI.dll
CMD [ "dotnet", "WebAPI.dll" ]