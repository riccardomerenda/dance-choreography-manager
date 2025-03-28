FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Dancer/DanceChoreographyManager.Services.Dancer.csproj", "src/Services/Dancer/"]
COPY ["src/ServiceDefaults/DanceChoreographyManager.ServiceDefaults.csproj", "src/ServiceDefaults/"]
COPY ["src/Shared/Models/DanceChoreographyManager.Shared.Models.csproj", "src/Shared/Models/"]
COPY ["src/Shared/DTOs/DanceChoreographyManager.Shared.DTOs.csproj", "src/Shared/DTOs/"]
COPY ["src/Shared/Utilities/DanceChoreographyManager.Shared.Utilities.csproj", "src/Shared/Utilities/"]
RUN dotnet restore "src/Services/Dancer/DanceChoreographyManager.Services.Dancer.csproj"
COPY . .
WORKDIR "/src/src/Services/Dancer"
RUN dotnet build "DanceChoreographyManager.Services.Dancer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DanceChoreographyManager.Services.Dancer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DanceChoreographyManager.Services.Dancer.dll"]