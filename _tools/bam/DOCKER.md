# Build
docker build -t {{ProcessTagName}} . [-f path_to_Dockerfile]

# Stop & Remove
docker container stop {{ProcessTagName}}
docker container rm {{ProcessTagName}}
docker container stop {{ProcessTagName}}instance
docker container rm {{ProcessTagName}}instance

# Run
docker run -d -p 8080:80 --name {{ProcessTagName}}instance {{ProcessTagName}}

# Push
docker push bamapps/images:{{ProcessTagName}}