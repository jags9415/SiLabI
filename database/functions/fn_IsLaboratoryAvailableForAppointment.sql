IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsLaboratoryAvailableForAppointment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_IsLaboratoryAvailableForAppointment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Check if a laboratory if available for an appointment.
-- =============================================
CREATE FUNCTION [dbo].[fn_IsLaboratoryAvailableForAppointment]
(
	@appointmentId	INT,
	@laboratoryId	INT,
	@date			DATETIMEOFFSET(3)
)
RETURNS BIT
AS
BEGIN
	DECLARE @currentDate DATETIMEOFFSET(3), @currentLaboratoryId INT;
	
	-- Get the appointment date and laboratory.
	SELECT	@currentDate = [Date],
			@currentLaboratoryId = [FK_Laboratory_Id]
	FROM Appointments WHERE PK_Appointment_Id = @appointmentId;
	
	-- If the appointment data is equal to the given paramaters return true.
	IF @currentDate = @date AND @currentLaboratoryId = @laboratoryId
		RETURN 1
		
	-- If there is not available seats return false.
	IF dbo.fn_GetLaboratoryAvailableSeats(@laboratoryId, @date) = 0
		RETURN 0;
		
	-- If labotatory is reserved return false.
	IF dbo.fn_IsLaboratoryReserved(@laboratoryId, @date, DATEADD(HOUR, 1, @date)) = 1
		RETURN 0;
		
	-- Else return true.
	RETURN 1;
END
GO


