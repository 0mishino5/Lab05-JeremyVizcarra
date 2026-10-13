-- Creación de la tabla de usuarios para la autenticación JWT
CREATE TABLE Usuarios (
    id_usuario SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(200) NOT NULL,
    rol VARCHAR(20) NOT NULL CHECK (rol IN ('Admin', 'Docente', 'Estudiante')),
    correo VARCHAR(100),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);
