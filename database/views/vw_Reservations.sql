IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Reservations]'))
	DROP VIEW [dbo].[vw_Reservations]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Reservations] AS
SELECT	Reservation.id, Reservation.start_time, Reservation.end_time, Reservation.created_at, Reservation.updated_at, Reservation.state, Professor.[professor.id], 
		Professor.[professor.name], Professor.[professor.last_name_1], Professor.[professor.last_name_2], Professor.[professor.email], Professor.[professor.full_name], 
		Professor.[professor.gender], Professor.[professor.phone], Professor.[professor.username], Professor.[professor.created_at], Professor.[professor.updated_at], 
		Professor.[professor.state], Laboratory.[laboratory.id], Laboratory.[laboratory.name], Laboratory.[laboratory.seats], Laboratory.[laboratory.state], 
		Laboratory.[laboratory.created_at], Laboratory.[laboratory.updated_at], Laboratory.[laboratory.appointment_priority], Laboratory.[laboratory.reservation_priority], 
		Software.[software.id], Software.[software.code], Software.[software.name], Software.[software.state], Software.[software.created_at], Software.[software.updated_at], 
		[Group].[group.id], [Group].[group.number], [Group].[group.period.value], [Group].[group.period.type], [Group].[group.period.year], [Group].[group.state], 
		[Group].[group.created_at], [Group].[group.updated_at], [Group].[group.course.id], [Group].[group.course.code], [Group].[group.course.name], 
		[Group].[group.course.state], [Group].[group.course.created_at], [Group].[group.course.updated_at], [Group].[group.professor.id], [Group].[group.professor.name], 
		[Group].[group.professor.last_name_1], [Group].[group.professor.last_name_2], [Group].[group.professor.full_name], [Group].[group.professor.email], 
		[Group].[group.professor.gender], [Group].[group.professor.phone], [Group].[group.professor.username], [Group].[group.professor.state], 
		[Group].[group.professor.created_at], [Group].[group.professor.updated_at], Reservation.attendance
FROM
(
	SELECT	R.PK_Reservation_Id AS [id], R.Attendance AS [attendance], R.Start_Time AS [start_time],
			R.End_Time AS [end_time], R.Created_At AS [created_at], R.Updated_At AS [updated_at],
			S.Name AS [state], R.FK_Group_Id AS [group], R.FK_Laboratory_Id AS [laboratory],
			R.FK_Professor_Id AS [professor], R.FK_Software_Id AS [software]
	FROM Reservations AS R
	INNER JOIN States AS S ON R.FK_State_Id = S.PK_State_Id
) AS Reservation
INNER JOIN
(
	SELECT	U.PK_User_Id AS [professor.id], U.Name AS [professor.name], U.Last_Name_1 AS [professor.last_name_1],
			U.Last_Name_2 AS [professor.last_name_2], U.Email AS [professor.email],
			RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [professor.full_name], 
            U.Gender AS [professor.gender], U.Phone AS [professor.phone], U.Username AS [professor.username],
            U.Created_At AS [professor.created_at], U.Updated_At AS [professor.updated_at], S.Name AS [professor.state]
	FROM Professors AS P
	INNER JOIN Users AS U ON P.FK_User_Id = U.PK_User_Id
	INNER JOIN States AS S ON U.FK_State_Id = S.PK_State_Id
) AS Professor ON Reservation.professor = Professor.[professor.id]
INNER JOIN
(
	SELECT L.PK_Laboratory_Id AS [laboratory.id], L.Name AS [laboratory.name], L.Seats AS [laboratory.seats], S.Name AS [laboratory.state], 
           L.Created_At AS [laboratory.created_at], L.Updated_At AS [laboratory.updated_at], L.Appointment_Priority AS [laboratory.appointment_priority], 
           L.Reservation_Priority AS [laboratory.reservation_priority]
	FROM Laboratories AS L
	INNER JOIN States AS S ON L.FK_State_Id = S.PK_State_Id
) AS Laboratory ON Reservation.laboratory = Laboratory.[laboratory.id]
LEFT OUTER JOIN
(
	SELECT S.PK_Software_Id AS [software.id], S.Code AS [software.code], S.Name AS [software.name], E.Name AS [software.state], 
           S.Created_At AS [software.created_at], S.Updated_At AS [software.updated_at]
	FROM Software AS S
	INNER JOIN States AS E ON S.FK_State_Id = E.PK_State_Id
) AS Software ON Reservation.software = Software.[software.id]
LEFT OUTER JOIN
(
	SELECT [Group].id AS [group.id], [Group].number AS [group.number], [Group].[period.value] AS [group.period.value], [Group].[period.type] AS [group.period.type], 
           [Group].[period.year] AS [group.period.year], [Group].state AS [group.state], [Group].created_at AS [group.created_at], 
           [Group].updated_at AS [group.updated_at], Course.[course.id] AS [group.course.id], Course.[course.code] AS [group.course.code], 
           Course.[course.name] AS [group.course.name], Course.[course.state] AS [group.course.state], Course.[course.created_at] AS [group.course.created_at], 
           Course.[course.updated_at] AS [group.course.updated_at], Professor.[professor.id] AS [group.professor.id], 
           Professor.[professor.name] AS [group.professor.name], Professor.[professor.last_name_1] AS [group.professor.last_name_1], 
           Professor.[professor.last_name_2] AS [group.professor.last_name_2], Professor.[professor.full_name] AS [group.professor.full_name], 
           Professor.[professor.email] AS [group.professor.email], Professor.[professor.gender] AS [group.professor.gender], 
           Professor.[professor.phone] AS [group.professor.phone], Professor.[professor.username] AS [group.professor.username], 
           Professor.[professor.state] AS [group.professor.state], Professor.[professor.created_at] AS [group.professor.created_at], 
           Professor.[professor.updated_at] AS [group.professor.updated_at]
	FROM
	(
		SELECT	G.PK_Group_Id AS id, G.Number AS number, G.FK_Course_Id AS course, G.FK_Professor_Id AS professor, P.Value AS [period.value], 
                PT.Name AS [period.type], G.Period_Year AS [period.year], S.Name AS state, G.Created_At AS created_at, G.Updated_At AS updated_at
        FROM Groups AS G
        INNER JOIN States AS S ON G.FK_State_Id = S.PK_State_Id
        INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
        INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
    ) AS [Group]
    INNER JOIN
	(
		SELECT C.PK_Course_Id AS [course.id], C.Code AS [course.code], C.Name AS [course.name], S.Name AS [course.state], 
                C.Created_At AS [course.created_at], C.Updated_At AS [course.updated_at]
        FROM Courses AS C
        INNER JOIN States AS S ON C.FK_State_Id = S.PK_State_Id
     ) AS Course ON [Group].course = Course.[course.id]
     INNER JOIN
     (
		SELECT	U.PK_User_Id AS [professor.id], U.Name AS [professor.name], U.Last_Name_1 AS [professor.last_name_1], 
                U.Last_Name_2 AS [professor.last_name_2], RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) 
                AS [professor.full_name], U.Email AS [professor.email], U.Gender AS [professor.gender], U.Phone AS [professor.phone], 
                U.Username AS [professor.username], S.Name AS [professor.state], U.Created_At AS [professor.created_at], 
                U.Updated_At AS [professor.updated_at]
		FROM Professors AS P
		INNER JOIN Users AS U ON P.FK_User_Id = U.PK_User_Id
		INNER JOIN States AS S ON U.FK_State_Id = S.PK_State_Id
	 ) AS Professor ON [Group].professor = Professor.[professor.id]
) AS [Group] ON Reservation.[group] = [Group].[group.id]
GO