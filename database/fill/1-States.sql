USE [SiLabI];
SET NOCOUNT ON;
INSERT INTO States (Type, Name) VALUES
('USER', 'active'),
('USER', 'disabled'),
('USER', 'blocked'),
('OPERATOR', 'active'),
('OPERATOR', 'disabled'),
('ADMINISTRATOR', 'active'),
('ADMINISTRATOR', 'disabled'),
('COURSE', 'active'),
('COURSE', 'disabled'),
('GROUP', 'active'),
('GROUP', 'disabled'),
('LABORATORY', 'active'),
('LABORATORY', 'disabled'),
('SOFTWARE', 'active'),
('SOFTWARE', 'disabled'),
('APPOINTMENT', 'before_start'),
('APPOINTMENT', 'in_progress'),
('APPOINTMENT', 'canceled'),
('APPOINTMENT', 'finished'),
('RESERVATION', 'before_start'),
('RESERVATION', 'in_progress'),
('RESERVATION', 'canceled'),
('RESERVATION', 'finished');

USE [master];