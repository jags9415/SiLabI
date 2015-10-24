IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteAdministrator]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteAdministrator]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set an administrator as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteAdministrator]
(
	@requester_id INT,		-- The identity of the requester user.
	@user_id INT			-- The user identity.
)
AS
BEGIN
	BEGIN TRANSACTION T;
	DECLARE @count INT, @state VARCHAR(30);
	SELECT @count = COUNT(*) FROM vw_Administrators WHERE [state] = 'Activo';
	SELECT @state = [state] FROM vw_Administrators WHERE [id] = @user_id;
	
	IF (@count = 1 AND @state = 'Activo')
	BEGIN
		RAISERROR('No se puede eliminar este administrador.', 15, 1);
		ROLLBACK TRANSACTION T;
		RETURN -1;
	END
	
	UPDATE Administrators
	SET FK_State_Id = dbo.fn_GetStateID('ADMINISTRATOR', 'Inactivo'),
		Updated_At = SYSDATETIMEOFFSET()
	WHERE FK_User_Id = @user_id;
	
	COMMIT TRANSACTION T;
END
GO


