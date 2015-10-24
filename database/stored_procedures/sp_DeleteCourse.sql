IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteCourse]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteCourse]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set a course as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteCourse]
(
	@requester_id INT,	-- The identity of the requester user.
	@id		INT			-- The course identity.
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
	
	UPDATE Courses
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('COURSE', 'Inactivo'), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [PK_Course_Id] = @id;
	
	COMMIT TRANSACTION T;
END

GO


