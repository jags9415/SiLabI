IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Laboratories]'))
	DROP VIEW [dbo].[vw_Laboratories]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Laboratories] AS
SELECT	L.PK_Laboratory_Id AS [id], L.Name AS [name], L.Seats AS [seats], S.Name AS [state],
		L.Created_At AS [created_at], L.Updated_At AS [updated_at],
		L.Appointment_Priority AS [appointment_priority], L.Reservation_Priority AS [reservation_priority]
FROM Laboratories AS L
INNER JOIN States AS S ON L.FK_State_Id = S.PK_State_Id
GO