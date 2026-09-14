# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first (leverages Docker layer cache when only source files change)
COPY AlgoMotion.csproj Directory.Build.props nuget.config ./
RUN dotnet restore "AlgoMotion.csproj"

# Copy the rest of the source and publish
COPY . .
RUN dotnet publish "AlgoMotion.csproj" -c Release -o /app/publish

# ---- Runtime stage (static file server) ----
FROM nginx:1.27-alpine AS final
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
