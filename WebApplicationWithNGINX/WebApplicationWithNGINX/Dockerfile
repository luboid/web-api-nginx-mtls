#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
# nuget & client cert validation need root certs
# COPY ca-root/procreditbank-root-ca.crt /usr/local/share/ca-certificates
# COPY ca-root/pcb-ca-root.crt /usr/local/share/ca-certificates
# COPY ca-root/pcb-ca-sub.crt /usr/local/share/ca-certificates
# RUN update-ca-certificates
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WebApplicationWithNGINX/WebApplicationWithNGINX.csproj", "WebApplicationWithNGINX/"]
# nuget & client cert validation need root certs
# COPY ca-root/procreditbank-root-ca.crt /usr/local/share/ca-certificates
# COPY ca-root/pcb-ca-root.crt /usr/local/share/ca-certificates
# COPY ca-root/pcb-ca-sub.crt /usr/local/share/ca-certificates
# RUN update-ca-certificates
RUN dotnet restore "WebApplicationWithNGINX/WebApplicationWithNGINX.csproj"
COPY . .
WORKDIR "/src/WebApplicationWithNGINX"
RUN dotnet build "WebApplicationWithNGINX.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplicationWithNGINX.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplicationWithNGINX.dll"]