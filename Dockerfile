# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln ./
COPY TodoList.API/*.csproj ./TodoList.API/
COPY TodoList.Application/*.csproj ./TodoList.Application/
COPY TodoList.Domain/*.csproj ./TodoList.Domain/
COPY TodoList.Infrastructure/*.csproj ./TodoList.Infrastructure/

RUN dotnet restore
COPY . .
RUN dotnet publish TodoList.API/TodoList.API.csproj -c Release -o /app/out

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TodoList.API.dll"]