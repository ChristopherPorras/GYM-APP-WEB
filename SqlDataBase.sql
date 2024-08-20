USE db_aaaf3a_gymproyect2;

-- Tabla de Usuarios
CREATE TABLE Usuarios
(
    CorreoElectronico VARCHAR(255) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Contrasena NVARCHAR(100) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Telefono NVARCHAR(20),
    TipoUsuario NVARCHAR(50) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    -- 1: Activo, 0: Inactivo
    HaPagado BIT NOT NULL DEFAULT 0,
    -- 1: Ha pagado, 0: No ha pagado
    CorreoVerificado BIT NOT NULL DEFAULT 0,
    TelefonoVerificado BIT NOT NULL DEFAULT 0
);

-- Tabla de Salt
CREATE TABLE Salts
(
    CorreoElectronico VARCHAR(255) PRIMARY KEY,
    Salt VARBINARY(MAX) NOT NULL,
    CONSTRAINT FK_Salt_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico)
);

-- Tabla de OTP para verificación
CREATE TABLE OTP
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    CodigoOTP NVARCHAR(6) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    Usado BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_OTP_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico)
);

-- Tabla de Roles
CREATE TABLE Roles
(
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(255)
);

-- Tabla Intermedia Usuarios-Roles
CREATE TABLE UsuarioRoles
(
    CorreoElectronico VARCHAR(255),
    RolId INT,
    CONSTRAINT FK_UsuarioRoles_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico),
    CONSTRAINT FK_UsuarioRoles_Rol FOREIGN KEY (RolId) REFERENCES Roles(Id),
    PRIMARY KEY (CorreoElectronico, RolId)
);

-- Tabla de Equipos
CREATE TABLE Equipos
(
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    GrupoMuscular NVARCHAR(20),
    Cantidad INT,
    Disponibilidad BIT
);

-- Tabla de Ejercicios
CREATE TABLE Ejercicios
(
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL,
    -- Peso, Tiempo, AMRAP
    Descripcion NVARCHAR(255),
    Peso DECIMAL(10, 2),
    -- Para ejercicios basados en peso
    Tiempo INT,
    -- Para ejercicios basados en tiempo y AMRAP
    AMRAP BIT
    -- Para indicar si el ejercicio es AMRAP
);

-- Tabla Intermedia Ejercicio-Equipo
CREATE TABLE EjercicioEquipos
(
    EjercicioId INT,
    EquipoId INT,
    CONSTRAINT FK_EjercicioEquipos_Ejercicio FOREIGN KEY (EjercicioId) REFERENCES Ejercicios(Id),
    CONSTRAINT FK_EjercicioEquipos_Equipo FOREIGN KEY (EquipoId) REFERENCES Equipos(Id),
    PRIMARY KEY (EjercicioId, EquipoId)
);

-- Tabla de Medidas Corporales
CREATE TABLE MedidasCorporales
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    FechaMedicion DATETIME NOT NULL DEFAULT GETDATE(),
    Peso DECIMAL(5, 2),
    Altura DECIMAL(5, 2),
    PorcentajeGrasa DECIMAL(5, 2),
    Edad INT,
    Genero NVARCHAR(10),
    EntrenadorCorreo VARCHAR(255),
    CONSTRAINT FK_MedidasCorporales_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico),
    CONSTRAINT FK_MedidasCorporales_Entrenador FOREIGN KEY (EntrenadorCorreo) REFERENCES Usuarios(CorreoElectronico)
);

-- Tabla de Citas de Medición Corporal
CREATE TABLE CitasMedicionCorporal
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    EntrenadorCorreo VARCHAR(255),
    FechaCita DATETIME NOT NULL,
    Estado NVARCHAR(20) NOT NULL,
    CONSTRAINT FK_CitasMedicionCorporal_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico),
    CONSTRAINT FK_CitasMedicionCorporal_Entrenador FOREIGN KEY (EntrenadorCorreo) REFERENCES Usuarios(CorreoElectronico)
);

