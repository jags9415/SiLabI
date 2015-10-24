IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetAppointmentsBetween]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetAppointmentsBetween]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the number of appointments in a range of time.
-- =============================================
CREATE FUNCTION [dbo].[fn_GetAppointmentsBetween]
(
	@laboratory INT,				-- The laboratory identity.
	@start_date DATETIMEOFFSET(3),	-- The start datetime.
	@end_date DATETIMEOFFSET(3)		-- The end datetime.
)
RETURNS INT
AS
BEGIN
	DECLARE @count INT;
	
	SELECT @count = COUNT(1) FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
	WHERE S.Name <> 'Cancelada' AND A.Date >= @start_date AND A.Date < @end_date
	
	RETURN @count
END
GO


