IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateReservation]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateReservation]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update a reservation data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateReservation]
(
	@requester_id	INT,				-- The identity of the requester user.
	@id				INT,				-- The reservation identity.
	@professor		VARCHAR(70),		-- The professor username.
	@start_time		DATETIMEOFFSET(3),	-- The reservation start time.
	@end_time		DATETIMEOFFSET(3),	-- The reservation end time.
	@group			INT,				-- The group identity.
	@laboratory		VARCHAR(500),		-- The laboratory name. If NULL then laboratory will be assigned automatically.
	@software		VARCHAR(20),		-- The software code.
	@state			VARCHAR(30),		-- The reservation state.
	@attendance		BIT					-- The reservation attendance.	
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DECLARE @professorId INT, @laboratoryId INT, @softwareId INT;
	DECLARE @currentProfessorId INT, @currentLaboratoryId INT, @currentGroupId INT, @currentStartTime DATETIMEOFFSET(3), @currentEndTime DATETIMEOFFSET(3);
	DECLARE @isValidDate BIT;
	
	SELECT	@currentProfessorId = R.FK_Professor_Id,
			@currentLaboratoryId = R.FK_Laboratory_Id,
			@currentGroupId = R.FK_Group_Id,
			@currentStartTime = R.Start_Time,
			@currentEndTime = R.End_Time
	FROM Reservations AS R WHERE R.PK_Reservation_Id = @id;
	
	-- Get the professor identity.
	SELECT @professorId = FK_User_Id FROM Professors AS P INNER JOIN Users AS U ON P.FK_User_Id = U.PK_User_Id WHERE U.Username = @professor;
	IF @professor IS NOT NULL AND @professorId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El docente ingresado no existe: %s.', 15, 1, @professor);
		RETURN -1;
	END
	
	-- Get the laboratory identity.
	SELECT @laboratoryId = PK_Laboratory_Id FROM Laboratories WHERE Name = @laboratory;
	IF @laboratoryId IS NULL AND @laboratory IS NOT NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El laboratorio ingresado no existe: %s.', 15, 1, @laboratory);
		RETURN -1;
	END
	
	-- Get the software identity.
	IF @software = ''
	BEGIN
		SET @softwareId = 0;
	END
	ELSE
	BEGIN
		IF @software IS NOT NULL
		BEGIN
			SELECT @softwareId = PK_Software_Id FROM Software WHERE Code = @software;
			IF @softwareId IS NULL
			BEGIN
				ROLLBACK TRANSACTION T;
				RAISERROR('El software ingresado no existe: %s.', 15, 1, @software);
				RETURN -1;
			END
		END
	END
	
	-- Check if the group belongs to the professor.
	IF @group IS NOT NULL AND @group <> 0
	BEGIN
		SET @professorId = COALESCE(@professorId, @currentProfessorId);
		IF NOT EXISTS (SELECT 1 FROM Groups WHERE PK_Group_Id = @group AND FK_Professor_Id = @professorId)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El grupo ingresado pertenece a otro docente.', 15, 1);
			RETURN -1;
		END
	END
	
	-- Check that the laboratory is available in the given time.
	IF (@start_time IS NOT NULL OR @end_time IS NOT NULL OR @laboratory IS NOT NULL)
	BEGIN
		SET @start_time = COALESCE(@start_time, @currentStartTime);
		SET @end_time = COALESCE(@end_time, @currentEndTime);
		SET @laboratoryId = COALESCE(@laboratoryId, @currentLaboratoryId);
		SET @group = COALESCE(@group, @currentGroupId);
		
		IF (@start_time <> @currentStartTime OR @end_time <> @currentEndTime OR @laboratoryId <> @currentLaboratoryId)
		BEGIN
			IF dbo.fn_GetAppointmentsBetween(@laboratoryId, @start_time, @end_time) > 0 OR
			EXISTS
			(
				SELECT 1 FROM Reservations AS R
				INNER JOIN States AS S ON R.FK_State_Id = S.PK_State_Id
				WHERE R.PK_Reservation_Id <> @id AND R.FK_Laboratory_Id = @laboratoryId AND S.Name <> 'Cancelada' AND
				  (
					((R.Start_Time BETWEEN @start_time AND @end_time) AND R.Start_Time <> @end_time)
					OR 
					((R.End_Time BETWEEN @start_time AND @end_time) AND R.End_Time <> @start_time)
				  )
			)
			BEGIN
				ROLLBACK TRANSACTION T;
				RAISERROR('El laboratorio no se encuentra disponible durante el tiempo ingresado.', 15, 1);
				RETURN -1;
			END
		END
		
		IF @group IS NOT NULL
		BEGIN
			-- Check if the date match the group period.
			SELECT TOP 1
			@isValidDate = dbo.fn_IsDateInPeriod(@start_time, P.Value, PT.Name, G.Period_Year) &
						   dbo.fn_IsDateInPeriod(@end_time, P.Value, PT.Name, G.Period_Year)
			FROM Groups AS G
			INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
			INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
			WHERE G.PK_Group_Id = @group;
			
			IF @isValidDate = 0
			BEGIN
				ROLLBACK TRANSACTION T;
				RAISERROR('La fecha ingresada no coincide con el periodo lectivo del grupo.', 15, 1);
				RETURN -1;
			END
		END
	END
	
	IF @state = 'Por iniciar' AND @end_time < SYSDATETIMEOFFSET()
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('No se permite activar reservaciones del pasado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Reservations SET
	Start_Time = ISNULL(@start_time, Start_Time),
	End_Time = ISNULL(@end_time, End_Time),
	Attendance = ISNULL(@attendance, Attendance),
	FK_Professor_Id = ISNULL(@professorId, FK_Professor_Id),
	FK_Laboratory_Id = ISNULL(@laboratoryId, FK_Laboratory_Id),
	FK_Software_Id = CAST(dbo.fn_IsNull(@softwareId, FK_Software_Id) AS INT),
	FK_Group_Id = CAST(dbo.fn_IsNull(@group, FK_Group_Id) AS INT),
	FK_State_Id = ISNULL(dbo.fn_GetStateID('RESERVATION', @state), FK_State_Id),
	Updated_At = SYSDATETIMEOFFSET()
	WHERE PK_Reservation_Id = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetReservation @requester_id, @id, '*';
END
GO


