SET NOCOUNT ON;

DECLARE @RESERVATION_state INT, @RESERVATION_professor INT, @RESERVATION_laboratory INT, @RESERVATION_software INT;
DECLARE @RESERVATION_group INT, @RESERVATION_i INT, @RESERVATION_rows INT;
DECLARE @RESERVATION_start_date DATETIMEOFFSET(3), @RESERVATION_end_date DATETIMEOFFSET(3);
SELECT @RESERVATION_state = PK_State_Id FROM States WHERE Type = 'RESERVATION' AND Name = 'Por iniciar';

SET @RESERVATION_i = 0;
SET @RESERVATION_rows = 50;

WHILE @RESERVATION_i < @RESERVATION_rows
BEGIN
	SELECT TOP 1 @RESERVATION_professor = FK_User_Id FROM Professors ORDER BY NEWID();
	SELECT TOP 1 @RESERVATION_laboratory = PK_Laboratory_Id FROM Laboratories WHERE Name = 'Laboratorio A';
	SELECT TOP 1 @RESERVATION_software = PK_Software_Id FROM Software ORDER BY NEWID();
	
	SELECT TOP 1 @RESERVATION_start_date = [date], @RESERVATION_end_date = DATEADD(HOUR, 1, [date])
	FROM dbo.fn_GetDateTimeRange(DATEADD(WEEK, 2, SYSDATETIMEOFFSET()), 4)
	WHERE dbo.fn_IsLaboratoryReserved(@RESERVATION_laboratory, [date], DATEADD(HOUR, 1, [date])) = 0
	AND dbo.fn_GetAppointmentsBetween(@RESERVATION_laboratory, [date], DATEADD(HOUR, 1, [date])) = 0
	ORDER BY NEWID();
	
	SELECT TOP 1 @RESERVATION_group = G.PK_Group_Id
	FROM Groups AS G
	INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
	INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
	WHERE FK_Professor_Id = @RESERVATION_professor AND dbo.fn_IsDateInPeriod(@RESERVATION_start_date, P.Value, PT.Name, G.Period_Year) = 1
	ORDER BY NEWID();
	
	IF NOT EXISTS (SELECT 1 FROM Reservations WHERE FK_Laboratory_Id = @RESERVATION_laboratory AND FK_Software_Id = @RESERVATION_software AND FK_Professor_Id = @RESERVATION_professor)
	BEGIN
		INSERT INTO Reservations(Start_Time, End_Time, FK_State_Id, FK_Professor_Id, FK_Laboratory_Id, FK_Software_Id, FK_Group_Id) VALUES
		(@RESERVATION_start_date, @RESERVATION_end_date, @RESERVATION_state, @RESERVATION_professor, @RESERVATION_laboratory, @RESERVATION_software, @RESERVATION_group)

		SET @RESERVATION_i = @RESERVATION_i + 1;
	END
END