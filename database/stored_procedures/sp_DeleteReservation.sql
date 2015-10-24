IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteReservation]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteReservation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set a reservation as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteReservation]
(
	@requester_id INT,		-- The requester user identity.
	@id		INT				-- The reservation identity.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	DECLARE @professor_id INT;
	
	-- Check if reservation exists.
	IF NOT EXISTS (SELECT 1 FROM Reservations WHERE PK_Reservation_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Reservación no encontrada.', 15, 1);
		RETURN -1;
	END
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Docente')
	BEGIN
		SELECT @professor_id = FK_Professor_Id FROM Reservations WHERE PK_Reservation_Id = @id; 
		IF (@professor_id <> @requester_id)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('La reservación ingresada pertenece a otro docente.', 15, 1);
			RETURN -1;
		END
	END
	
	UPDATE Reservations
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('RESERVATION', 'Cancelada'), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE PK_Reservation_Id = @id;
	
	COMMIT TRANSACTION T;
END
GO


