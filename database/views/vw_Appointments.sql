IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Appointments]'))
	DROP VIEW [dbo].[vw_Appointments]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Appointments]
AS
SELECT	Appointment.id, Appointment.date, Appointment.created_at, Appointment.updated_at, Appointment.state, Appointment.attendance, Student.[student.id], 
		Student.[student.name], Student.[student.last_name_1], Student.[student.last_name_2], Student.[student.email], Student.[student.full_name], Student.[student.gender], 
		Student.[student.phone], Student.[student.username], Student.[student.created_at], Student.[student.updated_at], Student.[student.state], Laboratory.[laboratory.id], 
		Laboratory.[laboratory.name], Laboratory.[laboratory.seats], Laboratory.[laboratory.state], Laboratory.[laboratory.created_at], Laboratory.[laboratory.updated_at], 
		Laboratory.[laboratory.appointment_priority], Laboratory.[laboratory.reservation_priority], Software.[software.id], Software.[software.code], Software.[software.name], 
		Software.[software.state], Software.[software.created_at], Software.[software.updated_at], [Group].[group.id], [Group].[group.number], [Group].[group.period.value], 
		[Group].[group.period.type], [Group].[group.period.year], [Group].[group.state], [Group].[group.created_at], [Group].[group.updated_at], Course.[group.course.id], 
		Course.[group.course.code], Course.[group.course.name], Course.[group.course.state], Course.[group.course.created_at], Course.[group.course.updated_at], 
		Professor.[group.professor.id], Professor.[group.professor.name], Professor.[group.professor.last_name_1], Professor.[group.professor.last_name_2], 
		Professor.[group.professor.email], Professor.[group.professor.full_name], Professor.[group.professor.gender], Professor.[group.professor.phone], 
		Professor.[group.professor.username], Professor.[group.professor.state], Professor.[group.professor.created_at], Professor.[group.professor.updated_at]
FROM
(
	SELECT	A.PK_Appointment_Id AS id, A.Date AS date, A.Attendance AS attendance, A.Created_At AS created_at, A.Updated_At AS updated_at, 
			S.Name AS state, A.FK_Laboratory_Id AS laboratory, A.FK_Software_Id AS software, A.FK_Student_Id AS student, A.FK_Group_Id AS [group]
	FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
) AS Appointment
INNER JOIN
(
	SELECT	U.PK_User_Id AS [student.id], U.Name AS [student.name], U.Last_Name_1 AS [student.last_name_1], U.Last_Name_2 AS [student.last_name_2], 
            U.Email AS [student.email], RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [student.full_name], 
            U.Gender AS [student.gender], U.Phone AS [student.phone], U.Username AS [student.username], U.Created_At AS [student.created_at], 
            U.Updated_At AS [student.updated_at], E.Name AS [student.state]
	FROM Students AS S
	INNER JOIN Users AS U ON S.FK_User_Id = U.PK_User_Id
	INNER JOIN States AS E ON U.FK_State_Id = E.PK_State_Id
) AS Student ON Appointment.student = Student.[student.id]
INNER JOIN
(
	SELECT L.PK_Laboratory_Id AS [laboratory.id], L.Name AS [laboratory.name], L.Seats AS [laboratory.seats], S.Name AS [laboratory.state], 
           L.Created_At AS [laboratory.created_at], L.Updated_At AS [laboratory.updated_at], L.Appointment_Priority AS [laboratory.appointment_priority], 
           L.Reservation_Priority AS [laboratory.reservation_priority]
    FROM Laboratories AS L
    INNER JOIN States AS S ON L.FK_State_Id = S.PK_State_Id
) AS Laboratory ON Appointment.laboratory = Laboratory.[laboratory.id]
INNER JOIN
(
	SELECT	S.PK_Software_Id AS [software.id], S.Code AS [software.code], S.Name AS [software.name], E.Name AS [software.state], 
            S.Created_At AS [software.created_at], S.Updated_At AS [software.updated_at]
	FROM Software AS S
	INNER JOIN States AS E ON S.FK_State_Id = E.PK_State_Id
) AS Software ON Appointment.software = Software.[software.id]
INNER JOIN
(
	SELECT	G.PK_Group_Id AS [group.id], G.Number AS [group.number], G.FK_Course_Id AS [group.course], G.FK_Professor_Id AS [group.professor], 
            P.Value AS [group.period.value], PT.Name AS [group.period.type], G.Period_Year AS [group.period.year], S.Name AS [group.state], 
            G.Created_At AS [group.created_at], G.Updated_At AS [group.updated_at]
    FROM Groups AS G
    INNER JOIN States AS S ON G.FK_State_Id = S.PK_State_Id
    INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
    INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
) AS [Group] ON Appointment.[group] = [Group].[group.id]
INNER JOIN
(
	SELECT C.PK_Course_Id AS [group.course.id], C.Code AS [group.course.code], C.Name AS [group.course.name], S.Name AS [group.course.state], 
           C.Created_At AS [group.course.created_at], C.Updated_At AS [group.course.updated_at]
    FROM Courses AS C
    INNER JOIN States AS S ON C.FK_State_Id = S.PK_State_Id
) AS Course ON [Group].[group.course] = Course.[group.course.id]
INNER JOIN
(
	SELECT	U.PK_User_Id AS [group.professor.id], U.Name AS [group.professor.name], U.Last_Name_1 AS [group.professor.last_name_1], 
            U.Last_Name_2 AS [group.professor.last_name_2], U.Email AS [group.professor.email], 
            RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [group.professor.full_name], U.Gender AS [group.professor.gender], 
            U.Phone AS [group.professor.phone], U.Username AS [group.professor.username], S.Name AS [group.professor.state], 
            U.Created_At AS [group.professor.created_at], U.Updated_At AS [group.professor.updated_at]
    FROM Professors AS P
    INNER JOIN Users AS U ON P.FK_User_Id = U.PK_User_Id
    INNER JOIN States AS S ON U.FK_State_Id = S.PK_State_Id
) AS Professor ON [Group].[group.professor] = Professor.[group.professor.id]
GO