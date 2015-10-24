IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_MarkYesterdayAppointmentsAsFinished]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_MarkYesterdayAppointmentsAsFinished]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Change the state of the appointment made yesterday to 'Finalizada' if the state is 'Por iniciar'.
-- =============================================
CREATE PROCEDURE [dbo].[sp_MarkYesterdayAppointmentsAsFinished]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @yesterday DATETIMEOFFSET(3), @start DATETIMEOFFSET(3), @end DATETIMEOFFSET(3);

	SET @yesterday = DATEADD(DAY, -1, SYSDATETIMEOFFSET());

	SET @start = dbo.fn_FloorDate(@yesterday, 'dd');
	SET @start = TODATETIMEOFFSET(@start, '-06:00');

	SET @end = dbo.fn_CeilDate(@yesterday, 'dd');
	SET @end = DATEADD(MILLISECOND, -1, @end);
	SET @end = TODATETIMEOFFSET(@end, '-06:00');
	
	UPDATE Appointments SET
	[FK_State_Id] = dbo.fn_GetStateID('APPOINTMENT', 'Finalizada'),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [Date] BETWEEN @start AND @end AND FK_State_Id = dbo.fn_GetStateID('APPOINTMENT', 'Por iniciar');
END
GO


