SET NOCOUNT ON;

INSERT INTO States (Type, Name) VALUES
('USER', 'Activo'),
('USER', 'Inactivo'),
('USER', 'Bloqueado'),
('OPERATOR', 'Activo'),
('OPERATOR', 'Inactivo'),
('ADMINISTRATOR', 'Activo'),
('ADMINISTRATOR', 'Inactivo'),
('COURSE', 'Activo'),
('COURSE', 'Inactivo'),
('GROUP', 'Activo'),
('GROUP', 'Inactivo'),
('LABORATORY', 'Activo'),
('LABORATORY', 'Inactivo'),
('SOFTWARE', 'Activo'),
('SOFTWARE', 'Inactivo'),
('APPOINTMENT', 'Por iniciar'),
('APPOINTMENT', 'Cancelada'),
('APPOINTMENT', 'Finalizada'),
('RESERVATION', 'Por iniciar'),
('RESERVATION', 'Cancelada'),
('RESERVATION', 'Finalizada');