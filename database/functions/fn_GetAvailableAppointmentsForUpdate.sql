IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetAvailableAppointmentsForUpdate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetAvailableAppointmentsForUpdate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the available dates and laboratories for updating an existing appointment.
-- =============================================
CREATE FUNCTION [dbo].[fn_GetAvailableAppointmentsForUpdate]
(
	@appointmentId INT	-- The appointment identity.
)
RETURNS @table TABLE
(
	[date] DATETIMEOFFSET(3),
	[spaces] INT,
	[laboratory.id] INT,
	[laboratory.name] VARCHAR(500),
	[labotatory.seats] INT,
	[labotatory.state] VARCHAR(30),
	[labotatory.created_at] DATETIMEOFFSET(3),
	[labotatory.updated_at] DATETIMEOFFSET(3),
	[labotatory.appointment_priority] INT,
	[labotatory.reservation_priority] INT
)
AS
BEGIN
	DECLARE @date DATETIMEOFFSET(3);
	SELECT @date = [Date] FROM Appointments WHERE PK_Appointment_Id = @appointmentId;
	
	INSERT @table
	SELECT [date], [spaces], [laboratory.id], [laboratory.name], [laboratory.seats], [laboratory.state],
		   [laboratory.created_at], [laboratory.updated_at], [laboratory.appointment_priority], [laboratory.reservation_priority]
	FROM
	(
		SELECT ROW_NUMBER() OVER ( PARTITION BY [Date] ORDER BY [laboratory.appointment_priority]) RN, *  
		FROM
		(
			SELECT R.[Date] AS [date], dbo.fn_GetLaboratoryAvailableSeats(L.PK_Laboratory_Id, R.Date) AS [spaces],
			L.PK_Laboratory_Id AS [laboratory.id], L.Name AS [laboratory.name], L.Seats AS [laboratory.seats], 
			S.Name AS [laboratory.state], L.Created_At AS [laboratory.created_at], 
			L.Updated_At AS [laboratory.updated_at], L.Appointment_Priority AS [laboratory.appointment_priority], 
            L.Reservation_Priority AS [laboratory.reservation_priority]
			FROM dbo.fn_GetDateTimeRange(DATEADD(HOUR, -1, @date), 2) AS R, Laboratories AS L
			INNER JOIN Appointments AS A ON A.PK_Appointment_Id = @appointmentId
			INNER JOIN States AS S ON L.FK_State_Id = S.PK_State_Id
			WHERE (R.[Date] = @date) OR
			(
				dbo.fn_StudentHasAppointment(A.FK_Student_Id, R.[Date]) = 0 AND
				dbo.fn_GetLaboratoryAvailableSeats(L.PK_Laboratory_Id, R.[Date]) > 0 AND
				dbo.fn_IsLaboratoryReserved(L.PK_Laboratory_Id, R.[Date], R.[Date]) = 0 AND
				(
					DATEDIFF(WEEK, R.[Date], @date) = 0 OR
					dbo.fn_GetStudentWeeklyAppointmentsCount(A.FK_Student_Id, R.[Date]) < 2
				)
			)
		) InnerDateRange
	) DateRange
	WHERE RN = 1;
	RETURN;
END
GO


