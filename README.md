# Gym Management Project

This project is a comprehensive web application for gym management, designed for administrators, trainers, and end users. The application allows managing users, roles, exercises, routines, payments, group classes, and users' physical progress.

## Key Features

1. **User Management:**
   - Create, read, update, and delete users.
   - Assign roles to users.
   - Authentication and session management.

2. **Routines:**
   - Assign personalized routines to clients by trainers.
   - View previous routines and track client progress.

3. **Group Classes:**
   - Manage and schedule group classes.

4. **Payments:**
   - Manage membership and class payments.
   - Generate payment reports.

5. **User Progress:**
   - Track clients' physical progress over time.

6. **Email Notifications:**
   - Send automatic notifications to users about appointments and progress.

## Technologies Used

- **Backend:**
  - C# .NET Core
  - ASP.NET Core MVC
  - Entity Framework Core
  - SQL Server

- **Frontend:**
  - HTML, CSS, JavaScript, Bootstrap
  - cshtml for dynamic views

- **APIs:**
  - RESTful APIs for managing users, routines, payments, progress, and group classes.

- **Authentication and Security:**
  - ASP.NET Core Identity
  - JWT (Authentication Tokens)

- **Other:**
  - Swagger for API documentation.
  - CORS for handling cross-origin policies.
  - Email configuration with SMTP services.

## Architecture

The project follows an **MVC (Model-View-Controller)** architecture, with a clear separation between business logic, data access, and user interface. Additionally, RESTful APIs are implemented to enable interaction between the client and server, providing a standard interface for CRUD operations.

## Project Structure

- **API:** Contains the controllers that manage the main operations.
- **BL (Business Logic):** Implements the business logic of the system.
- **DataAccess:** Handles database access using Entity Framework.
- **DTO (Data Transfer Objects):** Defines the objects used to transfer data between layers.

## Project Images

Below are some images showcasing the application's functionality and user interface:

### 1. Login Screen
![Login Screen](path/to/image/login.png)

### 2. User Management
![User Management](path/to/image/users.png)

### 3. Routine Assignment
![Routine Assignment](path/to/image/routines.png)

### 4. User Progress Dashboard
![User Progress Dashboard](path/to/image/progress.png)

_(Replace 'path/to/image/' with the correct paths to the project images)._

---

This README provides an overview of the gym management project. For more details, refer to the documentation and source code.
"""

# Writing the English version of the README.md file
with open(readme_path, 'w') as file:
    file.write(readme_content_english)

readme_content_english
