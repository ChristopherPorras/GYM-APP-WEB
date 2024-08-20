# Proyecto de Gestión de Gimnasio

Este proyecto es una aplicación web completa para la gestión de un gimnasio, diseñada para administradores, entrenadores y usuarios finales. La aplicación permite gestionar usuarios, roles, ejercicios, rutinas, pagos, clases grupales y el progreso físico de los usuarios.

## Funcionalidades Principales

1. **Gestión de Usuarios:**
   - Crear, leer, actualizar y eliminar usuarios.
   - Asignación de roles a los usuarios.
   - Autenticación y manejo de sesiones.

2. **Rutinas:**
   - Asignación de rutinas personalizadas a los clientes por parte de los entrenadores.
   - Visualización de rutinas anteriores y progreso de los clientes.

3. **Clases Grupales:**
   - Administración y agendamiento de clases grupales.

4. **Pagos:**
   - Control de pagos de membresías y clases.
   - Generación de reportes de pagos.

5. **Progreso del Usuario:**
   - Seguimiento del progreso físico del cliente a lo largo del tiempo.

6. **Notificaciones por Correo Electrónico:**
   - Envío de notificaciones automáticas a los usuarios sobre citas y progreso.

## Tecnologías Utilizadas

- **Backend:**
  - C# .NET Core
  - ASP.NET Core MVC
  - Entity Framework Core
  - SQL Server

- **Frontend:**
  - HTML, CSS, JavaScript, Bootstrap
  - cshtml para vistas dinámicas

- **APIs:**
  - RESTful APIs para la gestión de usuarios, rutinas, pagos, progreso, y clases grupales.

- **Autenticación y Seguridad:**
  - ASP.NET Core Identity
  - JWT (Tokens de Autenticación)

- **Otros:**
  - Swagger para la documentación de la API.
  - CORS para manejar las políticas de acceso cruzado.
  - Configuración de correo electrónico con servicios SMTP.

## Arquitectura

El proyecto sigue una arquitectura **MVC (Modelo-Vista-Controlador)**, con una clara separación entre la lógica de negocio, el acceso a datos, y la interfaz de usuario. Además, se implementaron APIs RESTful que permiten la interacción entre el cliente y el servidor, proporcionando una interfaz estándar para las operaciones de CRUD.

## Estructura del Proyecto

- **API:** Contiene los controladores que gestionan las operaciones principales.
- **BL (Business Logic):** Implementa la lógica de negocio del sistema.
- **DataAccess:** Maneja el acceso a la base de datos utilizando Entity Framework.
- **DTO (Data Transfer Objects):** Define los objetos utilizados para transferir datos entre las capas.

## Imágenes del Proyecto

A continuación, algunas imágenes del funcionamiento de la aplicación y la interfaz de usuario:

### 1. Pantalla de Inicio de Sesión
![Pantalla de Inicio de Sesión](ruta/a/la/imagen/login.png)

### 2. Gestión de Usuarios
![Gestión de Usuarios](ruta/a/la/imagen/usuarios.png)

### 3. Asignación de Rutinas
![Asignación de Rutinas](ruta/a/la/imagen/rutinas.png)

### 4. Panel de Progreso del Usuario
![Progreso del Usuario](ruta/a/la/imagen/progreso.png)

_(Reemplaza 'ruta/a/la/imagen/' con las rutas correctas a las imágenes del proyecto)._

---

Este README proporciona una visión general del proyecto de gestión de gimnasio. Para más detalles, consulta la documentación y el código fuente.
"""

# Writing the updated content to the README.md file
with open(readme_path, 'w') as file:
    file.write(updated_readme_content)

updated_readme_content
