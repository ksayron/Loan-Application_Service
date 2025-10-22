
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["TriInkom.csproj", "./"]
RUN dotnet restore "./TriInkom.csproj"


COPY . .
RUN dotnet publish "./TriInkom.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

RUN groupadd -r appuser && useradd -r -g appuser appuser \
    && mkdir -p /app && chown -R appuser:appuser /app

USER appuser
WORKDIR /app

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "TriInkom.dll"]
