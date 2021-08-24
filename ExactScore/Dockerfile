#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ExactScore/ExactScore.csproj", "ExactScore/"]
RUN dotnet restore "ExactScore/ExactScore.csproj"
COPY . .
WORKDIR "/src/ExactScore"
RUN dotnet build "ExactScore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExactScore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExactScore.dll"]