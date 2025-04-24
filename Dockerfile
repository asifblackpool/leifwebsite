ARG builder_image=mcr.microsoft.com/dotnet/sdk:7.0
FROM ${builder_image} AS build

WORKDIR /src
# The below allows layer caching for the restore.
COPY RazorPageLeifWebsite/RazorPageLeifWebsite.csproj .
RUN dotnet restore

COPY RazorPageLeifWebsite ./
RUN dotnet publish $csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
ENV ASPNETCORE_URLS=http://*:3001
WORKDIR /app
COPY --from=build /app/publish .

# Copy manifest.json to the root of the container
COPY manifest.json ./ 
EXPOSE 3001
ENTRYPOINT ["dotnet", "RazorPageLeifWebsite.dll"]
