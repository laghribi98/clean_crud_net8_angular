
# Ticket Management System

## Overview

The **Ticket Management System** is a full-stack web application for managing tickets efficiently. It consists of a **backend API** built with **.NET 8** using **CQRS** (Command Query Responsibility Segregation) and the **Mediator pattern** to ensure scalability and maintainability. The **frontend** is developed using **Angular 18** and **Bootstrap 5**, providing a clean, responsive user interface.

### Key Features:
- Full ticket lifecycle management: Create, update, view, and delete tickets.
- Support for **pagination**, **sorting**, and **filtering** tickets.
- **Inline ticket creation and update** for enhanced user experience.
- **RESTful API** on the backend and responsive design on the frontend.

## Prerequisites

Ensure you have the following installed:

- **Node.js v16+** (for running the frontend)
- **Angular CLI v15+** (for managing Angular frontend)
- **Bootstrap 5** (for frontend styling)
- **.NET 8 SDK** (for the backend API)
- **Docker** (optional, for running SQL Server in a container if not installed locally)

## Backend Setup

### Step 1: Clone the Repository
```bash
git clone <repository-url>
cd <backend-directory>
```

### Step 2: Install Dependencies
Restore the necessary NuGet packages:
```bash
dotnet restore
```

### Step 3: Set Up the Database
Run a SQL Server container if you don't have a local SQL Server installed:
```bash
docker run --name sql-server -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=your_password' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

Configure your connection string in `appsettings.json` to connect to the SQL Server.

### Step 4: Apply Migrations
Apply Entity Framework Core migrations to set up the database schema:
```bash
dotnet ef database update
```

### Step 5: Run the Backend API
Start the backend service:
```bash
dotnet run
```
The API will be available at `http://localhost:5035/api/Tickets`.

## API Documentation

The backend API is documented using **Swagger**. Swagger provides a user-friendly interface for testing and interacting with the API endpoints.

After running the backend API, you can access the Swagger documentation at:

- [Swagger UI - Ticket Management API](http://localhost:5035/swagger/index.html)

Here’s a screenshot of the Swagger interface for this project:

![Swagger UI Screenshot](./images/backend_swagger.png)

### Available Endpoints

- **GET** `/api/Tickets/{id}`: Retrieve a specific ticket by its ID.
- **PUT** `/api/Tickets/{id}`: Update a ticket by its ID.
- **DELETE** `/api/Tickets/{id}`: Delete a ticket by its ID.
- **POST** `/api/Tickets`: Create a new ticket.
- **GET** `/api/Tickets`: Retrieve all tickets with optional filters.

## Frontend Setup

### Step 1: Clone the Repository
```bash
git clone <repository-url>
cd <frontend-directory>
```

### Step 2: Install Dependencies
Install the required dependencies:
```bash
npm install
```

### Step 3: Run the Frontend Application
To start the Angular application:
```bash
ng serve
```
Visit `http://localhost:4200/` to view the app in your browser.

## Project Structure

### Backend
- **Domain Layer**: Core business logic including entities and enumerations.
- **Application Layer**: Handles business rules, commands, queries, and validation.
- **Infrastructure Layer**: Manages database access, repositories, and configurations.
- **API Layer**: Exposes the RESTful API with controllers and middleware.

### Frontend
- **src/app/components/**: Components like `TicketList`, `TicketForm`, etc.
- **src/app/models/**: Data models such as `TicketRequest`, `TicketResponse`, and `ApiResponse`.
- **src/app/services/**: `TicketService` for API communication.
- **src/styles.css**: Custom styles for the frontend.

## Core Features

### Backend
- **CQRS & Mediator Pattern**: Separates command and query logic using MediatR for scalability.
- **Entity Framework Core**: Handles database interactions.
- **Clean Architecture**: Organized by layers to ensure separation of concerns.
- **Error Handling Middleware**: Catches and returns structured error responses.

### Frontend
- **CRUD Operations**: Manage tickets (create, read, update, delete).
- **Pagination, Sorting, Filtering**: Navigate, sort, and filter tickets with ease.
- **Responsive Design**: Bootstrap-powered, mobile-friendly interface.

## Running Tests

### Backend
To run unit tests for the backend:
```bash
dotnet test
```
