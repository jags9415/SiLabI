IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetOne]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetOne]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get a single row from a table or view.
-- =============================================

CREATE PROCEDURE [dbo].[sp_GetOne]
(
	@table		VARCHAR(MAX),	-- The table name.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX);
	DECLARE @start INT, @end INT;

	-- SET default values.	
	SET @fields = COALESCE(@fields, '');
	SET @where = COALESCE(@where, '');
	
	-- SET default fields to retrieve.
	IF (@fields = '') SET @fields = '*';
	
	-- SET default WHERE query.
	IF (@where = '') SET @where = '1=1';

	SET @sql = 'SELECT TOP 1 ' + @fields + ' FROM ' + @table + ' WHERE ' + @where;
				
	EXECUTE(@sql);
END
GO


