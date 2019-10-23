FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app



FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS publish
#WORKDIR /src
COPY ./CaseBusiness/ CaseBusiness/
RUN dotnet restore CaseBusiness/CaseBusiness.csproj
#WORKDIR /src/CaseBusiness
RUN dotnet build CaseBusiness.csproj -f netcoreapp2.1 -c Release -o /app
#WORKDIR /src
COPY ./SMS.Api/ SMS.Api/
RUN dotnet restore SMS.Api/SMS.Api.csproj
#WORKDIR /src/SMS.Api
RUN dotnet publish SMS.Api.csproj -r linux-x64 -c Release -o /app

WORKDIR /app
RUN rm -rfv App_Config
RUN mkdir App_Config
RUN rm -rfv /src



FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet","SMS.Api.dll"]








