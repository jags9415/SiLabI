IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateSoftware]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateSoftware]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update a software data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateSoftware]
(
	@requester_id	INT,			-- The identity of the requester user.
	@id				INT,			-- The software identity.
	@name			VARCHAR(500),	-- The software name.
	@code			VARCHAR(20),	-- The software code.
	@state			VARCHAR(30)		-- The software state.
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
	
	-- Check if software exists.
	IF EXISTS (SELECT 1 FROM Software WHERE Code = @code AND PK_Software_Id <> @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un software con el código %s.', 15, 1, @code);
		RETURN -1;
	END
	
	UPDATE Software
	SET [Code] = ISNULL(@code, [Code]),
	[Name] = ISNULL(@name, [Name]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('SOFTWARE', @state), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [PK_Software_Id] = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetSoftware @requester_id, @id, '*';
END
GO


