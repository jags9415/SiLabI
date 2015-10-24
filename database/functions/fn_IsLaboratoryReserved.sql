IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsLaboratoryReserved]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_IsLaboratoryReserved]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Check if a laboratory is reserved between a range of time.
-- =============================================
CREATE FUNCTION [dbo].[fn_IsLaboratoryReserved]
(
	@id				INT,				-- The laboratory identity.
	@start_time		DATETIMEOFFSET(3),	-- The start datetime.
	@end_time		DATETIMEOFFSET(3)	-- The end datetime.
)
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT = 0;
	
	IF EXISTS
	(
		SELECT 1 FROM Reservations AS R
		INNER JOIN States AS S ON R.FK_State_Id = S.PK_State_Id
		WHERE R.FK_Laboratory_Id = @id AND S.Name <> 'Cancelada' AND
		  (
			((R.Start_Time BETWEEN @start_time AND @end_time) AND R.Start_Time <> @end_time)
			OR 
			((R.End_Time BETWEEN @start_time AND @end_time) AND R.End_Time <> @start_time)
		  )
	)
	BEGIN
		SET @result = 1
	END

	RETURN @result
END
GO


