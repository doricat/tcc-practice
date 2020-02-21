#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Order.Api.Web/Order.Api.Web.csproj", "src/Order.Api.Web/"]
RUN dotnet restore "src/Order.Api.Web/Order.Api.Web.csproj"
COPY . .
WORKDIR "/src/src/Order.Api.Web"
RUN dotnet build "Order.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Api.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Api.Web.dll"]