IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Users]'))
	DROP VIEW [dbo].[vw_Users]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Users] AS
SELECT	U.PK_User_Id AS [id], U.Name AS [name], U.Last_Name_1 AS [last_name_1], U.Last_Name_2 AS [last_name_2],
		RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [full_name],
		U.Email AS [email], U.Username AS [username], U.Gender AS [gender], U.Phone AS [phone],
		U.Created_At AS [created_at], U.Updated_At AS [updated_at], S.Name AS [state],
		dbo.fn_GetUserType(U.PK_User_Id) AS [type]
FROM Users AS U
INNER JOIN States AS S ON U.FK_State_Id = S.PK_State_Id
GO