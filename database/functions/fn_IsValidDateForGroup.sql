IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsValidDateForGroup]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_IsValidDateForGroup]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Check if a date is during a group period.
-- =============================================
CREATE FUNCTION [dbo].[fn_IsValidDateForGroup]
(
	@groupId	INT,				-- The group identity.
	@date		DATETIMEOFFSET(3)	-- The date.
)
RETURNS BIT
AS
BEGIN
	DECLARE @valid BIT;
	
	SELECT TOP 1 @valid = dbo.fn_IsDateInPeriod(@date, P.Value, PT.Name, G.Period_Year)
	FROM Groups AS G
	INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
	INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
	WHERE G.PK_Group_Id = @groupId;
	
	RETURN @valid;
END
GO


