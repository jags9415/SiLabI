IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Operators]'))
	DROP VIEW [dbo].[vw_Operators]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Operators] AS

SELECT	O.PK_Operator_Id AS [id], U.Name AS [name], U.Last_Name_1 AS [last_name_1], U.Last_Name_2 AS [last_name_2],
		RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [full_name],
		U.Email AS [email], U.Username AS [username], U.Gender AS [gender], U.Phone AS [phone],
		O.Created_At AS [created_at], O.Updated_At AS [updated_at], S.Name AS [state],
		P.Value AS [period.value], PT.Name AS [period.type], O.Period_Year AS [period.year]
FROM Operators AS O
INNER JOIN Users AS U ON O.FK_User_Id = U.PK_User_Id
INNER JOIN Periods AS P ON O.FK_Period_Id = P.PK_Period_Id
INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
INNER JOIN States AS S ON O.FK_State_Id = S.PK_State_Id
GO