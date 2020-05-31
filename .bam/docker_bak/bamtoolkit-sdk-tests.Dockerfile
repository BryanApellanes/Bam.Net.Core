FROM bamapps/bamtoolkit-sdk:latest AS test-env

WORKDIR /root/.bam/src/BamToolkit

ENV DIST=/tmp/bam
ENV BAMTOOLKITBIN=/root/.bam/toolkit/bin
ENV BAMTOOLKITSYMLINKS=/usr/local/bin
RUN ./build-tests.sh

ENTRYPOINT [ "run-unit-tests.sh" ]

