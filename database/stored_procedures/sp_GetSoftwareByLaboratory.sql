IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetSoftwareByLaboratory]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetSoftwareByLaboratory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the list of software in a specific laboratory.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetSoftwareByLaboratory]
(
	@requester_id	INT,			-- The identity of the requester user.
	@laboratory		INT,			-- The laboratory identity.
	@fields			VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by		VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where			VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	DECLARE @where_statement VARCHAR(MAX);
	SET @where = COALESCE(@where, '');
	SET @where_statement = 'id IN (SELECT FK_Software_Id FROM SoftwareByLaboratory WHERE FK_Laboratory_Id = ' + CAST(@laboratory AS VARCHAR) + ')';
	IF (@where <> '') SET @where_statement = @where_statement + ' AND ' + @where;
	EXEC dbo.sp_GetAll 'vw_Software', @fields, @order_by, @where_statement, NULL, -1;
END
GO


