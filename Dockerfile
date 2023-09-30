FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY ./Organizarty.Application/Organizarty.Application.csproj ./Organizarty.Application/
COPY ./Organizarty.Infra/Organizarty.Infra.csproj ./Organizarty.Infra/

RUN dotnet restore ./Organizarty.Application/Organizarty.Application.csproj

COPY . ./

RUN dotnet publish -c Release -o out ./Organizarty.Application/Organizarty.Application.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Organizarty.Application.dll"]
