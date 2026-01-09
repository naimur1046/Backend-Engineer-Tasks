# Backend Engineer Tasks - ASP.NET Core Implementation

A REST API service built with ASP.NET Core that provides three core functionalities: date calculations, number-to-words conversion, and weather statistics for Dhaka.

## Table of Contents

- [Overview](#overview)
- [Quick Start](#quick-start)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Testing](#testing)
- [Docker](#docker)
- [Project Structure](#project-structure)
- [Technical Details](#technical-details)

## Overview

This project implements three REST API endpoints as part of the Backend Engineer assessment:

1. **Number of Days**: Calculate the number of days between two dates without using standard date libraries
2. **Number to Words**: Convert numbers to their English word representation
3. **Temperature Stats**: Retrieve and analyze temperature data for Dhaka

## Quick Start

**Get up and running in 3 steps:**

```bash
# 1. Clone and navigate to the project
git clone https://github.com/naimur1046/Backend-Engineer-Tasks.git
cd Task

# 2. Restore dependencies and build
dotnet build

cd Task
# 3. Run the application
dotnet run
```

**Then open your browser to:** `https://localhost:5001/swagger`

That's it! You can now explore and test all API endpoints through Swagger UI.

## Features

- ✅ Used Asp.NET in the implementation for date calculations (no date libraries)
- ✅ Comprehensive number-to-words conversion (0-999.99)
- ✅ Integration with Open-Meteo API for real weather data
- ✅ JSON input/output for all endpoints
- ✅ Swagger/OpenAPI documentation
- ✅ Comprehensive unit test coverage
- ✅ Docker containerization support
- ✅ RESTful API design with ASP.NET Core

## Prerequisites

To build and run this application, you need:

- **.NET SDK**: Version 9.0
- **IDE (Optional)**: Visual Studio 2022, Visual Studio Code, or JetBrains Rider
- **Docker**: (Optional) For containerized deployment

### Installing Prerequisites

**On macOS:**

```bash
# Install .NET SDK
brew install --cask dotnet-sdk

# Or download from Microsoft
# https://dotnet.microsoft.com/download
```

**On Ubuntu/Debian:**

```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

**On Windows:**

- Download and install .NET SDK from [Microsoft](https://dotnet.microsoft.com/download)
- (Optional) Install [Visual Studio 2022](https://visualstudio.microsoft.com/) with ASP.NET workload

**Verify Installation:**

```bash
dotnet --version
```

## Installation

1. **Clone the repository:**

```bash
git clone https://github.com/naimur1046/Backend-Engineer-Tasks.git
cd Task
```

2. **Restore dependencies:**

```bash
dotnet restore
```

3. **Build the project:**

```bash
dotnet build
```

This will download all NuGet packages and compile the source code.

## Running the Application

### Using .NET CLI

Start the application with:

```bash
dotnet run
```

Or for a specific project:

```bash
dotnet run --project Task/Task.csproj
```

The server will start on `http://localhost:5000` and `https://localhost:5001` by default.

### Using Visual Studio

1. Open the solution file (`.sln`) in Visual Studio
2. Press `F5` or click the "Run" button
3. The application will start with the default browser opening to Swagger UI

### Accessing Swagger UI

Once the application is running, navigate to:

```
https://localhost:5001/swagger
```

Or:

```
http://localhost:5000/swagger
```

Swagger provides an interactive API documentation interface where you can:

- View all available endpoints
- See request/response schemas
- Test API endpoints directly from the browser
- Download OpenAPI specification

### Using a published build

1. Publish the application:

```bash
dotnet publish -c Release -o ./publish
```

2. Run the published application:

```bash
cd publish
dotnet YourProjectName.dll
```

## API Documentation

> **💡 Tip:** Once the application is running, visit `/swagger` for interactive API documentation and testing.

All endpoints accept and return JSON data.

### Using Swagger UI

Swagger UI provides an interactive interface to explore and test the API:

1. **Start the application** (see [Running the Application](#running-the-application))
2. **Navigate to Swagger UI:**
   - Development: `https://localhost:5001/swagger`
   - Or: `http://localhost:5000/swagger`
3. **Explore the API:**
   - View all available endpoints organized by controllers
   - See detailed request/response schemas
   - Test endpoints directly from the browser
4. **Test an endpoint:**
   - Click on an endpoint to expand it
   - Click "Try it out"
   - Fill in the request body
   - Click "Execute"
   - View the response

### API Endpoints Overview

### 1. Calculate Days Between Dates

Calculate the number of days between two dates.

**Endpoint:** `POST /api/days`

**Request Body:**

```json
{
  "startDate": "2024-01-01",
  "endDate": "2024-12-31"
}
```

**Date Format:** `YYYY-MM-DD`

**Response:**

```json
{
  "days": 366
}
```

**Example using curl:**

```bash
curl -X POST http://localhost:8080/api/days \
  -H "Content-Type: application/json" \
  -d '{"startDate": "2024-01-01", "endDate": "2024-12-31"}'
```

---

### 2. Number to Words

Convert a number to its English word representation.

**Endpoint:** `POST /api/number-to-words`

**Request Body:**

```json
{
  "number": 36.4
}
```

**Constraints:**

- Number range: 0 ≤ n < 1,000
- Maximum 2 decimal places
- Output in lowercase with single spaces between words
- No hyphens or "and"

**Response:**

```json
{
  "words": "thirty six point four zero"
}
```

**Examples:**

```bash
# Example 1: Whole number
curl -X POST http://localhost:8080/api/number-to-words \
  -H "Content-Type: application/json" \
  -d '{"number": 105}'

# Response: {"words": "one hundred five"}

# Example 2: Decimal number
curl -X POST http://localhost:8080/api/number-to-words \
  -H "Content-Type: application/json" \
  -d '{"number": 36.40}'

# Response: {"words": "thirty six point four zero"}
```

---

### 3. Temperature Statistics for Dhaka

Retrieve average, minimum, and maximum temperatures for Dhaka over a date range.

**Endpoint:** `POST /api/temperature/dhaka`

**Request Body:**

```json
{
  "startDate": "2024-01-01",
  "endDate": "2024-01-07"
}
```

**Date Format:** `YYYY-MM-DD`

**Response:**

```json
{
  "min": -5.4,
  "max": 1.3,
  "average": -1.44,
  "minText": "minus five point four",
  "maxText": "positive one point three",
  "averageText": "minus one point four four"
}
```

**Example using curl:**

```bash
curl -X 'POST' \
  'http://localhost:5000/api/Temperature/temperature-stats-for-dhaka ' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "startDate": "2026-01-01",
  "endDate": "2026-01-07"
  }'
```

**Notes:**

- Uses Open-Meteo API for weather data
- Dhaka coordinates: Latitude 23.8103, Longitude 90.4125
- Temperature values are in Celsius
- Text representation includes "positive" or "minus" prefix
- Properly handles floating-point calculations

## Configuration

The application uses `appsettings.json` for configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "OpenMeteo": {
    "BaseUrl": "https://api.open-meteo.com/v1",
    "Latitude": 23.8103,
    "Longitude": 90.4125
  },
  "Swagger": {
    "Title": "Backend Tasks API",
    "Version": "v1",
    "Description": "REST API for date calculations, number conversion, and weather stats"
  }
}
```

### Environment-Specific Configuration

For production, create `appsettings.Production.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

### Swagger Configuration in Program.cs

Swagger is configured in `Program.cs`:

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eskimi Backend Tasks API",
        Version = "v1",
        Description = "REST API for the Eskimi Backend Engineer assessment tasks"
    });
});

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger";
});
```

## Testing

The project includes comprehensive unit tests using xUnit, NUnit, or MSTest.

### Run all tests:

```bash
dotnet test
```

### Run tests with detailed output:

```bash
dotnet test --verbosity normal
```

### Run tests with code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run specific test project:

```bash
dotnet test tests/Task.Test/Task.Test.csproj
```

### Run specific test class:

```bash
dotnet test --filter "FullyQualifiedName~DaysCalculatorTests"
```

### Generate coverage report (using ReportGenerator):

```bash
# Install ReportGenerator tool
dotnet tool install -g dotnet-reportgenerator-globaltool

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

