FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoService/TodoService.csproj", "TodoService/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["Logging/Logging.csproj", "Logging/"]
RUN dotnet restore "TodoService/TodoService.csproj"
COPY . .
WORKDIR "/src/TodoService"
RUN dotnet build "TodoService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoService.dll"] 