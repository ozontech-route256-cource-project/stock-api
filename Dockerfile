
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./src/OzonEdu.StockApi/OzonEdu.StockApi.csproj", "./OzonEdu.StockApi/"]
RUN dotnet restore "./OzonEdu.StockApi/OzonEdu.StockApi.csproj"
COPY ./src .
WORKDIR "/src"
RUN dotnet build "./OzonEdu.StockApi/OzonEdu.StockApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./OzonEdu.StockApi/OzonEdu.StockApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OzonEdu.StockApi.dll"]