### Test output:

Tests are executed automatically during the build process and provide detailed feedback on:

- Date calculation accuracy (including leap years)
- Number conversion edge cases
- Temperature data processing
- API endpoint responses
- Error handling and validation

## Docker

### Dockerfile Example

Here's a multi-stage Dockerfile for ASP.NET Core:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Task/Task.csproj", "Task/Task/"]
RUN dotnet restore "Task/Task.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Task"
RUN dotnet build "Task.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Task.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Task.dll"]
```

### Build Docker Image

```bash
docker build -t backend-tasks .
```

### Run Docker Container

```bash
# Run on port 8080
docker run -d -p 8080:80 --name api backend-tasks

# With environment variables
docker run -d -p 8080:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  --name api \
  eskimi-backend-tasks
```

The application will be accessible at `http://localhost:8080`.

**Access Swagger in Docker:**

```
http://localhost:8080/swagger
```

### Docker Compose

Create a `docker-compose.yml` file:

```yaml
version: "3.8"

services:
  api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:80
    restart: unless-stopped
```

Run with Docker Compose:

```bash
docker-compose up -d
```

Stop the containers:

```bash
docker-compose down
```

## Project Structure

```
.
├── Task/
│   └── Task/
│       ├── Controllers/            # API Controllers
│       │   ├── DaysController.cs
│       │   ├── NumberToWordsController.cs
│       │   └── TemperatureController.cs
│       ├── Services/               # Business logic
│       │   ├── DaysCalculator.cs
│       │   ├── NumberToWordsService.cs
│       │   └── TemperatureService.cs
│       ├── Models/                 # DTOs and data models
│       │   ├── DaysRequest.cs
│       │   ├── NumberToWordsRequest.cs
│       │   └── TemperatureRequest.cs
│       ├── Program.cs              # Application entry point
│       ├── appsettings.json        # Configuration
│       └── YourProjectName.csproj  # Project file
├──     Task.Tests/
│       ├── DaysCalculatorTests.cs
│       ├── NumberToWordsTests.cs
│       ├── TemperatureServiceTests.cs
│       └── YourProjectName.Tests.csproj
├── Task.sln             # Solution file
├── Dockerfile                      # Docker configuration
└── README.md                       # This file
```

