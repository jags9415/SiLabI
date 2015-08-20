USE [SiLabI];
SET NOCOUNT ON;
DECLARE @SBL_labs_count INT, @SBL_software_count INT, @SBL_i INT, @SBL_j INT;

SET @SBL_i = 1;
SET @SBL_j = 1;

SET @SBL_labs_count = 5;
SET @SBL_software_count = 150;

WHILE @SBL_i <= @SBL_labs_count
BEGIN
	WHILE @SBL_j <= @SBL_software_count
	BEGIN
		INSERT INTO SoftwareByLaboratory(FK_Laboratory_Id, FK_Software_Id) VALUES
		(@SBL_i, @SBL_j);
		SET @SBL_j = @SBL_j + 1;
	END
	SET @SBL_i = @SBL_i + 1;
	SET @SBL_j = 1;
END

USE [master];