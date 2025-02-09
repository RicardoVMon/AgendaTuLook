-- CREATE DATABASE AgendaTuLook
USE AgendaTuLook

-- Tabla Rol
CREATE TABLE tRoles (
    RolId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
);

INSERT INTO tRoles (Nombre) VALUES ('Administrador');
INSERT INTO tRoles (Nombre) VALUES ('Cliente');

-- Tabla Usuarios
CREATE TABLE tUsuarios (
    UsuarioId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    Contrasennia VARCHAR(255) NULL,  -- Contraseña opcional para usuarios tradicionales
    GoogleId VARCHAR(255) NULL,  -- ID único proporcionado por Google para autenticación
    Telefono VARCHAR(15) NULL,  -- Opcional
    RolId BIGINT NOT NULL,  -- Relación con tRol para definir si es cliente o admin
    FechaRegistro DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_tUsuarios_Rol FOREIGN KEY (RolId) REFERENCES tRoles(RolId)
);

-- Tabla Notificaciones

CREATE TABLE tNotificaciones (
    NotificacionId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId BIGINT NOT NULL,
    Mensaje VARCHAR(255) NOT NULL,
    FechaEnvio DATETIME DEFAULT GETDATE(),
    Leido BIT DEFAULT 0,  -- Para saber si el usuario ha leído la notificación
    CONSTRAINT FK_tNotificaciones_Usuario FOREIGN KEY (UsuarioId) REFERENCES tUsuarios(UsuarioId)
);

-- Tabla Servicios
CREATE TABLE tServicios (
    ServicioId BIGINT IDENTITY(1,1) PRIMARY KEY,
    NombreServicio VARCHAR(100) NOT NULL,
    Descripcion TEXT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    Duracion INT NOT NULL, -- En minutos
	Imagen VARCHAR(255) NULL
);

CREATE TABLE tCitas (
    CitaId BIGINT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId BIGINT NOT NULL, 
    ServicioId BIGINT NOT NULL,  
    FechaInicio DATETIME NOT NULL,  
    FechaFin DATETIME NOT NULL,  
    Estado VARCHAR(20) CHECK (estado IN ('Finalizada', 'Confirmada', 'Cancelada')) NOT NULL,
    CONSTRAINT FK_tCitas_Usuario FOREIGN KEY (UsuarioId) REFERENCES tUsuarios(UsuarioId),
    CONSTRAINT FK_tCitas_Servicio FOREIGN KEY (ServicioId) REFERENCES tServicios(ServicioId)
);

CREATE TABLE tCalificaciones (
    CalificacionId BIGINT IDENTITY(1,1) PRIMARY KEY,
    CitaId BIGINT NOT NULL,  
    Puntuacion INT CHECK (puntuacion BETWEEN 1 AND 5) NOT NULL,  
    Comentario TEXT NULL,  
    Fecha DATETIME DEFAULT GETDATE(),  
    CONSTRAINT FK_tCalificaiones_Cita FOREIGN KEY (CitaId) REFERENCES tCitas(CitaId)
);

CREATE TABLE tMetodosPago (
    MetodoPagoId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE, 
    Estado BIT DEFAULT 1 
);


CREATE TABLE tPagos (
    PagoId BIGINT IDENTITY(1,1) PRIMARY KEY,
    CitaId BIGINT NOT NULL,  
    MetodoPagoId BIGINT NOT NULL,  
    Monto DECIMAL(10, 2) NOT NULL,  
    FechaPago DATETIME DEFAULT GETDATE(),
    TransaccionId VARCHAR(255) NULL,  -- ID de la transacción (para PayPal u otros)
    EstadoPago VARCHAR(50) NULL,  -- Estado del pago (Ej. 'Completado', 'Pendiente')
    CONSTRAINT FK_tPagos_Cita FOREIGN KEY (CitaId) REFERENCES tCitas(CitaId),
    CONSTRAINT FK_tPagos_MetodoPago FOREIGN KEY (MetodoPagoId) REFERENCES tMetodosPago(MetodoPagoId)
);

CREATE TABLE tExcepciones (
    ExcepcionId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Mensaje TEXT NOT NULL, 
    Fecha DATETIME DEFAULT GETDATE(),
    Origen VARCHAR(50) NOT NULL,
	UsuarioId BIGINT NULL
	CONSTRAINT FK_tExcepciones_Usuarios FOREIGN KEY (UsuarioId) REFERENCES tUsuarios(UsuarioId)
);
