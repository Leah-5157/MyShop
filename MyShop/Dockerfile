# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MyShop/MyShop.csproj", "MyShop/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["services/Services.csproj", "services/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY . .
RUN dotnet restore "MyShop/MyShop.csproj"
RUN dotnet build "MyShop/MyShop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyShop/MyShop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyShop.dll"]
