IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAvailableAppointmentsForUpdate]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetAvailableAppointmentsForUpdate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the list of available dates and laboratory for an appointment UPDATE operation.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAvailableAppointmentsForUpdate]
(
	@requester_id	INT,			-- The identity of the requester user.
	@appointment	INT,		    -- The appointment identity.
	@fields			VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by		VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where			VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql VARCHAR(MAX);
	
	-- SET default values.	
	SET @fields = COALESCE(@fields, '');
	SET @order_by = COALESCE(@order_by, '');
	SET @where = COALESCE(@where, '');
	
	-- SET default fields to retrieve.
	IF (@fields = '') SET @fields = '*';
	
	-- SET default ORDER BY query.
	IF (@order_by = '') SET @order_by = '[date] ASC';
	
	-- SET default WHERE query.
	IF (@where = '') SET @where = '1=1';
	
	SET @sql = 'SELECT ' + @fields + ' FROM dbo.fn_GetAvailableAppointmentsForUpdate(' + CAST(@appointment AS VARCHAR) + ') WHERE ' + @where + ' ORDER BY ' + @order_by;
				
	EXECUTE(@sql);
END
GO


