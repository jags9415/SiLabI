IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetStudentsByGroup]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetStudentsByGroup]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the list of students in a specific group.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetStudentsByGroup]
(
	@requester_id INT,			-- The identity of the requester user.
	@group		INT,			-- The group identity.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	DECLARE @where_statement VARCHAR(MAX);
	SET @where = COALESCE(@where, '');
	SET @where_statement = 'id IN (SELECT FK_Student_Id FROM StudentsByGroup WHERE FK_Group_Id = ' + CAST(@group AS VARCHAR) + ')';
	IF (@where <> '') SET @where_statement = @where_statement + ' AND ' + @where;
	EXEC dbo.sp_GetAll 'vw_Students', @fields, @order_by, @where_statement, NULL, -1;
END
GO


