FROM mono:latest

RUN mono --version
RUN msbuild bamweb.csproj

CMD ["bamweb.exe", "/S", "/content:/bam/content", "/verbose"]