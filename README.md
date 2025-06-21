# 🐶 Pet Hotel CMS - MVP

This is a Web API project developed for the ASP.NET Core Passion Project course.

## 📖 Project Description

**Pet Hotel CMS** is a lightweight content management system for a pet boarding facility. It allows staff to manage **owners**, their **pets**, and **bookings** (e.g. for a pet’s stay) using RESTful API endpoints.  
This MVP focuses on clean CRUD functionality and relationship management between entities.

## ✅ Features (MVP Scope)

- ASP.NET Core **Web API** (no frontend)
- Entity Framework Core with **Code-First Migrations**
- SQLite as database
- CRUD operations for:
  - Owners
  - Pets
  - Bookings
- **DTOs** for secure and clean data transfer
- Owner–Pet relationship with `/owners/{id}/with-pets` endpoint
- Swagger documentation and testing

## 📁 Entities

- `Owner`: A person who owns pets. Includes full name, email, phone number, and address.
- `Pet`: An animal belonging to an owner. Includes name, type, breed, age.
- `Booking`: A record for a pet’s stay at the hotel. Includes start and end date, and notes.

## 🧩 Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- SQLite
- Swagger (Swashbuckle)
- Visual Studio Code / .NET CLI

## 🧪 How to Run Locally

```bash
dotnet restore
dotnet ef database update
dotnet run
