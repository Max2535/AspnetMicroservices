#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ordering.API/Ordering.API/Ordering.API.csproj", "Ordering.API/Ordering.API/"]
COPY ["Ordering.Application/Ordering.Application/Ordering.Application.csproj", "Ordering.Application/Ordering.Application/"]
COPY ["Ordering.Domain/Ordering.Domain/Ordering.Domain.csproj", "Ordering.Domain/Ordering.Domain/"]
COPY ["Ordering.Infrastructure/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Ordering.Infrastructure/Ordering.Infrastructure/"]
RUN dotnet restore "Ordering.API/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/Ordering.API/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]