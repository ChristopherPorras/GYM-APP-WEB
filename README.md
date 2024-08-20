# Gym Management Project

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-%23512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-%23CC2927.svg?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![HTML5](https://img.shields.io/badge/HTML5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/CSS3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-%23F7DF1E.svg?style=for-the-badge&logo=javascript&logoColor=black)
![Bootstrap](https://img.shields.io/badge/Bootstrap-%23563D7C.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

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
  - ![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
  - ![.NET](https://img.shields.io/badge/.NET-%23512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white)
  - ![Entity Framework](https://img.shields.io/badge/Entity%20Framework-%23512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white)
  - ![SQL Server](https://img.shields.io/badge/SQL%20Server-%23CC2927.svg?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

- **Frontend:**
  - ![HTML5](https://img.shields.io/badge/HTML5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)
  - ![CSS3](https://img.shields.io/badge/CSS3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)
  - ![JavaScript](https://img.shields.io/badge/JavaScript-%23F7DF1E.svg?style=for-the-badge&logo=javascript&logoColor=black)
  - ![Bootstrap](https://img.shields.io/badge/Bootstrap-%23563D7C.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

- **APIs:**
  - RESTful APIs for managing users, routines, payments, progress, and group classes.

- **Authentication and Security:**
  - ![ASP.NET Core Identity](https://img.shields.io/badge/ASP.NET%20Core%20Identity-%23512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white)
  - ![JWT](https://img.shields.io/badge/JWT-%23FF5F5F.svg?style=for-the-badge&logo=json-web-tokens&logoColor=white)

- **Other:**
  - ![Swagger](https://img.shields.io/badge/Swagger-%2385EA2D.svg?style=for-the-badge&logo=swagger&logoColor=white)
  - ![CORS](https://img.shields.io/badge/CORS-%231DA1F2.svg?style=for-the-badge&logo=webflow&logoColor=white)
  - Email configuration with SMTP services.

## Architecture

The project follows an **MVC (Model-View-Controller)** architecture, with a clear separation between business logic, data access, and user interface. Additionally, RESTful APIs are implemented to enable interaction between the client and server, providing a standard interface for CRUD operations.

## Technology Analysis

1. **C# .NET Core:**
   - .NET Core is a powerful framework for building modern, scalable web applications. It provides a high-performance runtime and extensive libraries, making it ideal for backend development in this gym management system. C# as the primary language offers strong typing, rich APIs, and robust security features.

2. **ASP.NET Core MVC:**
   - The MVC architecture in ASP.NET Core allows for clear separation of concerns, which is crucial for maintainability and scalability. It helps organize the application into Models, Views, and Controllers, making the development process more efficient and easier to manage.

3. **Entity Framework Core:**
   - As an ORM (Object-Relational Mapping) framework, Entity Framework Core simplifies data access by allowing developers to work with databases using .NET objects. This eliminates the need for most of the data-access code that typically needs to be written, making it faster to develop and less prone to errors.

4. **SQL Server:**
   - SQL Server provides a reliable and secure database management system that integrates seamlessly with Entity Framework Core. Its robustness and scalability ensure that the application can handle large volumes of data efficiently.

5. **HTML, CSS, JavaScript, Bootstrap:**
   - The frontend technologies offer a responsive and user-friendly interface. Bootstrap, combined with HTML, CSS, and JavaScript, ensures that the application is visually appealing and accessible across various devices and screen sizes.

6. **RESTful APIs:**
   - RESTful APIs provide a standardized way for the frontend and backend to communicate. This enables a decoupled architecture, where the frontend and backend can evolve independently, making the system more modular and easier to maintain.

7. **ASP.NET Core Identity & JWT:**
   - ASP.NET Core Identity provides a secure way to manage user authentication and authorization. Combined with JWT tokens, it ensures that user sessions are secure and scalable, enabling the application to support multiple users and roles effectively.

8. **Swagger:**
   - Swagger simplifies API development by providing interactive documentation. This makes it easier for developers to understand and test the APIs, improving productivity and reducing errors during development.

9. **CORS:**
   - Cross-Origin Resource Sharing (CORS) policies are implemented to ensure that the application can safely handle requests from different origins, which is important for security, especially in web applications that interact with external services.

## GitHub Stats

![GitHub Stats](https://github-readme-stats.vercel.app/api/top-langs/?username=YOUR_GITHUB_USERNAME&layout=compact&theme=radical)

_(Replace `YOUR_GITHUB_USERNAME` with your GitHub username to display the languages used in this repository)._

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
