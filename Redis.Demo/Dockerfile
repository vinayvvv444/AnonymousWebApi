FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Redis.Demo/Redis.Demo.csproj", "Redis.Demo/"]
RUN dotnet restore "Redis.Demo/Redis.Demo.csproj"
COPY . .
WORKDIR "/src/Redis.Demo"
RUN dotnet build "Redis.Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Redis.Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Redis.Demo.dll"]
