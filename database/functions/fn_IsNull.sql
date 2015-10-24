IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsNull]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_IsNull]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Similar funcion to ISNULL().
--
-- If the first parameter is NULL then return the second parameter.
-- If the first parameter is '' or 0 then return NULL.
-- Else return the first parameter.
-- =============================================
CREATE FUNCTION [dbo].[fn_IsNull]
(
	@p1 sql_variant,
	@p2 sql_variant
)
RETURNS sql_variant
AS
BEGIN
	RETURN CASE 
		WHEN @p1 IS NULL THEN @p2
		WHEN @p1 = '' THEN NULL
		WHEN @p1 = 0 THEN NULL
		ELSE @p1
	END
END
GO


