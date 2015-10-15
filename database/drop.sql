USE [master];

ALTER DATABASE [SiLabI]
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

ALTER DATABASE [SiLabI]
SET READ_ONLY
GO

ALTER DATABASE [SiLabI]
SET MULTI_USER
GO

IF EXISTS (SELECT 1 FROM SYS.DATABASES WHERE NAME = 'SiLabI')
	DROP DATABASE SiLabI
	GO
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'SilabiLogin')
	DROP LOGIN [SilabiLogin]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'AdministratorLogin')
	DROP LOGIN [AdministratorLogin]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'OperatorLogin')
	DROP LOGIN [OperatorLogin]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'ProfessorLogin')
	DROP LOGIN [ProfessorLogin]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'StudentLogin')
	DROP LOGIN [StudentLogin]
GO