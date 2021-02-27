FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ./src/ ./
RUN dotnet restore ./Noteify.Web/Noteify.Web.csproj
RUN dotnet restore ./Noteify.Data/Noteify.Data.csproj
RUN dotnet publish ./Noteify.Web/Noteify.Web.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app

COPY --from=build-env /app/out .
CMD ["dotnet", "/app/Noteify.Web.dll"]