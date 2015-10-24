IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_FloorDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_FloorDate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Floor a date to a specific datepart.
-- =============================================
CREATE FUNCTION [dbo].[fn_FloorDate] (
  @seed DATETIMEOFFSET,	-- The date.
  @part VARCHAR(2)		-- The part to floor. (ss, mi, hh, dd, mm, yy).
)
RETURNS DATETIMEOFFSET
AS
BEGIN
  DECLARE @date DATETIMEOFFSET =
  CASE
    WHEN LOWER(@part) = 'yy' THEN DATEADD(YEAR, DATEDIFF(YEAR, 0, @seed), 0)
    WHEN LOWER(@part) = 'mm' THEN DATEADD(MONTH, DATEDIFF(MONTH, 0, @seed), 0)
    WHEN LOWER(@part) = 'dd' THEN DATEADD(DAY, DATEDIFF(DAY, 0, @seed), 0)
    WHEN LOWER(@part) = 'hh' THEN DATEADD(HOUR, DATEDIFF(HOUR, 0, @seed), 0)
    WHEN LOWER(@part) = 'mi' THEN DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @seed), 0)
    WHEN LOWER(@part) = 'ss' THEN DATEADD(SECOND, DATEDIFF(SECOND, '2000-01-01', @seed), '2000-01-01')
    ELSE DATEADD(DAY, DATEDIFF(DAY, 0, @seed), 0)
  END;
  
  RETURN TODATETIMEOFFSET(@date, DATEPART(tz, @seed));
END
GO


