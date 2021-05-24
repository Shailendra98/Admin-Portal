#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
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

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/web/AdminPortal/AdminPortal.Main/AdminPortal.Main.csproj", "src/web/AdminPortal/AdminPortal.Main/"]
COPY src/SharedKernel src/SharedKernel/
COPY src/ApplicationCore src/ApplicationCore/
COPY src/Queries src/Queries/
COPY src/Infrastructure src/Infrastructure/
COPY src/web/AdminPortal src/web/AdminPortal/
RUN dotnet restore "src/web/AdminPortal/AdminPortal.Main/AdminPortal.Main.csproj"
WORKDIR "/src/src/web/AdminPortal/AdminPortal.Main"
RUN dotnet build "AdminPortal.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminPortal.Main.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminPortal.dll"]