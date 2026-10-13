-- Usuarios de prueba. Las contrasenas estan encriptadas con PBKDF2-HMAC-SHA256,
-- 100000 iteraciones, sal aleatoria de 16 bytes. Formato: iteraciones.sal.hash
-- admin / Admin123*   |   docente / Docente123*   |   estudiante / Estudiante123*
INSERT INTO Usuarios (username, password_hash, rol, correo, activo)
VALUES 
('admin', '100000.eTjL1ZKhuCZ2GLXt89NMyA==.JDslkxsXZ7NkM+ADHaX/VVaTzwLwisGxkX6cGBvhSZ4=', 'Admin', 'admin@tecsup.edu.pe', TRUE),
('docente', '100000.5LearaivvwI19edbguA+tQ==.zOmzsPnj/YhtxlFZ7O6ScySvMUg3xQVg0+7qAZOxQ0U=', 'Docente', 'carlos.ramirez@mail.com', TRUE),
('estudiante', '100000.GTNkB3bBl5r3RFeU3U7hIQ==.y8tSvg1u4NABiKgkd4WKTz37Fke4b0gUyrNAd5mX0ms=', 'Estudiante', 'juan.perez@mail.com', TRUE);
