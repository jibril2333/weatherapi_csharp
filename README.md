# Weather and Chat API Service

A RESTful API service built with ASP.NET Core that provides real-time weather information and chat functionality using OpenAI's GPT model.

## Features

- Real-time weather data retrieval for cities
- City coordinates lookup
- Daily temperature forecasts (max/min)
- AI-powered chat functionality
- Swagger/OpenAPI documentation
- Clean architecture with separation of concerns
- Dependency injection for better testability and maintainability

## Technology Stack

- ASP.NET Core
- C# 12
- Open-Meteo API
- OpenAI API
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
- OpenAI API Key

### Installation

1. Clone the repository

```bash
git clone https://github.com/jibril2333/weatherapi_csharp.git
```

2. Navigate to the project directory

```bash
cd restapi_c
```

3. Create a `.env` file in the project root with your OpenAI API key:

```
OPENAI_API_KEY=your-api-key-here
```

4. Restore dependencies

```bash
dotnet restore
```

5. Run the application

```bash
dotnet run
```

The API will be available at `http://localhost:5038`

### API Documentation

Once the application is running, you can access the Swagger documentation at:

- Swagger UI: `http://localhost:5038/swagger`
- OpenAPI JSON: `http://localhost:5038/swagger/v1/swagger.json`

### API Endpoints

#### Weather Endpoints
- `GET /weather/{cityName}` - Get weather forecast for a specific city
  - Example: `GET /weather/beijing`
  - Returns daily temperature forecasts (max/min) for the next 7 days

#### Chat Endpoints
- `POST /chat` - Send a message to the AI chat service
  - Request body:
    ```json
    {
        "message": "Your message here"
    }
    ```
  - Returns AI response in JSON format

## Development

### Adding New Features

1. Create appropriate models in the `Models` directory
2. Define interfaces in the `Interfaces` directory
3. Implement services in the `Services` directory
4. Create controllers in the `Controllers` directory
5. Register new services in `Program.cs`

### Environment Variables

The application uses the following environment variables:
- `OPENAI_API_KEY`: Your OpenAI API key
- `CERT_PATH`: (Optional) Path to SSL certificate for HTTPS

## License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.
