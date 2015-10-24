IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_CeilDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_CeilDate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Ceil a date to a specific datepart.
-- =============================================
CREATE FUNCTION [dbo].[fn_CeilDate]
(
  @seed DATETIMEOFFSET, -- The date.
  @part VARCHAR(2)      -- The part to ceil. (ss, mi, hh, dd, mm, yy).
)
RETURNS DATETIMEOFFSET
AS
BEGIN

   DECLARE
       @second     DATETIMEOFFSET,
       @minute     DATETIMEOFFSET,
       @hour       DATETIMEOFFSET,
       @day        DATETIMEOFFSET,
       @month      DATETIMEOFFSET,
       @year       DATETIMEOFFSET;

   SELECT @seed = CASE
	   WHEN @part = 'ss' THEN DATEADD(ss, 1, @seed)
	   WHEN @part = 'mi' THEN DATEADD(mi, 1, @seed)
	   WHEN @part = 'hh' THEN DATEADD(hh, 1, @seed)
	   WHEN @part = 'dd' THEN DATEADD(dd, 1, @seed)     
	   WHEN @part = 'mm' THEN DATEADD(mm, 1, @seed)
	   WHEN @part = 'yy' THEN DATEADD(yy, 1, @seed)
   END

   SELECT @second = DATEADD(ms, -1 * DATEPART(ms, @seed), @seed);
   SELECT @minute = DATEADD(ss, -1 * DATEPART(ss, @second), @second);
   SELECT @hour = DATEADD(mi, -1 * DATEPART(mi, @minute), @minute)
   SELECT @day = DATEADD(hh, -1 * DATEPART(hh, @hour), @hour);
   SELECT @month = DATEADD(dd, -1 * (DAY(@day) - 1), @day);
   SELECT @year = DATEADD(mm, -1 * (MONTH(@month) - 1), @month);

   RETURN CASE
       WHEN @part = 'ss' THEN @second
       WHEN @part = 'mi' THEN @minute
       WHEN @part = 'hh' THEN @hour
       WHEN @part = 'dd' THEN @day
       WHEN @part = 'mm' THEN @month
       WHEN @part = 'yy' THEN @year
   END
END
GO


