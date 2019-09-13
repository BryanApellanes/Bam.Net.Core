FROM mono:3.10-onbuild
CMD ["bamweb.exe", "/S", "/content:/bam/content", "/verbose"]