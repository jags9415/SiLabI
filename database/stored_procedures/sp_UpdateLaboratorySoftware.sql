IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateLaboratorySoftware]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateLaboratorySoftware]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update the software list of a laboratory.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateLaboratorySoftware]
(
	@requester_id	INT,						-- The identity of the requester user.
	@laboratory		INT,						-- The laboratory identity.
	@softwares		AS SoftwareList READONLY	-- The list of software codes.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DELETE SoftwareByLaboratory
	WHERE FK_Laboratory_Id = @laboratory;
	
	EXEC dbo.sp_AddSoftwareToLaboratory @requester_id, @laboratory, @softwares;
	
	COMMIT TRANSACTION T;
END
GO


