USE [master]
GO

-- Create silabi login.
IF NOT EXISTS (SELECT loginname FROM syslogins WHERE name = 'SilabiLogin' and dbname = 'SiLabI')
BEGIN
	CREATE LOGIN [SilabiLogin] WITH PASSWORD = 'password', DEFAULT_DATABASE=[SiLabI], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
END
GO

-- Create administrator login.
IF NOT EXISTS (SELECT loginname FROM syslogins WHERE name = 'AdministratorLogin' and dbname = 'SiLabI')
BEGIN
	CREATE LOGIN [AdministratorLogin] WITH PASSWORD = 'password', DEFAULT_DATABASE=[SiLabI], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
END
GO

-- Create operator login.
IF NOT EXISTS (SELECT loginname FROM syslogins WHERE name = 'OperatorLogin' and dbname = 'SiLabI')
BEGIN
	CREATE LOGIN [OperatorLogin] WITH PASSWORD = 'password', DEFAULT_DATABASE=[SiLabI], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
END
GO

-- Create professor login.
IF NOT EXISTS (SELECT loginname FROM syslogins WHERE name = 'ProfessorLogin' and dbname = 'SiLabI')
BEGIN
	CREATE LOGIN [ProfessorLogin] WITH PASSWORD = 'password', DEFAULT_DATABASE=[SiLabI], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
END
GO

-- Create student login.
IF NOT EXISTS (SELECT loginname FROM syslogins WHERE name = 'StudentLogin' and dbname = 'SiLabI')
BEGIN
	CREATE LOGIN [StudentLogin] WITH PASSWORD = 'password', DEFAULT_DATABASE=[SiLabI], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
END
GO

USE [SiLabI]
GO

-- Create database users.
CREATE USER [Silabi] FOR LOGIN [SilabiLogin] WITH DEFAULT_SCHEMA=[dbo]
CREATE USER [Administrator] FOR LOGIN [AdministratorLogin] WITH DEFAULT_SCHEMA=[dbo]
CREATE USER [Operator] FOR LOGIN [OperatorLogin] WITH DEFAULT_SCHEMA=[dbo]
CREATE USER [Professor] FOR LOGIN [ProfessorLogin] WITH DEFAULT_SCHEMA=[dbo]
CREATE USER [Student] FOR LOGIN [StudentLogin] WITH DEFAULT_SCHEMA=[dbo]

-- Grant authentication permission to all users.
GRANT EXECUTE ON [dbo].[sp_Authenticate] TO [SiLabI]

-- Create profile_admin role.
CREATE ROLE [profile_admin] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'profile_admin', 'Administrator'
EXEC sp_addrolemember 'profile_admin', 'Operator'
EXEC sp_addrolemember 'profile_admin', 'Professor'
EXEC sp_addrolemember 'profile_admin', 'Student'
GRANT SELECT ON [dbo].[vw_Users] TO [profile_admin]
GRANT UPDATE ON [dbo].[Users] TO [profile_admin]
GRANT EXECUTE ON [dbo].[sp_GetUser] TO [profile_admin]
GRANT EXECUTE ON [dbo].[sp_GetUserByUsername] TO [profile_admin]
GRANT EXECUTE ON [dbo].[sp_UpdateUser] TO [profile_admin]

-- Create public_data_reader role.
CREATE ROLE [public_data_reader] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'public_data_reader', 'Administrator'
EXEC sp_addrolemember 'public_data_reader', 'Operator'
EXEC sp_addrolemember 'public_data_reader', 'Professor'
EXEC sp_addrolemember 'public_data_reader', 'Student'
GRANT SELECT ON [dbo].[vw_Software] TO [public_data_reader]
GRANT SELECT ON [dbo].[vw_Laboratories] TO [public_data_reader]
GRANT SELECT ON [dbo].[vw_Groups] TO [public_data_reader]
GRANT SELECT ON [dbo].[vw_Courses] TO [public_data_reader]
GRANT SELECT ON [dbo].[StudentsByGroup] TO [public_data_reader]
GRANT SELECT ON [dbo].[SoftwareByLaboratory] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetSoftware] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetSoftwares] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetSoftwaresCount] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetLaboratory] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetLaboratories] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetLaboratoriesCount] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetGroup] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetGroups] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetGroupsCount] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetCourse] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetCourses] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetCoursesCount] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetStudentsByGroup] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetSoftwareByLaboratory] TO [public_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetGroupsByStudent] TO [public_data_reader]

-- Create public_data_writer role.
CREATE ROLE [public_data_writer] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'public_data_writer', 'Administrator'
EXEC sp_addrolemember 'public_data_writer', 'Operator'
GRANT INSERT, UPDATE ON [dbo].[Software] TO [public_data_writer]
GRANT INSERT, UPDATE ON [dbo].[Laboratories] TO [public_data_writer]
GRANT INSERT, UPDATE ON [dbo].[Groups] TO [public_data_writer]
GRANT INSERT, UPDATE ON [dbo].[Courses] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateSoftware] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateSoftware] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteSoftware] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateLaboratory] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateLaboratory] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteLaboratory] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateGroup] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateGroup] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteGroup] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateCourse] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateCourse] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteCourse] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_AddStudentsToGroup] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateGroupStudents] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_RemoveStudentsFromGroup] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_AddSoftwareToLaboratory] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateLaboratorySoftware] TO [public_data_writer]
GRANT EXECUTE ON [dbo].[sp_RemoveSoftwareFromLaboratory] TO [public_data_writer]
GRANT EXECUTE ON TYPE::[dbo].[SoftwareList] TO [public_data_writer]
GRANT EXECUTE ON TYPE::[dbo].[UserList] TO [public_data_writer]

