# 🎵 **PetPlaylist** 

- `Owner`: Pet owner information (name, email, phone, address)
- `Pet`: Pet details (name, type, breed, age) with behavior tracking
- `Behavior`: Pet behaviors (Calm, Energetic, Anxious, Playful, etc.)
- `Artist` & `Song`: Music catalog management
- `Playlist`: Music collections associated with specific behaviors
- `Booking`: Pet stay records

## 🎯 Core Functionality

- **Behavior Management**: Assign behaviors to pets
- **Music Catalog**: Manage artists, songs, and playlists
- **Smart Recommendations**: Get playlist suggestions based on pet behaviors
- **Booking System**: Track pet stays and services unique system that manages pets and creates personalized music playlists based on their behaviors. It allows staff to track pet behaviors and recommend appropriate music playlists to help calm, energize, or entertain pets during their stay.Playlist - Behavior-Based Music Recommendation System

This is a Web API project that combines pet management with behavior-based music playlist recommendations.

## 📖 Project Description

**Pet Hotel CMS** is a lightweight content management system for a pet boarding facility. It allows staff to manage **owners**, their **pets**, and **bookings** (e.g. for a pet’s stay) using RESTful API endpoints.  
This MVP focuses on clean CRUD functionality and relationship management between entities.

## ✅ Features

- ASP.NET Core **Web API** with Swagger documentation
- Entity Framework Core with **Code-First Migrations**
- SQLite database
- CRUD operations for:
  - Owners & Pets
  - Artists & Songs
  - Playlists & Bookings
  - Behaviors & Pet-Behavior assignments
- **Behavior-based playlist recommendations**
- Many-to-many relationships between pets-behaviors and behaviors-playlists
- **DTOs** for secure and clean data transfer

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
```

Visit `http://localhost:5000/swagger` to explore the API endpoints.

## 🎵 API Endpoints

### Behavior Management
- `GET /api/Behaviors` - List all behaviors
- `POST /api/Behaviors` - Create new behavior
- `POST /api/Behaviors/{behaviorId}/AssignToPet/{petId}` - Assign behavior to pet

### Playlist Recommendations
- `GET /api/Playlist/ForPet/{petId}` - Get recommended playlists for a pet
- `GET /api/Playlist/ForBehavior/{behaviorId}` - Get playlists for specific behavior
- `POST /api/Playlist/{playlistId}/AssociateBehavior/{behaviorId}` - Associate playlist with behavior

### Pet & Owner Management
- Standard CRUD operations for Owners, Pets, and Bookings
- Music catalog management for Artists, Songs, and Playlists
