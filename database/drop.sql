USE [master];

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'WebService')
	DROP USER [WebService]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'WebServiceLogin')
	DROP LOGIN [WebServiceLogin]
GO

IF EXISTS (SELECT 1 FROM SYS.DATABASES WHERE NAME = 'SiLabI')
	DROP DATABASE SiLabI
	GO
GO