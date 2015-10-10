USE [SiLabI];
SET NOCOUNT ON;
DECLARE @APPOINTMENT_state INT, @APPOINTMENT_student INT, @APPOINTMENT_laboratory INT, @APPOINTMENT_software INT;
DECLARE @APPOINTMENT_date DATETIME;
DECLARE @APPOINTMENT_group INT, @APPOINTMENT_i INT, @APPOINTMENT_rows INT;
SELECT @APPOINTMENT_state = PK_State_Id FROM States WHERE Type = 'APPOINTMENT' AND Name = 'Por iniciar';

SET @APPOINTMENT_i = 0;
SET @APPOINTMENT_rows = 200;

WHILE @APPOINTMENT_i < @APPOINTMENT_rows
BEGIN
	SELECT TOP 1 @APPOINTMENT_student = FK_User_Id FROM Students ORDER BY NEWID();
	SELECT TOP 1 @APPOINTMENT_group = FK_Group_Id FROM StudentsByGroup WHERE FK_Student_Id = @APPOINTMENT_student ORDER BY NEWID();
	SELECT TOP 1 @APPOINTMENT_software = PK_Software_Id FROM Software ORDER BY NEWID();
	SELECT TOP 1 @APPOINTMENT_date = [date], @APPOINTMENT_laboratory = [laboratory.id] FROM fn_GetAvailableAppointments(@APPOINTMENT_student) ORDER BY NEWID();

	IF NOT EXISTS (SELECT 1 FROM Appointments WHERE FK_Laboratory_Id = @APPOINTMENT_laboratory AND FK_Software_Id = @APPOINTMENT_software AND FK_Student_Id = @APPOINTMENT_student)
	BEGIN
		INSERT INTO Appointments(Date, FK_State_Id, FK_Student_Id, FK_Laboratory_Id, FK_Software_Id, FK_Group_Id) VALUES
		(@APPOINTMENT_date, @APPOINTMENT_state, @APPOINTMENT_student, @APPOINTMENT_laboratory, @APPOINTMENT_software, @APPOINTMENT_group)

		SET @APPOINTMENT_i = @APPOINTMENT_i + 1;
	END
END

USE [master];