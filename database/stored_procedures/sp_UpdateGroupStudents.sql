IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateGroupStudents]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateGroupStudents]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update the student list of a group.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateGroupStudents]
(
	@requester_id	INT,					-- The identity of the requester user.
	@group			INT,					-- The group identity.
	@students		AS UserList READONLY	-- The list of students usernames.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DELETE StudentsByGroup
	WHERE FK_Group_Id = @group;
	
	EXEC dbo.sp_AddStudentsToGroup @requester_id, @group, @students;
	
	COMMIT TRANSACTION T;
END
GO


