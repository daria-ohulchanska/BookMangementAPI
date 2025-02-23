# Book Management API

## Overview
Book Management API is a RESTful API built with ASP.NET Core Web API and EF Core. It allows users to manage a collection of books, including adding, updating, retrieving, and soft-deleting.

## Technologies Used
- **Programming Language:** C#
- **Framework:** .NET 8/9
- **Database:** SQL Server (via Entity Framework Core)
- **Architecture:** 3-layered (Models, Data Access, API)
- **Tools & Libraries:**
  - EF Core
  - Swagger (for API documentation)
  - JWT Authentication

## Features
- **CRUD Operations:**
  - Add a book (single and bulk)
  - Update a book
  - Soft delete books (single and bulk)
  - Retrieve a list of books sorted by popularity with pagination
  - Retrieve book details (with a popularity score calculation)
- **Validation:**
  - Prevent duplicate book titles
- **Popularity Calculation:**
  - Based on views count and book age:
    ```
    Popularity Score = BookViews * 0.5 + YearsSincePublished * 2
    ```
- **JWT Authentication:**
  - Secure endpoints so only authorized users can access them

## API Endpoints
### Books
| Method | Endpoint                | Description |
|--------|-------------------------|-------------|
| GET    | /api/books/titles       | Get paginated list of books sorted by popularity |
| GET    | /api/books/{id}         | Get details of a book (increments views count) |
| POST   | /api/books              | Add a new book |
| POST   | /api/books/add-range    | Add a book range |
| PUT    | /api/books/{id}         | Update book details |
| DELETE | /api/books/{id}         | Soft delete a book |
| DELETE | /api/books/{ids}        | Soft delete a book range |

### Authentication
| Method | Endpoint                | Description |
|--------|-------------------------|-------------|
| POST   | /api/auth/sign-in       | Authenticate user and return JWT tokens |
| POST   | /api/auth/sign-up       | Register a new user with email and password |
| POST   | /api/auth/sign-out      | Logout user and invalidate refresh token |

