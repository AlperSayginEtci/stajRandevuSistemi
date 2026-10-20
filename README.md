# 📚 Library Reservation System

A web application for a multi-branch library network. Members can **reserve study rooms** and **borrow books** online, and administrators manage libraries, rooms, the book catalog and stock. I built it with **ASP.NET Core 8 Razor Pages** and **Entity Framework Core** during my mandatory summer internship (2025).

> The UI is in Turkish.

## Features

### For members
- **Register / log in** with cookie-based authentication. Passwords are hashed with ASP.NET Core Identity's `PasswordHasher`
- **Browse libraries** and see each branch's rooms and book stock
- **Study room booking** between 09:00 and 20:00, with conflict detection:
  - *Private* bookings reserve the whole room and block any overlapping booking
  - *Shared* bookings let several members use the same room until its **capacity** is reached
  - Already booked time slots are shown on the booking page
- **Book borrowing** with a chosen return date. Stock is decremented per branch
- **My reservations** (rooms and books), **reading history**, profile and password change

### For administrators
- Role-based admin area (`[Authorize(Roles = "Admin")]`)
- Manage **libraries**, assign **rooms** to branches and set their capacity
- Maintain a central **book pool** (title, author, genre, cover image upload) and assign stock to each branch
- View and manage **users** and their reservations

## Tech Stack

| Area | Tools |
|---|---|
| Framework | ASP.NET Core 8, Razor Pages |
| ORM | Entity Framework Core 9 (code-first migrations) |
| Database | Microsoft SQL Server |
| Auth | Cookie authentication, role claims, `PasswordHasher<T>` |
| Front end | Bootstrap, jQuery Validation, custom theme |

## Data Model

```
Kullanici (User) ──< RandevuOda (RoomBooking) >── Oda (Room) >──< Kutuphane (Library)
       │                                                              │
       └──< RandevuKitap (BookLoan) >── Kitap (Book) >── KutuphaneKitapStock (stock per library)
       └──< OkunanKitap (ReadingHistory)
```

Many-to-many relations (library ↔ room, library ↔ book stock) are modelled with composite keys in `ApplicationDbContext` using the Fluent API.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express or Docker)

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/AlperSayginEtci/stajRandevuSistemi.git
   cd stajRandevuSistemi/stajRandevuSistemi
   ```
2. Set your connection string in `appsettings.json` → `ConnectionStrings:DefaultConnection`, for example:
   ```json
   "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=KutuphaneDB;Trusted_Connection=True;TrustServerCertificate=True"
   ```
3. Create the database and run:
   ```bash
   dotnet tool install --global dotnet-ef   # if not installed
   dotnet ef database update
   dotnet run
   ```
4. Register an account. Signing in with `admin@hotmail.com` opens the admin panel.

## What I Learned
- Designing a relational schema with composite keys and many-to-many relations in EF Core
- Implementing booking logic with time-overlap checks and shared-capacity rules
- Cookie authentication and role-based authorization in ASP.NET Core
