FROM nginx:latest as BASE

COPY /Nginx/nginx.conf /etc/nginx/nginx.conf
# COPY /Nginx/localhost.crt /etc/ssl/certs/localhost.crt
# COPY /Nginx/localhost.key /etc/ssl/private/localhost.key

# update certificate store with procredit root CA autority alternative to trusted-certificate-store.pem
# COPY ca-root/procreditbank-root-ca.crt /usr/local/share/ca-certificates
# COPY ca-root/pcb-ca-root.crt /usr/local/share/ca-certificates
# COPY ca-root/pcb-ca-sub.crt /usr/local/share/ca-certificates
# RUN update-ca-certificates