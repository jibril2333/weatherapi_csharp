# Weather API Service

A RESTful API service built with ASP.NET Core that provides real-time weather information for cities using the Open-Meteo API.

## Features

- Real-time weather data retrieval for cities
- City coordinates lookup
- Daily temperature forecasts (max/min)
- Swagger/OpenAPI documentation
- Clean architecture with separation of concerns
- Dependency injection for better testability and maintainability

## Technology Stack

- ASP.NET Core
- C# 12
- Open-Meteo API
- Swagger/OpenAPI
- HTTP Client Factory
- Dependency Injection

## Project Structure

```
├── Controllers/     # API endpoints
├── Models/         # Data models
├── Services/       # Business logic
├── Interfaces/     # Service contracts
└── DTOs/          # Data Transfer Objects
```

## Getting Started

### Prerequisites

- .NET 9.0 or later
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone [repository-url]
```

2. Navigate to the project directory
```bash
cd restapi_c
```

3. Restore dependencies
```bash
dotnet restore
```

4. Run the application
```bash
dotnet run
```

The API will be available at `http://localhost:5038`

### API Documentation

Once the application is running, you can access the Swagger documentation at:
- Swagger UI: `http://localhost:5038/swagger`
- OpenAPI JSON: `http://localhost:5038/swagger/v1/swagger.json`

### API Endpoints

- `GET /weather/{cityName}` - Get weather forecast for a specific city
  - Example: `GET /weather/beijing`
  - Returns daily temperature forecasts (max/min) for the next 7 days

## Development

### Adding New Features

1. Create appropriate models in the `Models` directory
2. Define interfaces in the `Interfaces` directory
3. Implement services in the `Services` directory
4. Create controllers in the `Controllers` directory
5. Register new services in `Program.cs`

## License

This project is licensed under the MIT License - see the LICENSE file for details. 