## NuGet Packages

The project uses the following key NuGet packages:

### Runtime Dependencies

```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" /> <!-- If using Newtonsoft instead of System.Text.Json -->
```

### Test Dependencies

```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
<PackageReference Include="xunit" Version="2.6.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.4" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="coverlet.collector" Version="6.0.0" />
```

### Install a package

```bash
dotnet add package Swashbuckle.AspNetCore
```

## Technical Details

### Date Calculation Algorithm

The date calculation implementation:

- Does not use any standard date/time libraries
- Handles leap years correctly (divisible by 4, except centuries unless divisible by 400)
- Supports dates from year 1 onwards
- Uses Zeller's congruence or similar algorithms for day-of-week calculations
- Accounts for varying month lengths

### Number to Words Conversion

The conversion logic:

- Handles hundreds, tens, and units places
- Processes decimal points correctly
- Outputs lowercase text with single spaces
- No hyphens (e.g., "twenty one" not "twenty-one")
- No "and" conjunction (e.g., "one hundred five" not "one hundred and five")

### Temperature Service

The weather service:

- Integrates with Open-Meteo API (https://api.open-meteo.com)
- Uses HttpClient for API requests
- Parses JSON responses using System.Text.Json or Newtonsoft.Json
- Calculates statistics: min, max, average
- Reuses number-to-words logic for text representation
- Handles positive/negative temperatures appropriately
- Implements async/await pattern for non-blocking API calls

### Technology Stack

- **Framework:** ASP.NET Core 6.0/7.0/8.0
- **Language:** C# 10/11/12
- **API Documentation:** Swagger/Swashbuckle
- **JSON Processing:** System.Text.Json
- **Testing:** xUnit/NUnit/MSTest
- **HTTP Client:** HttpClient
- **Dependency Injection:** Built-in ASP.NET Core DI
- **Build Tool:** .NET CLI / MSBuild

## Error Handling

The API provides appropriate error responses for:

- Invalid date formats
- Invalid date ranges (end date before start date)
- Numbers outside the valid range (0-999.99)
- Numbers with more than 2 decimal places
- Network errors when fetching weather data
- Invalid JSON payloads

**Example error response:**

```json
{
  "error": "Invalid date format. Expected YYYY-MM-DD",
  "status": 400
}
```

## Troubleshooting

### Port Already in Use

If you get an error that the port is already in use:

```bash
# Change the port in Properties/launchSettings.json
# Or run with a custom port:
dotnet run --urls "http://localhost:5050"
```

### HTTPS Certificate Issues

For development HTTPS certificate issues:

```bash
# Trust the development certificate
dotnet dev-certs https --trust
```

### Swagger Not Loading

1. Ensure Swagger is enabled in `Program.cs`
2. Check that you're accessing the correct URL: `/swagger` not `/swagger/index.html`
3. Verify the app is running in Development environment

### Cannot Connect to Open-Meteo API

- Check your internet connection
- Verify the API endpoint is accessible
- Review logs for detailed error messages
- Check if you need to configure a proxy

## Development Notes

- All input and output use JSON format as specified
- Code follows C# and ASP.NET Core best practices
- Comprehensive error handling and validation with model binding
- Clean separation of concerns (Controllers, Services, Models)
- Dependency Injection for loose coupling
- Async/await pattern for asynchronous operations
- Strong typing with C# records and classes
- Swagger/OpenAPI for interactive API documentation

## Contact

For questions or feedback about this implementation, please contact
naimurrahman1046@gmail.com

---
