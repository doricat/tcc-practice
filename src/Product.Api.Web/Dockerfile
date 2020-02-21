#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Product.Api.Web/Product.Api.Web.csproj", "src/Product.Api.Web/"]
RUN dotnet restore "src/Product.Api.Web/Product.Api.Web.csproj"
COPY . .
WORKDIR "/src/src/Product.Api.Web"
RUN dotnet build "Product.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product.Api.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.Api.Web.dll"]