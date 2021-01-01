https://kupczynski.info/2013/04/21/creating-your-own-certificates.html
https://gist.github.com/Soarez/9688998
https://jamielinux.com/docs/openssl-certificate-authority/appendix/intermediate-configuration-file.html
https://easyengine.io/wordpress-nginx/tutorials/ssl/multidomain-ssl-subject-alternative-names/
https://www.digicert.com/kb/ssl-support/openssl-quick-reference-guide.htm
http://chschneider.eu/linux/server/openssl.shtml

# CA sign cert request
openssl ca -config openssl.cnf -policy policy_anything -out certs/localhost.pem -infiles ../csr/localhost/localhost.csr
password: P@#ssw0rd

# Create self-signed server certificate
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout localhost.key -out /mnt/f/certificates/localhost.crt -config /mnt/f/certificates/localhost.conf -passin pass:P@ssw0rd

# Pack public & private keys 
openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt
openssl pkcs12 -export -out client1.pfx -inkey client1.pem -in ../../my-ca/certs/client1.pem

# Convert DER format into PEM 
openssl x509 -inform der -in my-public-certificate.der -out my-public-certificate.pem

# Create request for server certificate
openssl req -config localhost.conf -new -newkey rsa:2048 -nodes -keyout localhost.pem -out localhost.csr
openssl req -text -in localhost.csr -noout -verify