IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetStudentWeeklyAppointmentsCount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetStudentWeeklyAppointmentsCount]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the amount of appointments made from a student in a week.
-- =============================================
CREATE FUNCTION [dbo].[fn_GetStudentWeeklyAppointmentsCount]
(
	@id		INT,				-- The student identity.
	@date	DATETIMEOFFSET(3)	-- A datetime.
)
RETURNS INT
AS
BEGIN
	DECLARE @count INT;
	
	SELECT @count = COUNT(1) FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
	WHERE A.FK_Student_Id = @id AND S.Name <> 'Cancelada' AND
	DATEDIFF(WEEK, A.Date, @date) = 0 AND DATEDIFF(YEAR, A.Date, @date) = 0
	
	RETURN @count
END

GO


