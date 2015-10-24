IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetUserType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetUserType]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the type of a user.
-- =============================================
CREATE FUNCTION [dbo].[fn_GetUserType]
(
	@UserId INT		-- The user identity.
)
RETURNS VARCHAR(30)
AS
BEGIN
	IF EXISTS 
	(
		SELECT 1 FROM Administrators AS A
		INNER JOIN States AS S ON S.PK_State_Id = A.FK_State_Id
		WHERE A.FK_User_Id = @UserId AND S.Name = 'Activo'
	)
    RETURN 'Administrador'

    IF EXISTS
	(
		SELECT 1 FROM Operators AS O
		INNER JOIN States AS S ON S.PK_State_Id = O.FK_State_Id
		INNER JOIN Periods AS P ON P.PK_Period_Id = O.FK_Period_Id
		INNER JOIN PeriodTypes AS PT ON PT.PK_Period_Type_Id = P.FK_Period_Type_Id
		WHERE O.FK_User_Id = @UserId AND S.Name = 'Activo' AND dbo.fn_IsDateInPeriod(SYSDATETIMEOFFSET(), P.Value, PT.Name, O.Period_Year) = 1
	)
	RETURN 'Operador'
    
    IF EXISTS (SELECT 1 FROM Students WHERE FK_User_Id = @UserId)
    RETURN 'Estudiante'
    
    IF EXISTS (SELECT 1 FROM Professors WHERE FK_User_Id = @UserId)
    RETURN 'Docente'
    
    RETURN NULL
END
GO


