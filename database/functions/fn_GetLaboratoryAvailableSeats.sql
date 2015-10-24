IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetLaboratoryAvailableSeats]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetLaboratoryAvailableSeats]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the amount of seats available in a laboratory at a specific time.
-- =============================================
CREATE FUNCTION [dbo].[fn_GetLaboratoryAvailableSeats]
(
	@id INT,					-- The laboratory identity.
	@date DATETIMEOFFSET(3)		-- The datetime.
)
RETURNS INT
AS
BEGIN
	DECLARE @appointments INT, @seats INT;
	
	SELECT @seats = Seats FROM Laboratories WHERE PK_Laboratory_Id = @id;
	
	SELECT @appointments = COUNT(1) FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
	WHERE A.FK_Laboratory_Id = @id AND A.Date = @date AND S.Name <> 'Cancelada';
	
	RETURN @seats - @appointments;
END
GO


