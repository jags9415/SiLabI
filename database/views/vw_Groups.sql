IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Groups]'))
	DROP VIEW [dbo].[vw_Groups]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Groups]
AS
SELECT	[Group].id, [Group].number, [Group].[period.value], [Group].[period.type], [Group].[period.year], [Group].state, [Group].created_at, [Group].updated_at, Course.[course.id], 
		Course.[course.code], Course.[course.name], Course.[course.state], Course.[course.created_at], Course.[course.updated_at], Professor.[professor.id], 
		Professor.[professor.name], Professor.[professor.last_name_1], Professor.[professor.last_name_2], Professor.[professor.full_name], Professor.[professor.email], 
		Professor.[professor.gender], Professor.[professor.phone], Professor.[professor.username], Professor.[professor.state], Professor.[professor.created_at], 
		Professor.[professor.updated_at]
FROM
(
	SELECT	G.PK_Group_Id AS [id], G.Number AS [number], G.FK_Course_Id AS [course], G.FK_Professor_Id AS [professor], P.Value AS [period.value], 
            PT.Name AS [period.type], G.Period_Year AS [period.year], S.Name AS [state], G.Created_At AS [created_at], G.Updated_At AS [updated_at]
    FROM Groups AS G
    INNER JOIN States AS S ON G.FK_State_Id = S.PK_State_Id
    INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
    INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
) AS [Group]
INNER JOIN
(
	SELECT	C.PK_Course_Id AS [course.id], C.Code AS [course.code], C.Name AS [course.name], S.Name AS [course.state],
			C.Created_At AS [course.created_at], C.Updated_At AS [course.updated_at]
    FROM Courses AS C
    INNER JOIN States AS S ON C.FK_State_Id = S.PK_State_Id
) AS Course ON [Group].course = Course.[course.id]
INNER JOIN
(
	SELECT	U.PK_User_Id AS [professor.id], U.Name AS [professor.name], U.Last_Name_1 AS [professor.last_name_1], U.Last_Name_2 AS [professor.last_name_2], 
            RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [professor.full_name], U.Email AS [professor.email], 
            U.Gender AS [professor.gender], U.Phone AS [professor.phone], U.Username AS [professor.username], S.Name AS [professor.state], 
            U.Created_At AS [professor.created_at], U.Updated_At AS [professor.updated_at]
	FROM Professors AS P
	INNER JOIN Users AS U ON P.FK_User_Id = U.PK_User_Id
	INNER JOIN States AS S ON U.FK_State_Id = S.PK_State_Id
) AS Professor ON [Group].professor = Professor.[professor.id]
GO