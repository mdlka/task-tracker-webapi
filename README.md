# TaskTracker

TaskTracker is a task management API with CRUD operations for boards and tasks, featuring JWT authentication. 

## Technologies

- .NET Core 7
- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL
- JWT
- Swagger

## Project Overview

The project provides the following features:

- User registration and authentication using JWT;
- CRUD operations for boards;
- CRUD operations for tasks within boards;
- Access control to resources based on user credentials;
- API documentation via Swagger UI for simplified testing.

## Database Structure

![App Screenshot](https://raw.githubusercontent.com/mdlka/task-tracker-webapi/refs/heads/main/database_structure.png)

## API Endpoints

### Authentication

- `POST /api/auth/register` – Register a new user
- `POST /api/auth/login` – User login
- `POST /api/auth/refresh` – Refresh authentication token

### Boards

- `GET /api/boards` – Get all boards
- `GET /api/boards/{id}` – Get a specific board
- `POST /api/boards` – Create a new board
- `PUT /api/boards` – Update a board
- `DELETE /api/boards/{id}` – Delete a board

### Tasks

- `GET /api/todoitems?boardId={id}` – Get tasks from a board
- `GET /api/todoitems/{id}` – Get a specific task
- `POST /api/todoitems?boardId={id}` – Add a task
- `PUT /api/todoitems` – Update a task
- `DELETE /api/todoitems/{id}` – Delete a task

## Setup

1. Configure the database connection in `appsettings.json`.
2. Apply migrations: `dotnet ef database update`.
3. Run the API: `dotnet run --project TaskTracker.WebAPI`.
