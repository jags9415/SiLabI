IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateGroup]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateGroup]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update a group data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateGroup]
(
	@requester_id	INT,					-- The identity of the requester user.
	@id				INT,					-- The group identity.
	@course			VARCHAR(20),			-- The course code.
	@number			INT,					-- The group number.
	@professor		VARCHAR(70),			-- The professor username.
	@period_value	INT,					-- The period value. (1, 2, ...)
	@period_type	VARCHAR(50),			-- The period type ('Semestre', 'Bimestre', ...)
	@period_year	INT,					-- The year. (2014, 2015, ...)
	@state			VARCHAR(30),			-- The group state.
	@students		AS UserList READONLY	-- The list of students usernames.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if group exists.
	IF NOT EXISTS (SELECT 1 FROM Groups WHERE PK_Group_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Grupo no encontrado.', 15, 1);
		RETURN -1;
	END
	
	DECLARE @courseId INT, @professorId INT, @periodId INT;
	SELECT @courseId = PK_Course_Id FROM Courses WHERE Code = @course;
	SELECT @professorId = PK_User_Id FROM Users WHERE Username = @professor;
	SELECT @periodId = PK_Period_Id FROM Periods INNER JOIN PeriodTypes ON FK_Period_Type_Id = PK_Period_Type_Id WHERE Value = @period_value AND Name = @period_type;
	
	IF @course IS NOT NULL AND @courseId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El curso ingresado no existe: %s.', 15, 1, @course);
		RETURN -1;
	END
	
	IF @professor IS NOT NULL AND @professorId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El profesor ingresado no existe: %s.', 15, 1, @professor);
		RETURN -1;
	END
	
	IF (@period_value IS NOT NULL OR @period_type IS NOT NULL) AND @periodId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El periodo ingresado es inválido.', 15, 1);
		RETURN -1;
	END
	
	-- Check if group exists.
	IF EXISTS (SELECT 1 FROM Groups WHERE FK_Course_Id = @courseId AND FK_Professor_Id = @professorId AND FK_Period_Id = @periodId AND Period_Year = @period_year AND Number = @number AND PK_Group_Id <> @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un grupo con estos datos.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Groups
	SET FK_Course_Id = ISNULL(@courseId, FK_Course_Id),
	FK_Professor_Id = ISNULL(@professorId, FK_Professor_Id),
	FK_Period_Id = ISNULL(@periodId, FK_Period_Id),
	FK_State_Id = ISNULL(dbo.fn_GetStateID('GROUP', @state), FK_State_Id),
	Number = ISNULL(@number, Number),
	Period_Year = ISNULL(@period_year, Period_Year),
	Updated_At = SYSDATETIMEOFFSET()
	WHERE PK_Group_Id = @id;
	
	IF EXISTS (SELECT 1 FROM @students)
	BEGIN
		EXEC dbo.sp_UpdateGroupstudents @requester_id, @id, @students;
	END
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetGroup @requester_id, @id, '*';
END
GO


