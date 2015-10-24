IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteLaboratory]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteLaboratory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Mark a labotaratory as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteLaboratory]
(
	@requester_id INT,	-- The identity of the requester user.
	@id		INT			-- The laboratory identity.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if laboratory exists.
	IF NOT EXISTS (SELECT 1 FROM Laboratories WHERE PK_Laboratory_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Laboratorio no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Laboratories
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('LABORATORY', 'Inactivo'), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [PK_Laboratory_Id] = @id;
	
	COMMIT TRANSACTION T;
END
GO


