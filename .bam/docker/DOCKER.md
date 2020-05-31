# Build
docker build -t {{imageName} . [-f path_to_Dockerfile]

# Stop & Remove
docker container stop {{imageName}}
docker container rm {{imageName}}
docker container stop {{imageName}}instance
docker container rm {{imageName}}instance

# Run
docker run -d -p 8080:80 --name {{imageName}}instance {{imageName}}

# Push
docker push bryanapellanes/{{imageName}}:latest