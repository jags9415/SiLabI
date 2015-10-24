IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAppointment]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetAppointment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get an appointment data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAppointment]
(
	@requester_id	INT,			-- The requester user identity.
	@id				INT,			-- The appointment identity.
	@fields			VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	DECLARE @student_id INT;
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Estudiante')
	BEGIN
		SELECT @student_id = FK_Student_Id FROM Appointments WHERE PK_Appointment_Id = @id; 
		IF (@student_id <> @requester_id)
		BEGIN
			RAISERROR('La cita ingresada pertenece a otro estudiante.', 15, 1);
			RETURN -1;
		END
	END
	
	EXEC dbo.sp_GetOne 'vw_Appointments', @fields, @where;
END
GO


