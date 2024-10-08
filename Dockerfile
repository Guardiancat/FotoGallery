# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official ASP.NET Core build image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PhotoGallery.csproj", "./"]
RUN dotnet restore "PhotoGallery.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PhotoGallery.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhotoGallery.csproj" -c Release -o /app/publish

# Copy the build app to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhotoGallery.dll"]