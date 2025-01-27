FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["RestaurantManagement/RestaurantManagement.csproj", "RestaurantManagement/"]
RUN dotnet restore "RestaurantManagement/RestaurantManagement.csproj"
COPY . .
WORKDIR "/src/RestaurantManagement"
RUN dotnet build "RestaurantManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantManagement.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet RestaurantManagement.dll
