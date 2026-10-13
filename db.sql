-- Creación de la tabla de estudiantes
CREATE TABLE Estudiantes (
    id_estudiante SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    edad INT NOT NULL,
    direccion VARCHAR(255),
    telefono VARCHAR(20),
    correo VARCHAR(100)
);

-- Creación de la tabla de cursos
CREATE TABLE Cursos (
    id_curso SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    creditos INT NOT NULL
);

-- Creación de la tabla de profesores
CREATE TABLE Profesores (
    id_profesor SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    especialidad VARCHAR(100),
    correo VARCHAR(100)
);

-- Creación de la tabla de matrículas
CREATE TABLE Matriculas (
    id_matricula SERIAL PRIMARY KEY,
    id_estudiante INT REFERENCES Estudiantes(id_estudiante),
    id_curso INT REFERENCES Cursos(id_curso),
    semestre VARCHAR(20)
);

-- Creación de la tabla de evaluaciones
CREATE TABLE Evaluaciones (
    id_evaluacion SERIAL PRIMARY KEY,
    id_estudiante INT REFERENCES Estudiantes(id_estudiante),
    id_curso INT REFERENCES Cursos(id_curso),
    calificacion DECIMAL(5, 2),
    fecha DATE
);

-- Creación de la tabla de asistencias
CREATE TABLE Asistencias (
    id_asistencia SERIAL PRIMARY KEY,
    id_estudiante INT REFERENCES Estudiantes(id_estudiante),
    id_curso INT REFERENCES Cursos(id_curso),
    fecha DATE,
    estado VARCHAR(20) CHECK (estado IN ('Presente', 'Ausente', 'Justificada'))
);

-- Creación de la tabla de materias
CREATE TABLE Materias (
    id_materia SERIAL PRIMARY KEY,
    id_curso INT REFERENCES Cursos(id_curso),
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT
);