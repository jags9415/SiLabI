IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteSoftware]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteSoftware]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set a software as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteSoftware]
(
	@requester_id	INT,	-- The identity of the requester user.
	@id				INT		-- The software identity.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if software exists.
	IF NOT EXISTS (SELECT 1 FROM Software WHERE PK_Software_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Software no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Software
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('SOFTWARE', 'Inactivo'), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [PK_Software_Id] = @id;
	
	COMMIT TRANSACTION T;
END
GO


