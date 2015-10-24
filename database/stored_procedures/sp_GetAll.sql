IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAll]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetAll]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the list of rows that match a query on a view or table.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAll]
(
	@table		VARCHAR(MAX),	-- The table name.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX);
	DECLARE @start INT, @end INT;

	-- SET default values.	
	SET @fields = COALESCE(@fields, '');
	SET @order_by = COALESCE(@order_by, '');
	SET @where = COALESCE(@where, '');
	SET @page = COALESCE(@page, 1);
	SET @limit = COALESCE(@limit, 20);
	
	-- SET the start and end rows.
	SET @start = ((@page - 1) * @limit) + 1;
	SET @end = @page * @limit;
	
	-- SET default fields to retrieve.
	IF (@fields = '')
	BEGIN
		SET @fields = '*';
	END
	
	-- SET default ORDER BY query.
	IF (@order_by = '') SET @order_by = 'RAND()';
	
	-- SET default WHERE query.
	IF (@where = '') SET @where = '1=1';

	SET @sql =	'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_by + ') AS rn, ' +
				@fields + ' FROM ' + @table +
				' WHERE ' + @where + ') AS Sub WHERE rn >= ' + CAST(@start AS VARCHAR) +
				' AND (rn <= ' + CAST(@end AS VARCHAR) + ' OR ' + CAST(@limit AS VARCHAR) + ' < 0)';
				
	EXECUTE(@sql);
END

GO


