IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RemoveSoftwareFromLaboratory]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_RemoveSoftwareFromLaboratory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Remove a list of software from a laboratory.
-- =============================================
CREATE PROCEDURE [dbo].[sp_RemoveSoftwareFromLaboratory]
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
	WHERE FK_Laboratory_Id = @laboratory
	AND FK_Software_Id IN (SELECT S2.PK_Software_Id FROM @softwares AS S1 INNER JOIN Software AS S2 ON S1.Code = S2.Code);
	
	UPDATE Laboratories
	SET Updated_At = SYSDATETIMEOFFSET()
	WHERE PK_Laboratory_Id = @laboratory;
	
	COMMIT TRANSACTION T;
END
GO


