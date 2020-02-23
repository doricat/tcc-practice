#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/AutoCancellation.Service/AutoCancellation.Service.csproj", "src/AutoCancellation.Service/"]
COPY ["src/ViewModels.Shared/ViewModels.Shared.csproj", "src/ViewModels.Shared/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/AutoCancellation.Service/AutoCancellation.Service.csproj"
COPY . .
WORKDIR "/src/src/AutoCancellation.Service"
RUN dotnet build "AutoCancellation.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoCancellation.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoCancellation.Service.dll"]