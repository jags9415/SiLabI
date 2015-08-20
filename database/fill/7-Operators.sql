USE [SiLabI];
SET NOCOUNT ON;
DECLARE @OPERATOR_user_id INT, @OPERATOR_period_id INT, @OPERATOR_operator_state INT, @OPERATOR_i INT, @OPERATOR_rows INT;
SELECT @OPERATOR_operator_state = PK_State_Id FROM States WHERE Type = 'OPERATOR' AND Name = 'ACTIVE';

SET @OPERATOR_i = 0;
SET @OPERATOR_rows = 100;

WHILE @OPERATOR_i < @OPERATOR_rows
BEGIN
	SELECT TOP 1 @OPERATOR_user_id = FK_User_Id FROM Students ORDER BY NEWID();
	
	SELECT TOP 1 @OPERATOR_period_id = PK_Period_Id FROM Periods AS P
	INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
	WHERE PT.Name = 'Semestre'
	ORDER BY NEWID();

	IF NOT EXISTS (SELECT * FROM Operators WHERE FK_User_Id = @OPERATOR_user_id AND FK_Period_Id = @OPERATOR_period_id)
	BEGIN
		INSERT INTO Operators(FK_User_Id, FK_Period_Id, FK_State_Id) VALUES
		(@OPERATOR_user_id, @OPERATOR_period_id, @OPERATOR_operator_state);
		
		SET @OPERATOR_i = @OPERATOR_i + 1;
	END
END

USE [master];