-- Tabla de Rutinas
CREATE TABLE Rutinas
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    MedicionId INT,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Rutinas_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico),
    CONSTRAINT FK_Rutinas_Medicion FOREIGN KEY (MedicionId) REFERENCES MedidasCorporales(Id)
);

-- Tabla de Detalles de Rutinas
CREATE TABLE RutinaDetalles
(
    Id INT IDENTITY PRIMARY KEY,
    RutinaId INT,
    EjercicioId INT,
    Sets INT,
    Repeticiones INT,
    Tiempo INT,
    -- Solo para ejercicios basados en tiempo
    Peso DECIMAL(10, 2),
    -- Solo para ejercicios basados en peso
    CONSTRAINT FK_RutinaDetalles_Rutina FOREIGN KEY (RutinaId) REFERENCES Rutinas(Id),
    CONSTRAINT FK_RutinaDetalles_Ejercicio FOREIGN KEY (EjercicioId) REFERENCES Ejercicios(Id)
);

-- Tabla de Clases Grupales
CREATE TABLE ClasesGrupales
(
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    Cupo INT NOT NULL,
    Horario DATETIME NOT NULL,
    InstructorCorreo VARCHAR(255),
    CONSTRAINT FK_ClasesGrupales_Instructor FOREIGN KEY (InstructorCorreo) REFERENCES Usuarios(CorreoElectronico)
);

-- Tabla de Inscripciones a Clases Grupales
CREATE TABLE Inscripciones
(
    ClaseId INT,
    CorreoElectronico VARCHAR(255),
    FechaInscripcion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Inscripciones_Clase FOREIGN KEY (ClaseId) REFERENCES ClasesGrupales(Id),
    CONSTRAINT FK_Inscripciones_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico),
    PRIMARY KEY (ClaseId, CorreoElectronico)
);

-- Tabla de Pagos
CREATE TABLE Pagos
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    FechaPago DATETIME NOT NULL DEFAULT GETDATE(),
    Monto DECIMAL(10, 2) NOT NULL,
    MetodoPago NVARCHAR(50),
    EstadoPago NVARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    TransactionId NVARCHAR(255),
    CONSTRAINT FK_Pagos_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico)
);

-- Tabla de Descuentos
CREATE TABLE Descuentos
(
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    Porcentaje DECIMAL(5, 2) NOT NULL,
    FechaInicio DATETIME,
    FechaFin DATETIME
);

-- Tabla de Cupones
CREATE TABLE Cupones
(
    Id INT IDENTITY PRIMARY KEY,
    Codigo NVARCHAR(50) NOT NULL,
    DescuentoId INT,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    Usado BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Cupones_Descuento FOREIGN KEY (DescuentoId) REFERENCES Descuentos(Id)
);

-- Tabla de Entrenamientos
CREATE TABLE Entrenamientos
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    FechaEntrenamiento DATETIME NOT NULL DEFAULT GETDATE(),
    RutinaId INT,
    Observaciones NVARCHAR(255),
    CONSTRAINT FK_Entrenamientos_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico),
    CONSTRAINT FK_Entrenamientos_Rutina FOREIGN KEY (RutinaId) REFERENCES Rutinas(Id)
);

-- Tabla de Detalles de Entrenamientos
CREATE TABLE EntrenamientoDetalles
(
    Id INT IDENTITY PRIMARY KEY,
    EntrenamientoId INT,
    EjercicioId INT,
    Sets INT,
    Repeticiones INT,
    Tiempo INT,
    -- Solo para ejercicios basados en tiempo
    Peso DECIMAL(10, 2),
    -- Solo para ejercicios basados en peso
    CONSTRAINT FK_EntrenamientoDetalles_Entrenamiento FOREIGN KEY (EntrenamientoId) REFERENCES Entrenamientos(Id),
    CONSTRAINT FK_EntrenamientoDetalles_Ejercicio FOREIGN KEY (EjercicioId) REFERENCES Ejercicios(Id)
);

