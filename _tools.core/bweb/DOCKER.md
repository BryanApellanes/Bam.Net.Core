# Build
docker build -t bams . [-f path_to_Dockerfile]

# Stop & Remove
docker container stop bams
docker container rm bams
docker container stop bamsinstance
docker container rm bamsinstance

# Run
docker run -d -p 8080:80 --name bamsinstance bams

# Push
docker push bamapps/images:bams