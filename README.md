# Fichario Digital

A REST API built to help a doctor to manage he's clinic's patient records and finances. It handles the full lifecycle of patient data, registration, search, categorization, health plan association, along with financial tracking of income and expenses.

## Tech Stack

- **Runtime:** .NET 8.0 / ASP.NET Core
- **Language:** C#
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core (code-first migrations)
- **Auth:** JWT Bearer tokens with SHA256 password hashing
- **Docs:** Swagger / OpenAPI
- **Containerization:** Docker & Docker Compose

## Features

### Patient Management
- Full CRUD with validation and duplicate detection (CPF, RG, file numbers)
- Accent-insensitive and case-insensitive name search (PostgreSQL `unaccent()`)
- Auto-incrementing file numbers (consultation and ultrasound)
- Health plan and category association
- Emergency contact management (cascade delete)
- Pagination and advanced filtering (name, CPF, RG, phone, gender, birth date, category, health plan)

### Financial Module
- Track income and expenses with a single `Payment` entity
- Associate payments to doctors, patients, or health plans
- Filter by date range, payment method (Card, Pix, Cash), doctor, or health plan

### Doctors & Health Plans
- Doctor registry linked to payments
- Health plan registry linked to patients and payments
- Pre-delete impact verification for health plans and categories

### Authentication
- User registration and login
- JWT token issuance and validation
- All endpoints (except auth) require a valid Bearer token

## Project Structure

```
Controllers/        API endpoints
Business/           Service layer + interfaces
Data/
  Repositories/     Data access + interfaces
  Migrations/       EF Core migrations
  AppDbContext.cs    Database context
Model/
  DTO/              Request/response objects
  Mapper/           Entity <-> DTO mapping
  Enum/             PaymentMethod (Card, Pix, Cash)
Infrastructure/     DI, JWT config, Swagger config, DB migrator
Utils/              String normalization helpers
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://docs.docker.com/get-docker/) (for PostgreSQL)

### 1. Start the database

```bash
docker-compose up -d
```

This spins up a PostgreSQL instance with the `clinic_db` database. The `init.sql` script runs automatically to install the `unaccent` extension.

### 2. Set environment variables

| Variable | Description | Example |
|---|---|---|
| `CONNECTION_STRING` | PostgreSQL connection string | `Host=localhost;Port=5432;Database=clinic_db;Username=clinic_user;Password=clinic_pass` |
| `JWT_SECRET_KEY` | Secret key for signing JWT tokens | *(any strong secret)* |

### 3. Run the application

```bash
dotnet run
```

The API starts on `http://localhost:5000`. Migrations run automatically on startup.

### 4. Open Swagger UI

Navigate to `http://localhost:5000/swagger` to explore and test all endpoints.

### Reset the database

```bash
# Linux / macOS
./reset_db.sh

# Windows (PowerShell)
.\reset_db.ps1
```

These scripts tear down and recreate the Docker volume, giving you a clean database.

## API Overview

| Area | Endpoints |
|---|---|
| **Auth** | `POST /api/auth/register`, `POST /api/auth/login` |
| **Patients** | `GET /api/patient/list`, `GET /api/patient/search`, `GET /api/patient/{id}`, `POST /api/patient/create`, `POST /api/patient/update`, `POST /api/patient/delete/{id}`, `POST /api/patient/validate`, `GET /api/patient/getnextfilenumber` |
| **Payments** | `GET /api/payment`, `POST /api/payment`, `PUT /api/payment`, `DELETE /api/payment/{id}` |
| **Doctors** | `GET /api/doctor`, `POST /api/doctor`, `PUT /api/doctor`, `DELETE /api/doctor/{id}` |
| **Health Plans** | `GET /api/healthplan`, `POST /api/healthplan`, `POST /api/healthplan/update`, `POST /api/healthplan/delete` |
| **Categories** | `GET /api/category`, `POST /api/category`, `POST /api/category/update`, `POST /api/category/delete` |

All endpoints except `/api/auth/*` require a `Bearer` token in the `Authorization` header.

#IMPORTANT NOTE
This is a real app made for a clinic and was authorized to be in a public repo by the *client*
