IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAvailableAppointmentsForCreate]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetAvailableAppointmentsForCreate]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the list of available dates and laboratory for an appointment CREATE operation.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAvailableAppointmentsForCreate]
(
	@requester_id	INT,			-- The identity of the requester user.
	@username		VARCHAR(70),    -- The username of the student who is making the appointment.
	@fields			VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by		VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where			VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @id INT, @sql NVARCHAR(MAX);
	SELECT TOP 1 @id = PK_User_Id FROM Users WHERE Username = @username;
	
	IF @id IS NULL
	BEGIN
		RAISERROR('Usuario no encontrado.', 15, 1);
		RETURN -1;
	END
	
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
	
	SET @sql = 'SELECT ' + @fields + ' FROM dbo.fn_GetAvailableAppointmentsForCreate(' + CAST(@id AS VARCHAR) + ') WHERE ' + @where + ' ORDER BY ' + @order_by;
				
	EXECUTE(@sql);
END
GO


