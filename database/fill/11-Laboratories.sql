USE [SiLabI];
SET NOCOUNT ON;
DECLARE @LAB_laboratory_state INT;
SELECT @LAB_laboratory_state = PK_State_Id FROM States WHERE Type = 'LABORATORY' AND Name = 'Activo';

SET IDENTITY_INSERT Laboratories ON;

INSERT INTO Laboratories (PK_Laboratory_Id, Name, Seats, FK_State_Id) VALUES
(1, 'Laboratorio A', 20, @LAB_laboratory_state),
(2, 'Laboratorio B', 20, @LAB_laboratory_state),
(3, 'Laboratorio C', 25, @LAB_laboratory_state),
(4, 'Laboratorio D', 25, @LAB_laboratory_state),
(5, 'Laboratorio E', 30, @LAB_laboratory_state);

SET IDENTITY_INSERT Laboratories OFF;
USE [master];