USE [SiLabI];
SET NOCOUNT ON;
DECLARE @PROFESSOR_i INT, @PROFESSOR_rows INT;

SET @PROFESSOR_i = 801;
SET @PROFESSOR_rows = 1000;

WHILE @PROFESSOR_i <= @PROFESSOR_rows
BEGIN
	INSERT INTO Professors (FK_User_Id) VALUES (@PROFESSOR_i);
	SET @PROFESSOR_i = @PROFESSOR_i + 1;
END

USE [master];