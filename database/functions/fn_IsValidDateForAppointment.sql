IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsValidDateForAppointment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_IsValidDateForAppointment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Check if a date is valid for an appointment update.
-- =============================================
CREATE FUNCTION [dbo].[fn_IsValidDateForAppointment]
(
	@appointmentId	INT,				-- The appointment identity.
	@date			DATETIMEOFFSET(3)	-- The date.
)
RETURNS BIT
AS
BEGIN
	DECLARE @currentDate DATETIMEOFFSET(3);
	SELECT @currentDate = [Date] FROM Appointments WHERE PK_Appointment_Id = @appointmentId;
	
	IF @currentDate = @date
		RETURN 1;
	
	IF @date < SYSDATETIMEOFFSET()
		RETURN 0;
		
	RETURN 1;
END
GO