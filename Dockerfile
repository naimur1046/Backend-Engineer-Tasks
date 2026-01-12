# Stage 1: Build and Test
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Task/Task/Task.csproj", "Task/Task/"]
COPY ["Task/Task.Test/Task.Test.csproj", "Task/Task.Test/"]

# Restore dependencies
RUN dotnet restore "Task/Task/Task.csproj"
RUN dotnet restore "Task/Task.Test/Task.Test.csproj"

# Copy all source files
COPY . .

# Run tests
WORKDIR "/src/Task/Task.Test"
RUN dotnet test --configuration Release --no-restore --verbosity normal

# Build the application
WORKDIR "/src/Task/Task"
RUN dotnet build "Task.csproj" -c Release -o /app/build --no-restore

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Task.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create a non-root user for security
RUN addgroup --system --gid 1000 appuser && \
     adduser --system --uid 1000 --ingroup appuser --shell /bin/sh appuser

# Copy published files
COPY --from=publish /app/publish .

# Create logs directory and set permissions
RUN mkdir -p /app/__logs && chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
     CMD curl --fail http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "Task.dll"]