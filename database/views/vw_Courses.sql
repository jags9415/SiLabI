IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Courses]'))
	DROP VIEW [dbo].[vw_Courses]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Courses] AS
SELECT	C.PK_Course_Id AS [id], C.Code AS [code], C.Name AS [name],
		C.Created_At AS [created_at], C.Updated_At AS [updated_at], S.Name AS [state]
FROM Courses AS C
INNER JOIN States AS S ON C.FK_State_Id = S.PK_State_Id;
GO


