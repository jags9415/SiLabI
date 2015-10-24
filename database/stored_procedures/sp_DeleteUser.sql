IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteUser]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteUser]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set an user as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteUser]
(
	@requester_id	INT,	-- The identity of the requester user.
	@user_id		INT		-- The user identity.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	IF NOT EXISTS (SELECT 1 FROM vw_Users WHERE id = @user_id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Usuario no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Users
	SET FK_State_Id = dbo.fn_GetStateID('USER', 'Inactivo'),
	Updated_At = SYSDATETIMEOFFSET()
	WHERE PK_User_Id = @user_id;
	
	COMMIT TRANSACTION T;
END
GO


