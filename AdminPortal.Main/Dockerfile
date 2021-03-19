#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/web/AdminPortal/AdminPortal.csproj", "src/web/AdminPortal/"]
RUN dotnet restore "src/web/AdminPortal/AdminPortal.csproj"
COPY src/assemblies src/assemblies/
COPY src/ApplicationCore src/ApplicationCore/
COPY src/Infrastructure src/Infrastructure/
COPY src/web/AdminPortal src/web/AdminPortal/
WORKDIR "/src/src/web/AdminPortal"
RUN dotnet build "AdminPortal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminPortal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminPortal.dll"]