-- Tabla de Disponibilidad de Entrenadores
CREATE TABLE DisponibilidadEntrenadores
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    DiaSemana NVARCHAR(10),
    HoraInicio TIME,
    HoraFin TIME,
    CONSTRAINT FK_DisponibilidadEntrenadores_Entrenador FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico)
);

-- Tabla de Progreso del Usuario
CREATE TABLE ProgresoUsuario
(
    Id INT IDENTITY PRIMARY KEY,
    CorreoElectronico VARCHAR(255),
    FechaProgreso DATETIME NOT NULL DEFAULT GETDATE(),
    Peso DECIMAL(5, 2),
    MasaMuscular DECIMAL(5, 2),
    PorcentajeGrasa DECIMAL(5, 2),
    CONSTRAINT FK_ProgresoUsuario_Usuario FOREIGN KEY (CorreoElectronico) REFERENCES Usuarios(CorreoElectronico)
);
GO

-- Procedimiento para registrar usuario
CREATE PROCEDURE RegistrarUsuario
    @CorreoElectronico VARCHAR(255),
    @Nombre NVARCHAR(100),
    @Contrasena NVARCHAR(255),
    @Telefono NVARCHAR(20),
    @TipoUsuario NVARCHAR(50)
AS
BEGIN
    DECLARE @Salt VARBINARY(MAX) = NEWID();
    DECLARE @ContrasenaEncriptada VARBINARY(MAX) = HASHBYTES('SHA2_512', @Contrasena + CAST(@Salt AS NVARCHAR(MAX)));

    INSERT INTO Usuarios
        (CorreoElectronico, Nombre, Contrasena, FechaRegistro, Telefono, TipoUsuario, Estado, HaPagado, CorreoVerificado, TelefonoVerificado)
    VALUES
        (@CorreoElectronico, @Nombre, @ContrasenaEncriptada, GETDATE(), @Telefono, @TipoUsuario, 1, 0, 0, 0);

    INSERT INTO Salts
        (CorreoElectronico, Salt)
    VALUES
        (@CorreoElectronico, @Salt);
END;
GO



-- Procedimiento para validar usuario
CREATE PROCEDURE ValidarUsuario
    @CorreoElectronico VARCHAR(255),
    @Contrasena NVARCHAR(255)
AS
BEGIN
    DECLARE @Salt VARBINARY(MAX);
    DECLARE @ContrasenaEncriptada VARBINARY(MAX);

    SELECT @Salt = Salt
    FROM Salts
    WHERE CorreoElectronico = @CorreoElectronico;
    SET @ContrasenaEncriptada = HASHBYTES('SHA2_512', @Contrasena + CAST(@Salt AS NVARCHAR(MAX)));

    IF EXISTS (SELECT 1
    FROM Usuarios
    WHERE CorreoElectronico = @CorreoElectronico AND Contrasena = @ContrasenaEncriptada AND Estado = 1)
    BEGIN
        SELECT 'Inicio de sesión exitoso' AS Mensaje;
    END
    ELSE
    BEGIN
        SELECT 'Correo electrónico o contraseña incorrecta o usuario inactivo' AS Mensaje;
    END
END;
GO

-- Procedimiento para asignar rol a un usuario
CREATE PROCEDURE AsignarRol
    @CorreoElectronico VARCHAR(255),
    @RolId INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1
    FROM UsuarioRoles
    WHERE CorreoElectronico = @CorreoElectronico AND RolId = @RolId)
    BEGIN
        INSERT INTO UsuarioRoles
            (CorreoElectronico, RolId)
        VALUES
            (@CorreoElectronico, @RolId);
    END
    ELSE
    BEGIN
        PRINT 'El usuario ya tiene asignado este rol';
    END
END;
GO

