#Runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
USER root
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_HTTP_PORTS=80
ENV ASPNETCORE_HTTPS_PORTS=443
ENV ASPNETCORE_URLS=http://*:80/;https://*:443/;


#Build 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet build FinHealthAPI/FinHealthAPI.csproj -c Release -o /app/build 


FROM build AS publish
USER root
RUN dotnet publish FinHealthAPI/FinHealthAPI.csproj -c Release -o /app/publish
FROM build-env AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "FinHealthAPI.dll" ]

ENV ConnectionStrings__Db_Local="Server=172.17.0.2,1433;Database=master;User Id=sa;Password=Lucho_2186;Encrypt=False;TrustServerCertificate=True"