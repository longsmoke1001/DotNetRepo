# Personal Notes API

A RESTful API built with ASP.NET Core for managing personal notes, diaries, and password records.

## Tech Stack

- ASP.NET Core 10
- Entity Framework Core 10
- SQLite
- JWT Authentication
- Swagger / OpenAPI

## Features

- CRUD operations for notes
- Categorization (General / Diary / Password)
- Search by title or content
- Pagination support
- JWT Authentication
- Swagger API documentation

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQLite (built-in)

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/your-username/personal-notes-api.git
   cd personal-notes-api
## API Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| GET | `/api/notes` | Get all notes (paginated) |
| GET | `/api/notes/{id}` | Get a specific note |
| POST | `/api/notes` | Create a new note |
| PUT | `/api/notes/{id}` | Update a note |
| DELETE | `/api/notes/{id}` | Delete a note |
| GET | `/api/notes/search?query={keyword}` | Search notes |
| GET | `/api/notes/category/{category}` | Filter by category |
| POST | `/api/auth/login` | Login to get JWT token |

## Default Test User

For testing purposes, you can use the following credentials:

- Username: `admin`
- Password: `password`
