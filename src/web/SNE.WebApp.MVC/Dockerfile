#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/web/SNE.WebApp.MVC/SNE.WebApp.MVC.csproj", "src/web/SNE.WebApp.MVC/"]
RUN dotnet restore "src/web/SNE.WebApp.MVC/SNE.WebApp.MVC.csproj"
COPY . .
WORKDIR "/src/src/web/SNE.WebApp.MVC"
RUN dotnet build "SNE.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SNE.WebApp.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SNE.WebApp.MVC.dll"]