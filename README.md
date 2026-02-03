# Password Manager

A secure, full-stack password management solution built with .NET 10 and PostgreSQL. This application provides secure password storage with encryption, JWT-based authentication, and a RESTful API.

## 📋 Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Security Features](#security-features)
- [Configuration](#configuration)
- [Database Schema](#database-schema)

## ✨ Features

- **Secure Password Storage**: Encrypted password entries with user-specific encryption keys
- **User Authentication**: JWT-based authentication with refresh token support
- **Token Management**: Token revocation and verification capabilities
- **User Management**: Full CRUD operations for user accounts
- **Password Entries**: Create, read, update, and delete password entries
- **Audit Trail**: Automatic tracking of creation and modification timestamps
- **CORS Support**: Configured for frontend integration (React)
- **FluentValidation**: Comprehensive input validation
- **Clean Architecture**: Separated concerns with Domain, Infrastructure, and Presentation layers

## 🏗️ Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

```
┌─────────────────────────────────────────────┐
│     PasswordManager.Web.Client (API)       │
│          Controllers & Endpoints            │
└──────────────┬──────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│       PasswordManager.Domain               │
│    Services, Validators, Business Logic    │
└──────────────┬──────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│     PasswordManager.Infrastructure         │
│   Repositories, EF Core, Data Access       │
└────────────────────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│       PasswordManager.Model                │
│    DTOs, ViewModels, Builders, Mappers     │
└────────────────────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│       PasswordManager.Common               │
│          Constants & Utilities             │
└────────────────────────────────────────────┘
```

## 🛠️ Technology Stack

- **Framework**: .NET 10 / C# 14.0
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **Validation**: FluentValidation
- **Object Mapping**: AutoMapper
- **API Documentation**: OpenAPI/Swagger
- **Architecture**: Repository Pattern, Unit of Work Pattern

## 📁 Project Structure

### PasswordManager.Web.Client
API layer containing controllers and endpoint definitions:
- `Controllers/Account/AuthController.cs` - Authentication endpoints
- `Controllers/Account/UserController.cs` - User management endpoints
- `Controllers/EntryController.cs` - Password entry endpoints
- `Program.cs` - Application configuration and DI setup

### PasswordManager.Domain
Business logic layer:
- Services implementing business rules
- FluentValidation validators
- Service interfaces

### PasswordManager.Infrastructure
Data access layer:
- Entity Framework Core entities
- Repository implementations
- Database context and migrations
- Unit of Work implementation

### PasswordManager.Model
Data transfer layer:
- DTOs (Data Transfer Objects)
- ViewModels
- Model builders and mappers

### PasswordManager.Common
Shared utilities and constants

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL 12 or higher
- Visual Studio 2022 or higher (or VS Code)

### Environment Variables

Set the following environment variables (Machine level):

```
PMJWT__Issuer=<your-jwt-issuer>
PMJWT__Audience=<your-jwt-audience>
PMJWT__Key=<your-secret-key-min-32-chars>
PM_DB_CONNECTION_STRING=Host=localhost;Database=passwordmanager;Username=<user>;Password=<password>
```

### Installation

1. Clone the repository:
```bash
git clone https://github.com/Jguzm/PaswordManager.git
cd PaswordManager
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Apply database migrations:
```bash
cd PasswordManager.Infrastructure
dotnet ef database update
```

4. Run the application:
```bash
cd ../PasswordManager.Web.Client
dotnet run
```

The API will be available at `http://localhost:5000` (or configured port).

## 📚 API Documentation

### Base URL
All endpoints are prefixed with `/api`

### Authentication Endpoints (`/api/auth`)

#### POST /api/auth/register
Register a new user account.

**Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "username": "string",
  "email": "string",
  "password": "string"
}
```

**Response:** `200 OK` or `400 Bad Request`

#### POST /api/auth/login
Authenticate and receive JWT tokens.

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Response:**
```json
{
  "authToken": {
    "token": "string",
    "refreshToken": "string",
    "expiresAt": "2024-01-01T00:00:00Z"
  }
}
```

#### POST /api/auth/refresh
Refresh an expired JWT token.

**Request Body:** `"refreshTokenString"`

**Response:** Same as login

#### POST /api/auth/logout
Revoke a token (logout).

**Request Body:** `"tokenString"`

**Response:** `200 OK` or `404 Not Found`

#### POST /api/auth/verify
Verify token validity.

**Request Body:** `"tokenString"`

**Response:**
```json
{
  "valid": true
}
```

### User Endpoints (`/api/users`) 🔒

All user endpoints require authentication.

#### GET /api/users
Retrieve all users.

**Response:**
```json
[
  {
    "id": 1,
    "firstName": "string",
    "lastName": "string",
    "username": "string",
    "email": "string",
    "isActive": true
  }
]
```

#### GET /api/users/current
Get current authenticated user details.

#### GET /api/users/{userId}
Get user by ID.

#### POST /api/users
Create a new user.

#### PUT /api/users/{userId}
Update user information.

#### DELETE /api/users/{userId}
Delete a user.

### Entry Endpoints (`/api/entries`) 🔒

All entry endpoints require authentication.

#### GET /api/entries
Get all password entries for the authenticated user.

**Response:**
```json
{
  "total": 10,
  "entries": [
    {
      "id": 1,
      "title": "Gmail",
      "username": "user@example.com",
      "password": "encrypted_password",
      "url": "https://gmail.com",
      "notes": "Personal email",
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    }
  ]
}
```

#### GET /api/entries/{entryId}
Get a specific password entry.

#### POST /api/entries
Create a new password entry.

**Request Body:**
```json
{
  "title": "string",
  "username": "string",
  "password": "string",
  "url": "string",
  "notes": "string"
}
```

#### PUT /api/entries/{entryId}
Update a password entry.

#### DELETE /api/entries/{entryId}
Delete a password entry.

## 🔐 Security Features

### Password Hashing
- User passwords are hashed using industry-standard algorithms
- Each user has a unique authentication salt

### Encryption
- Password entries are encrypted using user-specific encryption keys
- Each entry has its own initialization vector (IV)
- Encryption salt stored per user

### JWT Authentication
- Access tokens with configurable expiration
- Refresh tokens for seamless re-authentication
- Token revocation support
- Device tracking for tokens

### Validation
- FluentValidation for comprehensive input validation
- Business rule validation at the service layer
- Entity validation before database operations

### Additional Security Measures
- Failed login attempt tracking (`BadPwdCount`)
- CORS configuration for frontend isolation
- HTTPS ready (uncomment in production)
- Antiforgery token support

## ⚙️ Configuration

### CORS
Default configuration allows requests from `http://localhost:5173` (React dev server). Update in `Program.cs` for production:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy => 
        policy.WithOrigins("https://your-production-domain.com")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
```

### JWT Settings
Configure JWT settings via environment variables:
- `PMJWT__Issuer`: Token issuer
- `PMJWT__Audience`: Token audience
- `PMJWT__Key`: Secret key (minimum 32 characters)

### Database
PostgreSQL connection string set via `PM_DB_CONNECTION_STRING` environment variable.

## 🗄️ Database Schema

### Users Table
| Column | Type | Description |
|--------|------|-------------|
| Id | bigint | Primary key (auto-increment) |
| FirstName | varchar | User's first name |
| LastName | varchar | User's last name |
| Username | varchar | Unique username |
| Email | varchar | User's email address |
| PasswordHash | varchar | Hashed password |
| AuthenticationSalt | varchar | Salt for password hashing |
| EncryptionSalt | varchar | Salt for entry encryption |
| IsActive | boolean | Account status |
| BadPwdCount | integer | Failed login attempts |
| CreatedAt | timestamp | Account creation date |
| UpdatedAt | timestamp | Last modification date |

### Entries Table
| Column | Type | Description |
|--------|------|-------------|
| Id | bigint | Primary key (auto-increment) |
| Title | varchar | Entry title/name |
| Username | varchar | Stored username |
| Password | varchar | Encrypted password |
| Url | varchar | Associated URL |
| Notes | text | Additional notes |
| InitializationVector | varchar | Encryption IV |
| CreatedAt | timestamp | Entry creation date |
| UpdatedAt | timestamp | Last modification date |

### AuthTokens Table
| Column | Type | Description |
|--------|------|-------------|
| Id | bigint | Primary key (auto-increment) |
| Token | varchar | JWT access token |
| RefreshToken | varchar | Refresh token |
| CreatedAt | timestamp | Token creation date |
| ExpiresAt | timestamp | Token expiration date |
| RefreshTokenExpiresAt | timestamp | Refresh token expiration |
| RevokedAt | timestamp | Token revocation date (nullable) |
| DeviceInfo | varchar | User agent/device info |
| UserId | bigint | Foreign key to Users |

## 🤝 Contributing

This is a personal project. For questions or suggestions, please open an issue.

## 📧 Contact

- GitHub: [@JaGuzDev](https://github.com/JaGuzDev)
- Repository: [PaswordManager](https://github.com/JaGuzDev/PaswordManager)

---

**Note**: Remember to keep your JWT secret keys and database credentials secure. Never commit sensitive configuration to version control.
