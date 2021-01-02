# web-api-nginx-mtls
Web Api behind NGINX with mTLS authentication &amp; SSL termination

- You need to have installed ```Docker Desktop on Windows``` with Linux containers by default.
- Use as startup project ```WebApplicationWithNGINX\docker-compose```.
- There is simple CA in ```certificates``` directory.
- Import ```/certificates/my-ca/certs/my-ca.pem.crt``` into ```Local Machine``` - ```Trusted Root Certification Authorities``` this will help browsers to trust server certificate.
