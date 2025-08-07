# Vehicle Management Demo Solution

This is a demo/test project to showcase a simple vehicle management system with clean architecture, testability, and modern .NET patterns.

---

## **How to Run**

### 1. **Create the Database**

Create a database with these tables and example data:

- **VehicleMake**
    - Columns: `Id` (int, PK), `Name` (string), `Abrv` (string)
    - Example rows:  
      `1, BMW, BMW`  
      `2, Ford, FORD`  
      `3, Volkswagen, VW`

- **VehicleModel**
    - Columns: `Id` (int, PK), `MakeId` (int, FK to VehicleMake), `Name` (string), `Abrv` (string)
    - Example rows:  
      `1, 1, 128, 128`  
      `2, 1, 325, 325`  
      `3, 1, X5, X5`  
      `4, 2, Focus, FOC`  
      `5, 3, Golf, GLF`

You can use **SQL Server Express**, **LocalDB**, or any SQL Server compatible with EF6.

---

### 2. **Build & Run the Solution**

- Open the solution in **Visual Studio** (2019 or later).
- Build all projects to restore NuGet packages.
- Update the **connection string** in `Project.MVC_\web.config` to point to your database.
- Run the solution. The default project is **Project.MVC_** (the web frontend).

---

## **Project Structure & Features**

- **Project.Service_**
    - EF6 Code First Models for VehicleMake and VehicleModel
    - `VehicleService` class for CRUD, sorting, filtering, and paging (async/await everywhere)
    - All logic is abstracted via interfaces for testability
- **Project.MVC_**
    - Admin views for VehicleMakes and VehicleModels (CRUD, sorting, filtering, paging)
    - Filter VehicleModels by Make
    - Uses view models (never exposes EF models to the view)
    - Returns appropriate HTTP status codes
    - Uses **AutoMapper** for mapping between models and view models
    - Uses **Ninject** for Dependency Injection

---

## **Implementation Details**

- **Async/await** used throughout all layers
- **Interfaces** for all services and repositories (for easy unit testing)
- **Dependency Injection** via **Ninject**
- **AutoMapper** for mapping
- **Entity Framework 6** (Code First)
- **MSTest** and **Moq** for unit testing

---

## **How to Test**

- Run the solution.  
- Navigate to `/VehicleMake` or `/VehicleModel` in your browser for the admin UI.
- You can sort, filter, and page through makes and models.
- Filtering models by make is supported.

---

## **Notes**

- This is a demo project for demonstration/testing purposes only.
- The codebase is clean, testable, and applies common .NET best practices for maintainability.

---

## **Questions?**

If you have trouble running the solution or need further customization, please contact the developer (kristijan.matic@hotmail.com).