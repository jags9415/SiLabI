IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetGroupsByStudent]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetGroupsByStudent]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the list of groups to which a student belongs.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetGroupsByStudent]
(
	@requester_id	INT,			-- The identity of the requester user.
	@student		VARCHAR(70),	-- The student username.
	@fields			VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by		VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where			VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	DECLARE @studentId INT, @where_statement VARCHAR(MAX);
	
	-- Get the student identity.
	SELECT @studentId = FK_User_Id FROM Students AS S INNER JOIN Users AS U ON S.FK_User_Id = U.PK_User_Id WHERE U.Username = @student;
	
	IF @studentId IS NOT NULL
	BEGIN
		SET @where = COALESCE(@where, '');
		SET @where_statement = 'id IN (SELECT FK_Group_Id FROM StudentsByGroup WHERE FK_Student_Id = ' + CAST(@studentId AS VARCHAR) + ')';
		IF (@where <> '') SET @where_statement = @where_statement + ' AND ' + @where;
		
		EXEC dbo.sp_GetAll 'vw_Groups', @fields, @order_by, @where_statement, NULL, -1;
	END
END
GO


