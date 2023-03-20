FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RabbitMQConsumerDemo/RabbitMQConsumerDemo.csproj", "RabbitMQConsumerDemo/"]
RUN dotnet restore "RabbitMQConsumerDemo/RabbitMQConsumerDemo.csproj"
COPY . .
WORKDIR "/src/RabbitMQConsumerDemo"
RUN dotnet build "RabbitMQConsumerDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQConsumerDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQConsumerDemo.dll"]
