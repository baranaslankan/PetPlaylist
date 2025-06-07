# Pet Hotel CMS - MVP

This is a simple Web API project built for my ASP.NET Core Passion Project course.

## 🐾 Project Description

Pet Hotel CMS is a system to manage pet hotel customers and their pets. This MVP version allows basic Create, Read, Update, and Delete (CRUD) operations for pet owners and their animals through API endpoints.

## ✅ Features (MVP Scope)

- ASP.NET Core Web API (no frontend)
- Entity Framework Core with **Code-First Migrations**
- SQLite database
- CRUD operations using **LINQ**
- **DTO**s for clean data transfer
- Swagger API documentation

## 📁 Entities

- `Owner`: A person who owns one or more animals.
- `Animal`: A pet belonging to an owner (e.g., dog, cat).

## 🧩 Technologies Used

- ASP.NET Core (.NET 8)
- Entity Framework Core
- SQLite
- Swagger / Swashbuckle
- Visual Studio Code / CLI

## 🧪 How to Run

```bash
dotnet restore
dotnet ef database update
dotnet run
# PetHotelCMS
# PetHotelCMS
