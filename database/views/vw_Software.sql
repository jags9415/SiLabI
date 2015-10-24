IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Software]'))
	DROP VIEW [dbo].[vw_Software]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Software] AS
SELECT	S.PK_Software_Id AS [id], S.Code AS [code], S.Name AS [name], E.Name AS [state],
		S.Created_At AS [created_at], S.Updated_At AS [updated_at] 
FROM Software AS S
INNER JOIN States AS E ON S.FK_State_Id = E.PK_State_Id;
GO


