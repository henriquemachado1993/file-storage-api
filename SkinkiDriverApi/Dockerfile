FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 27016

ENV ASPNETCORE_URLS=http://+:27016

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SkinkiDriverApi.csproj", "./"]
RUN dotnet restore "SkinkiDriverApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SkinkiDriverApi.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "SkinkiDriverApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkinkiDriverApi.dll"]
