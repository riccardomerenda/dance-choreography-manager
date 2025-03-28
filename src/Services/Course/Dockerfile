FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/Course/DanceChoreographyManager.Services.Course.csproj", "src/Services/Course/"]
COPY ["src/ServiceDefaults/DanceChoreographyManager.ServiceDefaults.csproj", "src/ServiceDefaults/"]
COPY ["src/Shared/Models/DanceChoreographyManager.Shared.Models.csproj", "src/Shared/Models/"]
COPY ["src/Shared/DTOs/DanceChoreographyManager.Shared.DTOs.csproj", "src/Shared/DTOs/"]
COPY ["src/Shared/Utilities/DanceChoreographyManager.Shared.Utilities.csproj", "src/Shared/Utilities/"]
RUN dotnet restore "src/Services/Course/DanceChoreographyManager.Services.Course.csproj"
COPY . .
WORKDIR "/src/src/Services/Course"
RUN dotnet build "DanceChoreographyManager.Services.Course.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DanceChoreographyManager.Services.Course.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DanceChoreographyManager.Services.Course.dll"]