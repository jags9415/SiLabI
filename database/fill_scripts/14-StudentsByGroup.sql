SET NOCOUNT ON;

DECLARE @SBG_user_id INT, @SBG_group_id INT, @SBG_i INT, @SBG_rows INT;

SET @SBG_i = 0;
SET @SBG_rows = 5000;

WHILE @SBG_i < @SBG_rows
BEGIN
	SELECT TOP 1 @SBG_user_id = FK_User_Id FROM Students ORDER BY NEWID();
	SELECT TOP 1 @SBG_group_id = PK_Group_Id FROM Groups ORDER BY NEWID();

	IF NOT EXISTS (SELECT 1 FROM StudentsByGroup WHERE FK_Student_Id = @SBG_user_id AND FK_Group_Id = @SBG_group_id)
	BEGIN
		INSERT INTO StudentsByGroup(FK_Student_Id, FK_Group_Id) VALUES
		(@SBG_user_id, @SBG_group_id);

		SET @SBG_i = @SBG_i + 1;
	END
END