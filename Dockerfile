FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Backend.Api/Backend.Api.csproj", "Backend.Api/"]
COPY ["Backend.BL/Backend.BL.csproj", "Backend.BL/"]
COPY ["Backend.Common/Backend.Common.csproj", "Backend.Common/"]
COPY ["Backend.DAL/Backend.DAL.csproj", "Backend.DAL/"]
COPY ["Common/Common.csproj", "Common/"]
#COPY ["Auth.DAL/Auth.DAL.csproj", "Auth.DAL/"]
RUN dotnet restore "Backend.Api/Backend.Api.csproj"
COPY . .
WORKDIR "/src/Backend.Api"
RUN dotnet build "Backend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Api.dll"]
