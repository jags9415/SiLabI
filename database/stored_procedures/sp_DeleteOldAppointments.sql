IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteOldAppointments]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteOldAppointments]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Delete from the database all the appointments made before 1 year.
--
-- Example:
--   If the proceduce if called on 2015, then all the apppointment before 2014-01-01 will be deleted.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteOldAppointments]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @date DATETIMEOFFSET(3);
	
	SET @date = SYSDATETIMEOFFSET();				-- Get current datetime.
	SET @date = dbo.fn_FloorDate(@date, 'yy');		-- Set day to January 1st.
	SET @date = DATEADD(YEAR, -1, @date);			-- Substract 1 year.
	SET @date = TODATETIMEOFFSET(@date, '-06:00');	-- Convert to Central America Time (UTC -06:00)
	
	DELETE Appointments WHERE [Date] < @date;
END
GO


