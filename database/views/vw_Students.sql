IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Students]'))
	DROP VIEW [dbo].[vw_Students]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Students] AS
SELECT	U.PK_User_Id AS [id], U.Name AS [name], U.Last_Name_1 AS [last_name_1], U.Last_Name_2 AS [last_name_2],
		RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [full_name],
		U.Email AS [email], U.Username AS [username], U.Gender AS [gender], U.Phone AS [phone],
		U.Created_At AS [created_at], U.Updated_At AS [updated_at], E.Name AS [state]
FROM Students AS S
INNER JOIN Users AS U ON S.FK_User_Id = U.PK_User_Id
INNER JOIN States AS E ON U.FK_State_Id = E.PK_State_Id
GO