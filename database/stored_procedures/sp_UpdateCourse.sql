IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateCourse]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateCourse]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update a course data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateCourse]
(
	@requester_id	INT,			-- The identity of the requester user.
	@id				INT,			-- The course identity.
	@name			VARCHAR(500),	-- The course name.
	@code			VARCHAR(20),	-- The course code.
	@state			VARCHAR(30)		-- The course state.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if course exists.
	IF NOT EXISTS (SELECT 1 FROM Courses WHERE PK_Course_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Curso no encontrado.', 15, 1);
		RETURN -1;
	END
	
	-- Check if course exists.
	IF EXISTS (SELECT 1 FROM Courses WHERE Code = @code AND PK_Course_Id <> @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un curso con el código %s.', 15, 1, @code);
		RETURN -1;
	END
	
	UPDATE Courses
	SET [Code] = ISNULL(@code, [Code]),
	[Name] = ISNULL(@name, [Name]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('COURSE', @state), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [PK_Course_Id] = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetCourse @requester_id, @id, '*';
END
GO


