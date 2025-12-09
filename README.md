/* 
-----------------------------------------------------------------------------------------
README.md - Employee Management System - TalentoPlus S.A.S.
-----------------------------------------------------------------------------------------

# Employee Management System - TalentoPlus S.A.S.

## Project Description

This is a comprehensive Human Resources and Catalog Management System developed in 
ASP.NET Core 9.0 using a strict Clean Architecture approach. The project includes:

- Full employee CRUD.
- Mass Employee Import from Excel with dynamic column mapping.
- Catalog management (Departments, Positions).
- AI-assisted dashboard using Gemini API.
- Secure authentication using JSON Web Tokens (JWT).
- Full containerization with Docker and Docker Compose.

Developed by: **Mariana Quintero Cardona** (Clan: Hopper)

-----------------------------------------------------------------------------------------

## Technologies and Latest Update

| Component            | Technology            | Version |
|--------------------- |-----------------------|---------|
| Backend Framework    | ASP.NET Core          | 9.0 |
| Language             | C#                    | 12 |
| Database             | MySQL                 | 8.0 |
| ORM                  | Entity Framework Core | 9.0 |
| Containerization     | Docker + Compose      | Latest |
| AI Integration       | Gemini API            | Latest |

-----------------------------------------------------------------------------------------

## Clean Architecture Structure

| Layer         | Project Name                 | Responsibility |
|-------------- |------------------------------|----------------|
| Presentation  | TalentoPlus.Api              | Controllers, DI setup, Swagger |
| Application   | TalentoPlus.Application      | Business logic, CQRS, DTOs |
| Domain        | TalentoPlus.Domain           | Entities, value objects, enums |
| Infrastructure| TalentoPlus.Infrastructure   | EF Core, repositories, SMTP, identity |

-----------------------------------------------------------------------------------------

## Local Setup and Execution with Docker Compose

### Prerequisites:
- Git installed
- Docker Desktop installed and running

-----------------------------------------------------------------------------------------

### Step 1: Clone the Repository

```bash
git clone https://github.com/MarianaQC/Sistema-de-Gestion-de-Empleados-TalentoPlus-S.A.S..git
cd Sistema-de-Gestion-de-Empleados-TalentoPlus-S.A.S.


Create and Configure the .env File

Create a file named .env in the root project directory with the following content:

# --- DATABASE CONFIGURATION (MySQL) ---
MYSQL_ROOT_PASSWORD=<YOUR_MYSQL_ROOT_PASSWORD>
MYSQL_DATABASE=talento_plus
MYSQL_USER=talento_user
MYSQL_PASSWORD=<YOUR_DB_USER_PASSWORD>

ConnectionStrings__DefaultConnection=Server=mysql_db;Port=3306;Database=talento_plus;Uid=talento_user;Pwd=<YOUR_DB_USER_PASSWORD>;

# --- JWT AND SECURITY ---
JWT_SECRET=a-string-secret-at-least-256-bits-long
JWT_ISSUER=TalentoPlus
JWT_AUDIENCE=TalentoPlus

# --- GEMINI AI KEY ---
GEMINI_API_KEY=AIzaSyBLA38WAFA5KJsRHLB0SynVBZU0f6GpYzI

# --- SMTP EMAIL SERVICE ---
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=admin@talentoplus.com
SMTP_PASSWORD=iryn givo cygy ifwc
SMTP_FROM=no-reply@talentoplus.com

Build and Run the Containers
docker-compose up -d --build


API runs on port 5162

MySQL runs on port 3307

Step 4: Access the API Documentation

Visit:

http://localhost:5162/swagger

Initial Admin Credentials

Use these in /api/Auth/login:

Email: admin@talentoplus.com

Password: Admin123!

Then copy the token and click Authorize in Swagger.

Optional: Run EF Core Migrations Manually
dotnet ef database update --project TalentoPlus.Infrastructure --startup-project TalentoPlus

