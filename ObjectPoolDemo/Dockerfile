FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ObjectPoolDemo/ObjectPoolDemo.csproj", "ObjectPoolDemo/"]
RUN dotnet restore "ObjectPoolDemo/ObjectPoolDemo.csproj"
COPY . .
WORKDIR "/src/ObjectPoolDemo"
RUN dotnet build "ObjectPoolDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ObjectPoolDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ObjectPoolDemo.dll"]
