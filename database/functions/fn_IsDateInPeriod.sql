IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsDateInPeriod]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_IsDateInPeriod]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Check if a date is between a period.
-- =============================================
CREATE FUNCTION [dbo].[fn_IsDateInPeriod]
(
	@date         DATETIMEOFFSET(3),	-- The datetime to check.
	@period_value INT,					-- The period value (1, 2, 3, ...)
	@period_type  VARCHAR(50),			-- The period type (Bimestre, Trimestre, ...)
	@period_year  INT					-- The period year (2015, 2016, ...)
)
RETURNS BIT
AS
BEGIN
	DECLARE @start_date DATETIMEOFFSET(3), @end_date DATETIMEOFFSET(3), @offset INT;
	
	SET @start_date = TODATETIMEOFFSET(CAST(CAST(2015 AS VARCHAR) AS DATE), '-06:00');
	SET @end_date = TODATETIMEOFFSET(CAST(CAST(2015 AS VARCHAR) AS DATE), '-06:00');
	
	SELECT @offset =
      CASE @period_type
         WHEN 'BIMESTRE' THEN 2
         WHEN 'TRIMESTRE' THEN 3
         WHEN 'CUATRIMESTRE' THEN 4
         WHEN 'SEMESTRE' THEN 6
      END;
	
	SET @start_date = DATEADD(MONTH, (@offset * (@period_value - 1)), @start_date)
	SET @end_date = DATEADD(MONTH, (@offset * @period_value), @end_date)
	SET @end_date = DATEADD(MILLISECOND, -1, @end_date)

	IF (@date BETWEEN @start_date AND @end_date) RETURN 1
	
	RETURN 0
END

GO


