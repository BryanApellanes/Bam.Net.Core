# Build
FROM microsoft/dotnet:sdk AS build-env

RUN apt-get update \
    && apt-get install -y --no-install-recommends git-core curl build-essential openssl libssl-dev unzip \
    && rm -rf /var/lib/apt/lists/* 

RUN mkdir -p /root/.bam/src

WORKDIR /root 

COPY . ./

RUN mv ./_ssh/ ./.ssh \
    && chmod 700 ./.ssh \
    && chmod 644 ./.ssh/id_rsa.pub \
    && chmod 600 ./.ssh/id_rsa \
    && git clone https://github.com/BryanApellanes/BamToolkit.git ./.bam/src/BamToolkit --recursive

WORKDIR /root/.bam/src/BamToolkit

ENV DIST=/tmp/bam
RUN ./build-tool.sh bamweb

WORKDIR /root 
RUN rm -fr /root/.bam/src

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime

RUN apt-get update \
    && apt-get install -y --no-install-recommends git-core curl build-essential openssl libssl-dev unzip python3 \
    && rm -rf /var/lib/apt/lists/* 

RUN ln -s /usr/bin/python3 /usr/bin/python

WORKDIR /root

COPY . ./
RUN ./install-nodejs.sh

COPY --from=build-env /root/.bam ./.bam
ENV ASPNETCORE_ENVIRONMENT=PROD
ENV PATH "$PATH:/root/.bam/toolkit/ubuntu.16.10-x64/bamweb:/root/.local/bin"
RUN ./install-aws-cli.sh

