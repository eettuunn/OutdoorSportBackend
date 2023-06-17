FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Auth.Api/Auth.Api.csproj", "Auth.Api/"]
COPY ["Auth.BL/Auth.BL.csproj", "Auth.BL/"]
COPY ["Auth.Common/Auth.Common.csproj", "Auth.Common/"]
COPY ["Auth.DAL/Auth.DAL.csproj", "Auth.DAL/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "Auth.Api/Auth.Api.csproj"
COPY . .
WORKDIR "/src/Auth.Api"
RUN dotnet build "Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Api.dll"]


#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["../../OutdoorSportBackend/Auth.Api/Auth.Api.csproj", "Auth.Api/"]
#COPY ["../../OutdoorSportBackend/Auth.BL/Auth.BL.csproj", "Auth.BL/"]
#COPY ["../../OutdoorSportBackend/Auth.Common/Auth.Common.csproj", "Auth.Common/"]
#COPY ["../../OutdoorSportBackend/Auth.DAL/Auth.DAL.csproj", "Auth.DAL/"]
#COPY ["../../OutdoorSportBackend/Common/Common.csproj", "Common/"]
#RUN dotnet restore "Auth.Api/Auth.Api.csproj"
#WORKDIR "/src/Auth.Api"
#COPY . .
#RUN dotnet build -p:TargetFramework=net7.0 "Auth.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Auth.Api.dll"]