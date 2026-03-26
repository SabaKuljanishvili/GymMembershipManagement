# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ვაკოპირებთ .csproj ფაილებს სტრუქტურის მიხედვით
COPY ["GymMembershipManagement.API/GymMembershipManagement.API.csproj", "GymMembershipManagement.API/"]
COPY ["GymMembershipManagement.DATA/GymMembershipManagement.DATA.csproj", "GymMembershipManagement.DATA/"]
COPY ["GymMembershipManagement.SERVICE/GymMembershipManagement.SERVICE.csproj", "GymMembershipManagement.SERVICE/"]

# ვაკეთებთ Restore-ს
RUN dotnet restore "GymMembershipManagement.API/GymMembershipManagement.API.csproj"

# ვაკოპირებთ მთლიან კოდს
COPY . .
WORKDIR "/src/GymMembershipManagement.API"

# ვაშენებთ პროექტს
RUN dotnet publish "GymMembershipManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GymMembershipManagement.API.dll"]