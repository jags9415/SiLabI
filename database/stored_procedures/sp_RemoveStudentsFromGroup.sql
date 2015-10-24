IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RemoveStudentsFromGroup]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_RemoveStudentsFromGroup]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Remove a list of students from a group.
-- =============================================
CREATE PROCEDURE [dbo].[sp_RemoveStudentsFromGroup]
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
	WHERE FK_Group_Id = @group
	AND FK_Student_Id IN (SELECT U.PK_User_Id FROM @students AS S INNER JOIN Users AS U ON S.Username = U.Username);
	
	UPDATE Groups
	SET Updated_At = SYSDATETIMEOFFSET()
	WHERE PK_Group_Id = @group;
	
	COMMIT TRANSACTION T;
END
GO


