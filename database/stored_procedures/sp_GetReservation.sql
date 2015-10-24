IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetReservation]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetReservation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get a reservation data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetReservation]
(
	@requester_id	INT,			-- The requester user identity.
	@id				INT,			-- The reservation identity.
	@fields			VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	DECLARE @professor_id INT;
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Docente')
	BEGIN
		SELECT @professor_id = FK_Professor_Id FROM Reservations WHERE PK_Reservation_Id = @id; 
		IF (@professor_id <> @requester_id)
		BEGIN
			RAISERROR('La reservación ingresada pertenece a otro docente.', 15, 1);
			RETURN -1;
		END
	END
	
	EXEC dbo.sp_GetOne 'vw_Reservations', @fields, @where;
END
GO


