IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateAppointment]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateAppointment]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update an appointment data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateAppointment]
(
	@requester_id	INT,				-- The identity of the requester user.
	@id				INT,				-- The appointment identity.
	@student		VARCHAR(70),		-- The student username.
	@laboratory		VARCHAR(500),		-- The laboratory name.
	@software		VARCHAR(20),		-- The software code.
	@attendance		BIT,				-- The attendance flag.
	@date			DATETIMEOFFSET(3),	-- The date.
	@group			INT,				-- The group identity.
	@state			VARCHAR(30)			-- The state.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	DECLARE @studentId INT, @laboratoryId INT, @softwareId INT;
	DECLARE @currentState VARCHAR(30);
	
	-- Get the student identity.
	SELECT @studentId = FK_User_Id
	FROM Students AS S 
	INNER JOIN Users AS U ON S.FK_User_Id = U.PK_User_Id 
	WHERE U.Username = @student;
	
	-- Check the student identity.
	IF @student IS NOT NULL AND @studentId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El estudiante ingresado no existe: %s.', 15, 1, @student);
		RETURN -1;
	END
	
	-- Get the laboratory identity.
	SELECT @laboratoryId = PK_Laboratory_Id
	FROM Laboratories
	WHERE Name = @laboratory;
	
	-- Check the laboratory identity.
	IF @laboratoryId IS NULL AND @laboratory IS NOT NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El laboratorio ingresado no existe: %s.', 15, 1, @laboratory);
		RETURN -1;
	END
	
	-- Get the software identity.
	SELECT @softwareId = PK_Software_Id
	FROM Software WHERE Code = @software;
	
	-- Check the software identity.
	IF @software IS NOT NULL AND @softwareId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El software ingresado no existe: %s.', 15, 1, @software);
		RETURN -1;
	END
	
	-- Set default values.
	SELECT	@date = ISNULL(@date, A.[Date]),
			@studentId = ISNULL(@studentId, A.FK_Student_Id),
			@laboratoryId = ISNULL(@laboratoryId, A.FK_Laboratory_Id),
			@softwareId = ISNULL(@softwareId, A.FK_Software_Id),
			@currentState = S.Name
	FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
	WHERE A.PK_Appointment_Id = @id;
	
	-- Check if the date is after now.
	IF dbo.fn_IsValidDateForAppointment(@id, @date) = 0
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ingrese un día posterior a la fecha actual.', 15, 1);
		RETURN -1;
	END
	
	-- Check if laborqatory is available.
	IF ((@date IS NOT NULL OR @laboratory IS NOT NULL) AND dbo.fn_IsLaboratoryAvailableForAppointment(@id, @laboratoryId, @date) = 0)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El laboratorio no se encuentra disponible en la hora indicada.', 15, 1);
		RETURN -1;
	END
	
	IF @group IS NOT NULL
	BEGIN
		-- Check that the student is in the group.
		IF NOT EXISTS (SELECT 1 FROM StudentsByGroup WHERE FK_Group_Id = @group AND FK_Student_Id = @studentId)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El estudiante no pertenece al grupo ingresado.', 15, 1);
			RETURN -1;
		END
		
		-- Check if the date match the group period.
		IF dbo.fn_IsValidDateForGroup(@group, @date) = 0
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('La fecha ingresada no coincide con el periodo lectivo del grupo.', 15, 1);
			RETURN -1;
		END
	END
	
	-- Cannot modify future appointment attendance.
	IF @attendance IS NOT NULL AND @date > SYSDATETIMEOFFSET()
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('No se permite modificar la asistencia de citas futuras.', 15, 1);
		RETURN -1;
	END
	
	-- IF the state changed.
	IF @state <> @currentState
	BEGIN
		IF @state <> 'Cancelada' AND EXISTS
		(
			SELECT 1 FROM Appointments AS A
			INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
			WHERE   A.FK_Student_Id = @studentId AND 
					A.PK_Appointment_Id <> @id AND
					A.[Date] = @date AND
					S.Name <> 'Cancelada'
		)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('Ya existe una cita a la hora ingresada.', 15, 1);
			RETURN -1;
		END
		
		IF @state = 'Por iniciar' AND @date < SYSDATETIMEOFFSET()
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('No se permite activar citas del pasado.', 15, 1);
			RETURN -1;
		END
	END
	
	UPDATE Appointments SET
	[Date] = ISNULL(@date, [Date]),
	Attendance = ISNULL(@attendance, Attendance),
	FK_Student_Id = ISNULL(@studentId, FK_Student_Id),
	FK_Laboratory_Id = ISNULL(@laboratoryId, FK_Laboratory_Id),
	FK_Software_Id = ISNULL(@softwareId, FK_Software_Id),
	FK_Group_Id = ISNULL(@group, FK_Group_Id),
	FK_State_Id = ISNULL(dbo.fn_GetStateID('APPOINTMENT', @state), FK_State_Id),
	Updated_At = SYSDATETIMEOFFSET()
	WHERE PK_Appointment_Id = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetAppointment @requester_id, @id, '*';
END
GO


