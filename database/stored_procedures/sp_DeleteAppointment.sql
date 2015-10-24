IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteAppointment]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteAppointment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set an appointment as canceled.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteAppointment]
(
	@requester_id INT,		-- The requester user identity.
	@id		INT				-- The appointment identity.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	DECLARE @student_id INT;
	
	-- Check if appointment exists.
	IF NOT EXISTS (SELECT 1 FROM Appointments WHERE PK_Appointment_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Cita no encontrada.', 15, 1);
		RETURN -1;
	END
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Estudiante')
	BEGIN
		SELECT @student_id = FK_Student_Id FROM Appointments WHERE PK_Appointment_Id = @id; 
		IF (@student_id <> @requester_id)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('La cita ingresada pertenece a otro estudiante.', 15, 1);
			RETURN -1;
		END
	END
	
	UPDATE Appointments
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('APPOINTMENT', 'Cancelada'), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [PK_Appointment_Id] = @id;
	
	COMMIT TRANSACTION T;
END
GO


