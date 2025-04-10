FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["IDP/IDP.csproj", "IDP/"]
COPY ["IDP.Infrastructure/IDP.Infrastructure.csproj", "IDP.Infrastructure/"]
COPY ["IDP.Presentation/IDP.Presentation.csproj", "IDP.Presentation/"]
RUN dotnet restore "IDP/IDP.csproj"
COPY . .
WORKDIR "/src/IDP"
RUN dotnet build "IDP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IDP.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDP.dll"]