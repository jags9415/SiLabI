IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteGroup]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteGroup]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set a group as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteGroup]
(
	@requester_id	INT,	-- The identity of the requester user.
	@id				INT		-- The group identity.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if group exists.
	IF NOT EXISTS (SELECT 1 FROM Groups WHERE PK_Group_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Grupo no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Groups
	SET FK_State_Id = dbo.fn_GetStateID('GROUP', 'Inactivo'),
	Updated_At = SYSDATETIMEOFFSET()
	WHERE PK_Group_Id = @id;
	
	COMMIT TRANSACTION T;
END

GO


