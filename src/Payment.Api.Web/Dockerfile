#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Payment.Api.Web/Payment.Api.Web.csproj", "src/Payment.Api.Web/"]
RUN dotnet restore "src/Payment.Api.Web/Payment.Api.Web.csproj"
COPY . .
WORKDIR "/src/src/Payment.Api.Web"
RUN dotnet build "Payment.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.Api.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Api.Web.dll"]