#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 11111
EXPOSE 30000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Nuget.Config", "."]
COPY ["VeeFriends.Silo/VeeFriends.Silo.csproj", "VeeFriends.Silo/"]
COPY ["VeeFriends.Data/VeeFriends.Data.csproj", "VeeFriends.Data/"]
COPY ["VeeFriends.Abstractions/VeeFriends.Abstractions.csproj", "VeeFriends.Abstractions/"]
COPY ["VeeFriends.Models/VeeFriends.Models.csproj", "VeeFriends.Models/"]
COPY ["VeeFriends.Utilities/VeeFriends.Utilities.csproj", "VeeFriends.Utilities/"]
COPY ["VeeFriends.Blockchain.SmartContracts/VeeFriends.Blockchain.SmartContracts.csproj", "VeeFriends.Blockchain.SmartContracts/"]
COPY ["VeeFriends.Http/VeeFriends.Http.csproj", "VeeFriends.Http/"]
COPY ["VeeFriends.Shared/VeeFriends.Shared.csproj", "VeeFriends.Shared/"]
COPY ["VeeFriends.GraphQL/VeeFriends.GraphQL.csproj", "VeeFriends.GraphQL/"]
RUN dotnet restore "VeeFriends.Silo/VeeFriends.Silo.csproj"
COPY . .
WORKDIR "/src/VeeFriends.Silo"
RUN dotnet build "VeeFriends.Silo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VeeFriends.Silo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VeeFriends.Silo.dll"]