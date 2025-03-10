# Ambev Developer Evaluation - Sales API 🚀

This project is a Sales Management API, developed as part of the Ambev Developer Evaluation process. It provides a robust CRUD (Create, Read, Update, Delete) for managing sales and integrates best practices such as CQRS, dependency injection, validation, and unit testing.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Environment Variables](#environment-variables)
- [Running the Application](#running-the-application)
- [Authentication](#authentication)
- [API Endpoints](#api-endpoints)
- [Running Tests](#running-tests)
- [Contributing](#contributing)
- [License](#license)

## Features
- ✅ CRUD operations for sales (create, read, update, delete)
- ✅ Validation using FluentValidation
- ✅ CQRS pattern with MediatR for better separation of concerns
- ✅ Unit testing with xUnit, Moq, and FluentAssertions
- ✅ AutoMapper for DTO-to-domain mapping
- ✅ Authentication using JWT tokens
- ✅ Entity Framework Core (EF Core) with PostgreSQL
- ✅ Logging and error handling

## Technologies Used
- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core (EF Core)
- PostgreSQL (as the database)
- FluentValidation
- MediatR
- AutoMapper
- xUnit & Moq (for unit testing)
- JWT (JSON Web Tokens) for authentication
- Docker (optional for containerized deployment)

## Prerequisites
Ensure you have the following installed before setting up the project:
- .NET 8 SDK
- PostgreSQL (or use Docker)
- Docker (optional for containerized setup)

## Installation

1. Clone the repository
```sh
git clone https://github.com/your-repo/ambev-sales-api.git
cd ambev-sales-api
```

2. Configure the database connection
Update the appsettings.json or set environment variables:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=sales_db;Username=your_user;Password=your_password"
}
```

Alternatively, use Docker Compose:
```sh
docker-compose up -d
```

3. Apply database migrations
```sh
dotnet ef database update
```

4. Run the application
```sh
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

## Environment Variables

| Variable | Description |
|----------|-------------|
| Jwt:SecretKey | Secret key for JWT token encryption |
| ConnectionStrings:DefaultConnection | PostgreSQL connection string |
| Logging:LogLevel:Default | Logging level configuration |

## Authentication
This API uses JWT-based authentication. To access protected endpoints, you must obtain a token via login.

1. Authenticate Request
```http
POST /api/auth
```
Body:
```json
{
  "email": "admin@example.com",
  "password": "Pass@word1"
}
```

Response:
```json
{
  "success": true,
  "message": "User authenticated successfully",
  "data": {
    "token": "your_jwt_token_here",
    "role": "Admin"
  }
}
```

2. Use the Token
Include the token in the Authorization header of requests:
```http
Authorization: Bearer your_jwt_token_here
```

## API Endpoints

### Sales
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/sales | Create a new sale |
| GET | /api/sales/{id} | Retrieve a sale by ID |
| PUT | /api/sales | Update an existing sale |
| DELETE | /api/sales/{id} | Delete a sale |

For a full list of endpoints and request payloads, check Swagger UI:
```
https://localhost:44312/swagger
```

## Running Tests
The project includes unit tests for core business logic.

Run all tests
```sh
dotnet test
```

Run specific tests
```sh
dotnet test --filter "FullyQualifiedName~CreateSaleHandlerTests"
```

## Contributing
Contributions are welcome! Follow these steps:
1. Fork the repository
2. Create a new branch (feature/new-feature)
3. Commit changes using Conventional Commits
4. Push to the branch
5. Open a Pull Request (PR)

## License
This project is licensed under the MIT License.

## Author
👨‍💻 Felipe Gonçalves Ferreira
📧 Contact: contato@felipegf.com.br
📌 GitHub | LinkedIn