-- Procedimiento para registrar pago
CREATE PROCEDURE RegistrarPago
    @CorreoElectronico VARCHAR(255),
    @Monto DECIMAL(10, 2),
    @MetodoPago NVARCHAR(50),
    @TransactionId NVARCHAR(255)
AS
BEGIN
    INSERT INTO Pagos
        (CorreoElectronico, Monto, MetodoPago, EstadoPago, TransactionId)
    VALUES
        (@CorreoElectronico, @Monto, @MetodoPago, 'Pendiente', @TransactionId);
END;
GO

-- Procedimiento para actualizar el estado del pago
CREATE PROCEDURE ActualizarEstadoPago
    @CorreoElectronico VARCHAR(255),
    @EstadoPago NVARCHAR(20)
AS
BEGIN
    UPDATE Pagos
    SET EstadoPago = @EstadoPago
    WHERE CorreoElectronico = @CorreoElectronico;
END;
GO

-- Procedimiento para cambiar el estado del usuario
CREATE PROCEDURE CambiarEstadoUsuario
    @CorreoElectronico VARCHAR(255),
    @Estado BIT
AS
BEGIN
    UPDATE Usuarios
    SET Estado = @Estado
    WHERE CorreoElectronico = @CorreoElectronico;
END;
GO

-- Procedimiento para aplicar cupón de descuento
CREATE PROCEDURE AplicarCupon
    @Codigo NVARCHAR(50),
    @CorreoElectronico VARCHAR(255)
AS
BEGIN
    DECLARE @DescuentoId INT;
    DECLARE @Porcentaje DECIMAL(5, 2);

    SELECT @DescuentoId = DescuentoId
    FROM Cupones
    WHERE Codigo = @Codigo AND Usado = 0;
    IF @DescuentoId IS NOT NULL
    BEGIN
        SELECT @Porcentaje = Porcentaje
        FROM Descuentos
        WHERE Id = @DescuentoId;

        -- Aplicar el descuento
        UPDATE Pagos
        SET Monto = Monto - (Monto * @Porcentaje / 100)
        WHERE CorreoElectronico = @CorreoElectronico AND EstadoPago = 'Pendiente';

        -- Marcar el cupón como usado
        UPDATE Cupones
        SET Usado = 1
        WHERE Codigo = @Codigo;

        SELECT 'Cupón aplicado con éxito' AS Mensaje;
    END
    ELSE
    BEGIN
        SELECT 'Cupón no válido o ya utilizado' AS Mensaje;
    END
END;
GO

-- Trigger para verificar la complejidad de la contraseña
CREATE TRIGGER trg_VerificarContrasenaCompleja
ON Usuarios
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @CorreoElectronico VARCHAR(255);
    DECLARE @Nombre NVARCHAR(100);
    DECLARE @Contrasena VARBINARY(MAX);
    DECLARE @Salt VARBINARY(MAX);
    DECLARE @Telefono NVARCHAR(20);
    DECLARE @TipoUsuario NVARCHAR(50);

    SELECT @CorreoElectronico = CorreoElectronico, @Nombre = Nombre, @Contrasena = Contrasena,
        @Salt = Salt, @Telefono = Telefono, @TipoUsuario = TipoUsuario
    FROM inserted;

    IF (LEN(CAST(@Contrasena AS NVARCHAR(MAX))) >= 8 AND
        PATINDEX('%[0-9]%', CAST(@Contrasena AS NVARCHAR(MAX))) > 0 AND
        PATINDEX('%[a-zA-Z]%', CAST(@Contrasena AS NVARCHAR(MAX))) > 0 AND
        PATINDEX('%[^a-zA-Z0-9]%', CAST(@Contrasena AS NVARCHAR(MAX))) > 0)
    BEGIN
        INSERT INTO Usuarios
            (CorreoElectronico, Nombre, Contrasena, FechaRegistro, Telefono, TipoUsuario, Estado, HaPagado, CorreoVerificado, TelefonoVerificado)
        VALUES
            (@CorreoElectronico, @Nombre, @Contrasena, GETDATE(), @Telefono, @TipoUsuario, 1, 0, 0, 0);

        INSERT INTO Salts
            (CorreoElectronico, Salt)
        VALUES
            (@CorreoElectronico, @Salt);
    END
    ELSE
    BEGIN
        RAISERROR('La contraseña no cumple con los requisitos de complejidad', 16, 1);
    END
