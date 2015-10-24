IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetDateTimeRange]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetDateTimeRange]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get a range of datetimes.
-- =============================================
CREATE FUNCTION [dbo].[fn_GetDateTimeRange]
(
	@start DATETIMEOFFSET(3),	-- The start datetime.
	@weeks INT					-- The weeks offset.
)
RETURNS @daterange TABLE ([date] DATETIMEOFFSET(3))
AS
BEGIN 
	DECLARE @start_date DATETIMEOFFSET(3), @end_date DATETIMEOFFSET(3), @weekday INT;
	
	-- Switch the timezone to CST Central Standard Time (UTC -06:00).
	SET @start = SWITCHOFFSET(@start, '-06:00'); 
	-- Ceil the date to the next hour.
	SET @start_date = dbo.fn_CeilDate(@start, 'hh');
	-- Calculate the end date.
	SET @end_date = DATEADD(WEEK, @weeks, @start_date);

	-- If the time from @start_date is before 08:00 am. SET time of @start_date to 08:00 am.
	IF DATEPART(HOUR, @start_date) < 8
	BEGIN
		SET @start_date = TODATETIMEOFFSET(CAST(@start_date AS DATE), DATEPART(tz, @start_date));
		SET @start_date = DATEADD(HOUR, 8, @start_date);
	END

	WHILE @start_date < @end_date
	BEGIN
		SET @weekday = DATEPART(dw, @start_date);
		
		-- Do not include Saturdays or Sundays.
		IF @weekday != 1 AND @weekday != 7
		BEGIN
			WHILE DATEPART(HOUR, @start_date) <= 17
			BEGIN
				INSERT INTO @daterange VALUES (@start_date);
				SET @start_date = DATEADD(HOUR, 1, @start_date);
			END
		END
		
		-- SET the time to 00:00:00.
		SET @start_date = TODATETIMEOFFSET(CAST(@start_date AS DATE), DATEPART(tz, @start_date));
		-- ADD 1 day.
		SET @start_date = DATEADD(DAY, 1, @start_date);
		-- ADD 8 hours.
		SET @start_date = DATEADD(HOUR, 8, @start_date);
	END
	
	RETURN;
END
GO


