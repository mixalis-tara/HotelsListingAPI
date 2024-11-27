# Hotel Management Web API

## Overview

This Web API is built using **ASP.NET Core 8.0**, designed for managing hotels and countries. The project follows a clean architecture with three separate layers: **Data**, **Core**, and **Main Application**. It implements robust features such as authentication, error handling, and versioning to provide a secure and scalable solution. The API is fully documented with Swagger and supports JWT-based authentication for secured endpoints.

## Features

- **Hotel and Country Management**: Perform CRUD operations for hotels and countries.
- **Authentication and Authorization**:
  - Secure endpoints using **JWT tokens**.
  - User account management with login and registration.
  - Refresh tokens for seamless user experience.
- **Repository Pattern**:
  - Use of generic interfaces and classes for efficient data access and reusability.
  - Separation of concerns with the implementation of contracts and repositories.
- **Mapping with AutoMapper**:
  - Prevent overposting and ensure security by using Data Transfer Objects (DTOs).
- **Global Error Handling**:
  - Custom exceptions like `NotFoundException` for better debugging and user feedback.
  - Centralized middleware to handle errors gracefully.
- **Versioning**:
  - API versioning to maintain backward compatibility.
- **Testing Tools**:
  - Integrated Swagger for API documentation and testing.
  - Postman collection support.
- **Deployment**:
  - Deployed locally using IIS.

## Technologies Used

### Backend
- **ASP.NET Core 8.0**: Framework for building the API.
- **Entity Framework Core**: ORM for database interactions.
- **JWT Bearer Authentication**: For secure user authentication.
- **AutoMapper**: For mapping between models and DTOs.

### Database
- **MSSQL Server**: Relational database.
- **EF Tools**: Used for migrations and database seeding.

### Logging
- **Serilog**: Advanced logging framework.
- **Seq**: A log visualization tool integrated with Serilog.

### API Documentation and Testing
- **Swagger with JWT Authentication**: Integrated to test secured endpoints.
- **API Versioning**: Tools like `versioning` and `versioning.apiExplorer` for maintaining multiple API versions.

### Libraries
- `AutoMapper`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.AspNetCore.ApiVersioning`
- `Serilog`, `Serilog.Expressions`, `Serilog.Sinks.Seq`

## Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/yourrepository.git
   ```

2. **Navigate to the project directory**:
   ```bash
   cd yourrepository
   ```

3. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

4. **Update the database connection string**:
   - Open `appsettings.json` in the Data project and update the `ConnectionStrings` section with your MSSQL database connection string.

5. **Apply database migrations**:
   ```bash
   dotnet ef database update
   ```

6. **Run the application**:
   ```bash
   dotnet run
   ```

7. **Set up IIS**:
   - Publish the app to a local folder:
     ```bash
     dotnet publish -o ./publish
     ```
   - Use IIS to add the published folder as a new site on localhost.

## Usage

### Hotel and Country Management

- **Create, Read, Update, Delete (CRUD)**:
  - Use the respective controllers (`HotelsController` and `CountriesController`) to perform CRUD operations on hotels and countries.
  - Endpoints are secured; ensure you have a valid JWT token.

### Authentication

- **Register**:
  - Use the `AuthManagerController` to create a new account.
  - A new token is generated upon successful registration.
- **Login**:
  - Use the same controller to log in with valid credentials and receive a JWT token.
- **Refresh Token**:
  - Access the token refresh endpoint to get a new token when the current one is close to expiry.

### Swagger Integration

- Open `http://localhost:<port>/swagger` in your browser to view and test the API.
- Add your JWT token using the **Authorize** button to test secured endpoints.

### Global Error Handling

- The application includes middleware for centralized error handling.
- Custom exceptions, such as `NotFoundException`, are used to provide meaningful error messages.

### Versioning

- Versioned endpoints are accessible using Swagger.
- Example: `/api/v1/hotels` for version 1.

## Customization

### JWT Authentication in Swagger

- The Swagger UI supports testing secured endpoints using JWT tokens. To customize:
  - Open `Startup.cs` or `Program.cs` and ensure the `AddSwaggerGen` method includes `c.AddSecurityDefinition` and `c.AddSecurityRequirement`.

### Serilog and Seq

- Update the `Seq` settings in `appsettings.json` to point to your Seq instance.
- Customize the logging levels or filters using `Serilog.Expressions`.

### Repository Pattern

- Modify or extend the generic repository interfaces and their implementations in the `Core` project to add new features or customize existing ones.

