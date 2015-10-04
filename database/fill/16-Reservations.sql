USE [SiLabI];
SET NOCOUNT ON;
DECLARE @RESERVATION_state INT, @RESERVATION_professor INT, @RESERVATION_laboratory INT, @RESERVATION_software INT;
DECLARE @RESERVATION_group INT, @RESERVATION_i INT, @RESERVATION_rows INT;
SELECT @RESERVATION_state = PK_State_Id FROM States WHERE Type = 'RESERVATION' AND Name = 'Por iniciar';

SET @RESERVATION_i = 0;
SET @RESERVATION_rows = 200;

WHILE @RESERVATION_i < @RESERVATION_rows
BEGIN
	SELECT TOP 1 @RESERVATION_professor = FK_User_Id FROM Professors ORDER BY NEWID();
	SELECT TOP 1 @RESERVATION_laboratory = PK_Laboratory_Id FROM Laboratories WHERE Name = 'Laboratorio A';
	SELECT TOP 1 @RESERVATION_software = PK_Software_Id FROM Software ORDER BY NEWID();
	SELECT TOP 1 @RESERVATION_group = PK_Group_Id FROM Groups WHERE FK_Professor_Id = @RESERVATION_professor ORDER BY NEWID();

	IF NOT EXISTS (SELECT 1 FROM Reservations WHERE FK_Laboratory_Id = @RESERVATION_laboratory AND FK_Software_Id = @RESERVATION_software AND FK_Professor_Id = @RESERVATION_professor)
	BEGIN
		INSERT INTO Reservations(Start_Time, End_Time, FK_State_Id, FK_Professor_Id, FK_Laboratory_Id, FK_Software_Id, FK_Group_Id) VALUES
		('2015-09-22T09:00:00.000Z', '2015-09-22T10:00:00.000Z', @RESERVATION_state, @RESERVATION_professor, @RESERVATION_laboratory, @RESERVATION_software, @RESERVATION_group)

		SET @RESERVATION_i = @RESERVATION_i + 1;
	END
END

USE [master];