USE [SiLabI];
DECLARE @GROUP_course_id INT, @GROUP_professor_id INT, @GROUP_period_id INT, @GROUP_i INT, @GROUP_rows INT;

SET @GROUP_i = 0;
SET @GROUP_rows = 200;

WHILE @GROUP_i < @GROUP_rows
BEGIN
	SELECT TOP 1 @GROUP_course_id = PK_Course_Id FROM Courses ORDER BY NEWID();
	
	SELECT TOP 1 @GROUP_professor_id = FK_User_Id FROM Professors ORDER BY NEWID();
	
	SELECT TOP 1 @GROUP_period_id = PK_Period_Id FROM Periods AS P
	INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
	WHERE PT.Name = 'Semestre'
	ORDER BY NEWID();

	INSERT INTO Groups(FK_Course_Id, FK_Professor_Id, FK_Period_Id, Number) VALUES
	(@GROUP_course_id, @GROUP_professor_id, @GROUP_period_id, @GROUP_i + 1);

	SET @GROUP_i = @GROUP_i + 1;
END

USE [master];