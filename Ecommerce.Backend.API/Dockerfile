#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Ecommerce.Backend.API/Ecommerce.Backend.API.csproj", "Ecommerce.Backend.API/"]
COPY ["Ecommerce.Backend.Services/Ecommerce.Backend.Services.csproj", "Ecommerce.Backend.Services/"]
COPY ["Ecommerce.Backend.Common/Ecommerce.Backend.Common.csproj", "Ecommerce.Backend.Common/"]
COPY ["Ecommerce.Backend.Entities/Ecommerce.Backend.Entities.csproj", "Ecommerce.Backend.Entities/"]
COPY ["Ecommerce.PaymentGateway.SSLCommerz/Ecommerce.PaymentGateway.SSLCommerz.csproj", "Ecommerce.PaymentGateway.SSLCommerz/"]
RUN dotnet restore "Ecommerce.Backend.API/Ecommerce.Backend.API.csproj"
COPY . .
WORKDIR "/src/Ecommerce.Backend.API"
RUN dotnet build "Ecommerce.Backend.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Backend.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Backend.API.dll"]