-- Create protected_data_reader role.
CREATE ROLE [protected_data_reader] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'protected_data_reader', 'Administrator'
EXEC sp_addrolemember 'protected_data_reader', 'Operator'
GRANT SELECT ON [dbo].[vw_Users] TO [protected_data_reader]
GRANT SELECT ON [dbo].[vw_Students] TO [protected_data_reader]
GRANT SELECT ON [dbo].[vw_Professors] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetUser] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetUserByUsername] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetUsers] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetUsersCount] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetStudent] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetStudentByUsername] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetStudents] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetStudentsCount] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetProfessor] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetProfessorByUsername] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetProfessors] TO [protected_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetProfessorsCount] TO [protected_data_reader]

-- Create protected_data_writer role.
CREATE ROLE [protected_data_writer] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'protected_data_writer', 'Administrator'
EXEC sp_addrolemember 'protected_data_writer', 'Operator'
GRANT INSERT, UPDATE ON [dbo].[Users] TO [protected_data_writer]
GRANT INSERT, UPDATE ON [dbo].[Students] TO [protected_data_writer]
GRANT INSERT, UPDATE ON [dbo].[Professors] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateUser] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateUser] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteUser] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateStudent] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateStudent] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteStudent] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateProfessor] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateProfessor] TO [protected_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteProfessor] TO [protected_data_writer]

-- Create private_data_reader role.
CREATE ROLE [private_data_reader] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'private_data_reader', 'Administrator'
GRANT SELECT ON [dbo].[vw_Operators] TO [private_data_reader]
GRANT SELECT ON [dbo].[vw_Administrators] TO [private_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetOperator] TO [private_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetOperators] TO [private_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetOperatorsCount] TO [private_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetAdministrator] TO [private_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetAdministrators] TO [private_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetAdministratorsCount] TO [private_data_reader]

-- Create private_data_writer role.
CREATE ROLE [private_data_writer] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'private_data_writer', 'Administrator'
GRANT INSERT, UPDATE ON [dbo].[Operators] TO [private_data_writer]
GRANT INSERT, UPDATE ON [dbo].[Administrators] TO [private_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateOperator] TO [private_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteOperator] TO [private_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateAdministrator] TO [private_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteAdministrator] TO [private_data_writer]

-- Create appointment_data_reader role.
CREATE ROLE [appointment_data_reader] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'appointment_data_reader', 'Administrador'
EXEC sp_addrolemember 'appointment_data_reader', 'Operador'
EXEC sp_addrolemember 'appointment_data_reader', 'Student'
GRANT SELECT ON [dbo].[vw_Appointments] TO [appointment_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetAppointment] TO [appointment_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetAppointments] TO [appointment_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetAppointmentsCount] TO [appointment_data_reader]

-- Create appointment_data_writer role.
CREATE ROLE [appointment_data_writer] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'appointment_data_writer', 'Administrador'
EXEC sp_addrolemember 'appointment_data_writer', 'Operador'
EXEC sp_addrolemember 'appointment_data_writer', 'Student'
GRANT INSERT, UPDATE ON [dbo].[Appointments] TO [appointment_data_writer]
GRANT SELECT ON [dbo].[StudentsByGroup] TO [appointment_data_writer]
GRANT SELECT ON [dbo].[fn_GetAvailableAppointments] TO [appointment_data_writer]
GRANT EXECUTE ON [dbo].[sp_GetGroupsByStudent] TO [appointment_data_writer]
GRANT EXECUTE ON [dbo].[sp_GetAvailableAppointments] TO [appointment_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateAppointment] TO [appointment_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateAppointment] TO [appointment_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteAppointment] TO [appointment_data_writer]

-- Create reservation_data_reader role.
CREATE ROLE [reservation_data_reader] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'reservation_data_reader', 'Administrador'
EXEC sp_addrolemember 'reservation_data_reader', 'Operador'
EXEC sp_addrolemember 'reservation_data_reader', 'Professor'
GRANT SELECT ON [dbo].[vw_Reservations] TO [reservation_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetReservation] TO [reservation_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetReservations] TO [reservation_data_reader]
GRANT EXECUTE ON [dbo].[sp_GetReservationsCount] TO [reservation_data_reader]

-- Create reservation_data_writer role.
CREATE ROLE [reservation_data_writer] AUTHORIZATION [dbo]
EXEC sp_addrolemember 'reservation_data_writer', 'Administrador'
EXEC sp_addrolemember 'reservation_data_writer', 'Operador'
EXEC sp_addrolemember 'reservation_data_writer', 'Professor'
GRANT INSERT, UPDATE ON [dbo].[Reservations] TO [reservation_data_writer]
GRANT EXECUTE ON [dbo].[sp_CreateReservation] TO [reservation_data_writer]
GRANT EXECUTE ON [dbo].[sp_UpdateReservation] TO [reservation_data_writer]
GRANT EXECUTE ON [dbo].[sp_DeleteReservation] TO [reservation_data_writer]

USE [master]
GO