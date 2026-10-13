-- Insertar datos en la tabla de estudiantes
INSERT INTO Estudiantes (nombre, edad, direccion, telefono, correo) 
VALUES 
('Juan Pérez', 20, 'Calle Ficticia 123', '987654321', 'juan.perez@mail.com'),
('Ana Gómez', 22, 'Avenida Siempre Viva 456', '998877665', 'ana.gomez@mail.com');

-- Insertar datos en la tabla de cursos
INSERT INTO Cursos (nombre, descripcion, creditos) 
VALUES 
('Matemáticas I', 'Curso introductorio a las matemáticas', 4),
('Historia Universal', 'Historia de los principales eventos a nivel mundial', 3);

-- Insertar datos en la tabla de profesores
INSERT INTO Profesores (nombre, especialidad, correo) 
VALUES 
('Carlos Ramírez', 'Matemáticas', 'carlos.ramirez@mail.com'),
('Marta Díaz', 'Historia', 'marta.diaz@mail.com');

-- Insertar datos en la tabla de matrículas
INSERT INTO Matriculas (id_estudiante, id_curso, semestre) 
VALUES 
(1, 1, '2025-1'),
(2, 2, '2025-1');

-- Insertar datos en la tabla de evaluaciones
INSERT INTO Evaluaciones (id_estudiante, id_curso, calificacion, fecha) 
VALUES 
(1, 1, 18.5, '2025-03-10'),
(2, 2, 15.0, '2025-03-12');

-- Insertar datos en la tabla de asistencias
INSERT INTO Asistencias (id_estudiante, id_curso, fecha, estado) 
VALUES 
(1, 1, '2025-03-10', 'Presente'),
(2, 2, '2025-03-12', 'Ausente');

-- Insertar datos en la tabla de materias
INSERT INTO Materias (id_curso, nombre, descripcion) 
VALUES 
(1, 'Álgebra', 'Estudio de las estructuras algebraicas'),
(2, 'Geografía', 'Estudio de las formaciones geográficas del mundo');