# Ambev Developer Evaluation - Sales API 🚀

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![Test Coverage](https://img.shields.io/badge/coverage-85%25-green)
![Version](https://img.shields.io/badge/version-1.1.0-blue)
![License](https://img.shields.io/badge/license-MIT-yellow)

This project is a **Sales Management API**, developed as part of the Ambev Developer Evaluation process. It provides a **robust CRUD (Create, Read, Update, Delete)** for managing sales while integrating best practices such as **CQRS, dependency injection, validation, unit testing, and event sourcing**.

## 📋 Table of Contents

- [✨ Features](#-features)
- [🚀 Quick Start](#-quick-start)
- [🔧 Technologies Used](#-technologies-used)
- [📝 Prerequisites](#-prerequisites)
- [⚙️ Installation](#️-installation)
- [🔐 Environment Variables](#-environment-variables)
- [🏃‍♂️ Running the Application](#️-running-the-application)
- [🔒 Authentication](#-authentication)
- [🔌 API Endpoints](#-api-endpoints)
- [📊 Architecture](#-architecture)
- [📜 Event Sourcing & Logging](#-event-sourcing--logging)
- [🧪 Running Tests](#-running-tests)
- [❓ Troubleshooting](#-troubleshooting)
- [🛣️ Roadmap](#️-roadmap)
- [🤝 Contributing](#-contributing)
- [📜 License](#-license)
- [👤 Author](#-author)

## ✨ Features

- ✅ **CRUD operations** for sales (create, read, update, delete)
- ✅ **CQRS pattern** with MediatR for better separation of concerns
- ✅ **Event Sourcing** with MongoDB for auditability and tracking
- ✅ **MongoDB-based Log Storage** for scalable logging
- ✅ **FluentValidation** for input validation
- ✅ **AutoMapper** for DTO-to-domain mapping
- ✅ **JWT Authentication** for secure API access
- ✅ **Entity Framework Core** (EF Core) with PostgreSQL
- ✅ **API versioning** for backward compatibility
- ✅ **Comprehensive exception handling**
- ✅ **Performance monitoring**
- ✅ **Unit testing** with xUnit, Moq, and FluentAssertions
- ✅ **Docker support** for seamless deployment

## 🚀 Quick Start

```bash
# Clone the repository
git clone https://github.com/felipegf/Ambev.Omnia.Eval.git

# Navigate to the project directory
cd Ambev.Omnia.Eval

# Start with Docker (recommended)
docker-compose up -d

# The API is now running at https://localhost:8080
# Swagger UI available at https://localhost:8080/swagger
```

## 🔧 Technologies Used

* **.NET 8**: ASP.NET Core Web API with modern features
* **Entity Framework Core**: ORM for PostgreSQL database operations
* **MongoDB**: Used for event storage and log storage
* **Serilog**: Integrated with MongoDB as a logging sink
* **FluentValidation**: Advanced input validation
* **MediatR**: Implements the CQRS pattern
* **AutoMapper**: Simplifies object-to-object mapping
* **xUnit & Moq**: Unit testing framework
* **JWT Authentication**: Secure access to protected endpoints
* **Docker**: Containerized environment for seamless deployment
* **Swagger/OpenAPI**: API documentation

## 📝 Prerequisites

Ensure you have the following installed:
* .NET 8 SDK
* PostgreSQL (or use Docker)
* MongoDB (or use Docker)
* Docker (optional)
* Git

## ⚙️ Installation

### Option 1: Using Docker (Recommended)

```bash
# Clone the repository
git clone https://github.com/felipegf/Ambev.Omnia.Eval.git
cd Ambev.Omnia.Eval

# Start the application and database
docker-compose up -d
```

### Option 2: Manual Setup

```bash
# Clone the repository
git clone https://github.com/felipegf/Ambev.Omnia.Eval.git
cd Ambev.Omnia.Eval

# Configure the database connection in appsettings.json

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

## 🔐 Environment Variables

| Variable | Description | Default | Required |
|----------|-------------|---------|----------|
| `Jwt:SecretKey` | Secret key for JWT token encryption | - | Yes |
| `MongoDB:ConnectionString` | Connection string for MongoDB | - | Yes |
| `MongoDB:DatabaseName` | Name of MongoDB database | events_db | No |
| `Jwt:Issuer` | Token issuer | api.ambev.com | No |
| `Jwt:Audience` | Token audience | ambev.users | No |
| `Jwt:ExpiryMinutes` | Token expiry in minutes | 60 | No |
| `ConnectionStrings:DefaultConnection` | PostgreSQL connection string | - | Yes |

## 🏃‍♂️ Running the Application

After installation, the API will be available at:
- API Endpoint: `https://localhost:8080`
- Swagger Documentation: `https://localhost:8080/swagger`

## 🔒 Authentication

The API uses JWT Bearer token authentication:
1. Obtain a token via the `/api/auth/login` endpoint
2. Include the token in the Authorization header: `Bearer {token}`

## 🔌 API Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| GET | `/api/v1/sales` | List all sales | Required |
| GET | `/api/v1/sales/{id}` | Get sale by ID | Required |
| POST | `/api/v1/sales` | Create new sale | Required |
| PUT | `/api/v1/sales/{id}` | Update existing sale | Required |
| DELETE | `/api/v1/sales/{id}` | Delete sale | Required |
| POST | `/api/auth/login` | Authenticate user | Not Required |

## 📊 Architecture

This project implements **CQRS and Event Sourcing** for scalability and maintainability.

```
┌───────────────┐     ┌───────────────┐     ┌───────────────┐
│  Controllers  │────▶│    MediatR    │────▶│   Commands    │
└───────────────┘     └───────────────┘     └───────┬───────┘
                                                    │
┌───────────────┐     ┌───────────────┐     ┌───────▼───────┐
│   Database    │◀────│ Repositories  │◀────│   Handlers    │
└───────────────┘     └───────────────┘     └───────────────┘
```

## 📜 Event Sourcing & Logging

This API utilizes **event sourcing** and **centralized logging** to enhance traceability:

1. **MongoDB Event Store**
   * Stores domain events related to sales transactions.
   * Provides a historical timeline of all modifications.

2. **MongoDB Log Storage**
   * Uses **Serilog MongoDB Sink** for scalable log storage.
   * Supports structured and queryable logs.

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage report
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run specific test category
dotnet test --filter "Category=Integration"

# Run specific test class
dotnet test --filter "FullyQualifiedName~CreateSaleHandlerTests"
```

## ❓ Troubleshooting

### Common Issues & Fixes

1. **Database connection failures**
   * Ensure PostgreSQL and MongoDB are running
   * Verify connection strings in `appsettings.json`
   * Check network connectivity

2. **JWT Authentication issues**
   * Ensure token is valid and not expired
   * Verify secret key configuration
   * Check if roles are properly assigned

3. **Event store not persisting data**
   * Verify MongoDB connection
   * Check if the `events` collection exists

## 🛣️ Roadmap

* Implement **Product Management Module**
* Develop **Customer Management Module**
* Enhance **Reporting Features**
* Build **Admin Dashboard**
* Integrate with **Ambev Inventory System**
* Develop a **Mobile Application**
* Add **Offline Capabilities**

## 🤝 Contributing

Contributions are welcome! Follow these steps:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Commit changes: `git commit -m 'feat: add new feature'`
4. Push to branch: `git push origin feature/new-feature`
5. Open a Pull Request

Adhere to the Conventional Commits standard.

## 📜 License

This project is licensed under the **MIT License**. See the LICENSE file for details.

## 👤 Author

* 👨‍💻 **Felipe Gonçalves Ferreira**
* 📧 Contact: **contato@felipegf.com.br**
* 📌 [GitHub](https://github.com/yourusername) | [LinkedIn](https://linkedin.com/in/yourusername)

<p align="center"> Made with ❤️ for Ambev Developer Evaluation </p>