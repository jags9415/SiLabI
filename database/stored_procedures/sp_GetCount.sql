IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetCount]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetCount]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Count the number of rows that match a query on a view or table.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCount]
(
	@table		VARCHAR(MAX),	-- The table name.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX);

	-- SET default values.	
	IF (COALESCE(@where, '') = '')
		SET @where = '1=1';

	SET @sql =	'SELECT COUNT(1) AS count FROM ' + @table + ' WHERE ' + @where;
				
	EXECUTE(@sql); 
END
GO


