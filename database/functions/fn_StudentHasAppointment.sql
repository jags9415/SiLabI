IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_StudentHasAppointment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_StudentHasAppointment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Check if a student has an appointment at a specific time.
-- =============================================
CREATE FUNCTION [dbo].[fn_StudentHasAppointment]
(
  @studentId	INT,				-- The student identity.
  @date			DATETIMEOFFSET(3)	-- The datetime.
)
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT;
	
	IF EXISTS
	(
		SELECT 1 FROM Appointments AS A 
		INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
		WHERE A.FK_Student_Id = @studentId AND A.[Date] = @date AND S.Name <> 'Cancelada'
	)
		SET @result = 1;
	ELSE
		SET @result = 0
		
	RETURN @result;
END

GO


