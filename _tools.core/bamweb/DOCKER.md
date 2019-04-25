# Build
docker build -t bamweb . [-f path_to_Dockerfile]

# Stop & Remove
docker container stop bamweb
docker container rm bamweb
docker container stop bamwebinstance
docker container rm bamwebinstance

# Run
docker run -d -p 8080:80 --name bamwebinstance bamweb

# Push
docker push bamapps/images:bamweb