END;
GO

-- Procedimiento para eliminar equipo
CREATE PROCEDURE sp_EliminarEquipo
    @Id INT
AS
BEGIN
    DELETE FROM Equipos
    WHERE Id = @Id;
END;
GO

-- Procedimiento para actualizar equipo
CREATE PROCEDURE sp_ActualizarEquipo
    @Id INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @GrupoMuscular NVARCHAR(100),
    @Cantidad INT,
    @Disponibilidad BIT
AS
BEGIN
    UPDATE Equipos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        GrupoMuscular = @GrupoMuscular,
        Cantidad = @Cantidad,
        Disponibilidad = @Disponibilidad
    WHERE Id = @Id;
END;
GO

-- Procedimiento para obtener todos los equipos
CREATE PROCEDURE sp_ObtenerTodosLosEquipos
AS
BEGIN
    SELECT *
    FROM dbo.Equipos WITH(NOLOCK);
END;
GO

-- Procedimiento para crear equipo
CREATE PROCEDURE sp_CrearEquipo
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @GrupoMuscular NVARCHAR(100),
    @Cantidad INT,
    @Disponibilidad BIT
AS
BEGIN
    INSERT INTO Equipos
        (Nombre, Descripcion, GrupoMuscular, Cantidad, Disponibilidad)
    VALUES
        (@Nombre, @Descripcion, @GrupoMuscular, @Cantidad, @Disponibilidad);
END;
GO

CREATE PROCEDURE dbo.sp_ObtenerEquipoPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion, GrupoMuscular, Cantidad, Disponibilidad
    FROM Equipos
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE dbo.sp_RegistrarEjercicio
    @Nombre NVARCHAR(100),
    @Tipo NVARCHAR(50),
    @Descripcion NVARCHAR(255),
    @Peso DECIMAL(10,2),
    @Tiempo INT,
    @AMRAP BIT
AS
BEGIN
    INSERT INTO Ejercicios
        (Nombre, Tipo, Descripcion, Peso, Tiempo, AMRAP)
    VALUES
        (@Nombre, @Tipo, @Descripcion, @Peso, @Tiempo, @AMRAP);
END;
GO

CREATE PROCEDURE dbo.sp_EliminarEjercicio
    @Id INT
AS
BEGIN
    DELETE FROM Ejercicios
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE dbo.sp_ObtenerTodosLosEjercicios
AS
BEGIN
    SELECT *
    FROM dbo.Ejercicios WITH(NOLOCK);
END;
GO

CREATE PROCEDURE dbo.sp_ObtenerEjercicioPorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Tipo, Descripcion, Peso, Tiempo, AMRAP
    FROM Ejercicios
    WHERE Id = @Id;
END;
GO

--Procedimiento para actualizar ejercicios
CREATE PROCEDURE dbo.sp_ActualizarEjercicio
    @Id INT,
    @Nombre NVARCHAR(100),
    @Tipo NVARCHAR(50),
    @Descripcion NVARCHAR(255),
    @Peso DECIMAL(10, 2),
    @Tiempo INT,
    @AMRAP BIT
AS
BEGIN
    UPDATE Ejercicios
    SET 
        Nombre = @Nombre,
        Tipo = @Tipo,
        Descripcion = @Descripcion,
        Peso = @Peso,
        Tiempo = @Tiempo,
        AMRAP = @AMRAP
    WHERE Id = @Id;
END;
GO





