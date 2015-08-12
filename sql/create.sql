/****** #####                 ##### ******/
/****** ##### CREATE DATABASE ##### ******/
/****** #####                 ##### ******/


USE [master]
GO

/****** Object:  Database [SiLabI]    Script Date: 08/11/2015 10:51:00 ******/
CREATE DATABASE [SiLabI] ON  PRIMARY 
( NAME = N'SiLabI', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\SiLabI.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SiLabI_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\SiLabI_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [SiLabI] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SiLabI].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [SiLabI] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [SiLabI] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [SiLabI] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [SiLabI] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [SiLabI] SET ARITHABORT OFF 
GO

ALTER DATABASE [SiLabI] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [SiLabI] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [SiLabI] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [SiLabI] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [SiLabI] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [SiLabI] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [SiLabI] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [SiLabI] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [SiLabI] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [SiLabI] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [SiLabI] SET  DISABLE_BROKER 
GO

ALTER DATABASE [SiLabI] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [SiLabI] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [SiLabI] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [SiLabI] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [SiLabI] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [SiLabI] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [SiLabI] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [SiLabI] SET  READ_WRITE 
GO

ALTER DATABASE [SiLabI] SET RECOVERY FULL 
GO

ALTER DATABASE [SiLabI] SET  MULTI_USER 
GO

ALTER DATABASE [SiLabI] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [SiLabI] SET DB_CHAINING OFF 
GO


/****** #####               ##### ******/
/****** ##### CREATE TABLES ##### ******/
/****** #####               ##### ******/


USE [SiLabI]
GO
/****** Object:  Table [dbo].[TransactionTypes]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransactionTypes](
	[PK_Transaction_Type_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NULL,
 CONSTRAINT [PK_TransactionTypes] PRIMARY KEY CLUSTERED 
(
	[PK_Transaction_Type_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[States]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[States](
	[PK_State_Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](30) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[PK_State_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PeriodTypes]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PeriodTypes](
	[PK_Period_Type_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Max_Value] [int] NOT NULL,
 CONSTRAINT [PK_PeriodTypes] PRIMARY KEY CLUSTERED 
(
	[PK_Period_Type_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[fn_CurrentIP]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_CurrentIP] ()
RETURNS varchar(255)
AS
BEGIN
    DECLARE @IP_Address varchar(255);

    SELECT @IP_Address = client_net_address
    FROM sys.dm_exec_connections
    WHERE Session_id = @@SPID;
    
    IF (@IP_Address = '<local machine>')
	BEGIN
		SET @IP_Address = '127.0.0.1';
	END

    Return @IP_Address;
END
GO
/****** Object:  Table [dbo].[Laboratories]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Laboratories](
	[PK_Laboratory_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[Seats] [int] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [PK_Laboratories] PRIMARY KEY CLUSTERED 
(
	[PK_Laboratory_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Courses](
	[PK_Course_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[PK_Course_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Periods]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Periods](
	[PK_Period_Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [int] NOT NULL,
	[FK_Period_Type_Id] [int] NOT NULL,
 CONSTRAINT [PK_Periods] PRIMARY KEY CLUSTERED 
(
	[PK_Period_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Software]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Software](
	[PK_Software_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [PK_Software] PRIMARY KEY CLUSTERED 
(
	[PK_Software_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[PK_User_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](70) NOT NULL,
	[Last_Name_1] [varchar](70) NOT NULL,
	[Last_Name_2] [varchar](70) NULL,
	[Username] [varchar](70) NOT NULL,
	[Password] [varchar](70) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](20) NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[PK_User_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Students]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Students](
	[University_Id] [varchar](20) NOT NULL,
	[FK_User_Id] [int] NOT NULL,
 CONSTRAINT [IX_Students_University_Id] UNIQUE NONCLUSTERED 
(
	[University_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Students_User_Id] UNIQUE NONCLUSTERED 
(
	[FK_User_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SoftwareByLaboratory]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoftwareByLaboratory](
	[FK_Software_Id] [int] NOT NULL,
	[FK_Laboratory_Id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Logs](
	[PK_Log_Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Old_Data] [varchar](max) NULL,
	[New_Data] [varchar](max) NULL,
	[IP_Address] [varchar](255) NOT NULL,
	[FK_User_Id] [int] NOT NULL,
	[FK_Transaction_Type_Id] [int] NOT NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[PK_Log_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Administrators]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administrators](
	[FK_User_Id] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [IX_Administrators_User_Id] UNIQUE NONCLUSTERED 
(
	[FK_User_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Professors]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Professors](
	[FK_User_Id] [int] NOT NULL,
 CONSTRAINT [IX_Professors_User_Id] UNIQUE NONCLUSTERED 
(
	[FK_User_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Operators]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operators](
	[FK_User_Id] [int] NOT NULL,
	[Period_Year] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	[FK_Period_Id] [int] NOT NULL,
 CONSTRAINT [IX_Operators] UNIQUE NONCLUSTERED 
(
	[FK_User_Id] ASC,
	[FK_Period_Id] ASC,
	[Period_Year] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[PK_Appointment_Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_Student_Id] [int] NOT NULL,
	[FK_Laboratory_Id] [int] NOT NULL,
	[FK_Software_Id] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
(
	[PK_Appointment_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[PK_Group_Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[Period_Year] [int] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_Course_Id] [int] NOT NULL,
	[FK_Professor_Id] [int] NOT NULL,
	[FK_Period_Id] [int] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[PK_Group_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Groups] UNIQUE NONCLUSTERED 
(
	[Number] ASC,
	[FK_Professor_Id] ASC,
	[FK_Course_Id] ASC,
	[FK_Period_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentsByGroup]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentsByGroup](
	[FK_Student_Id] [int] NOT NULL,
	[FK_Group_Id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_UserType]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_UserType](@UserId INT)
RETURNS VARCHAR(30)
AS
BEGIN
    DECLARE @Type VARCHAR(30);
    
    IF EXISTS (SELECT 1 FROM Students WHERE FK_User_Id = @UserId)
    BEGIN
		SET @Type = 'student'
    END
    
    IF EXISTS (SELECT 1 FROM Professors WHERE FK_User_Id = @UserId)
    BEGIN
		SET @Type = 'professor'
    END
    
    IF EXISTS (SELECT 1 FROM Operators WHERE FK_User_Id = @UserId)
    BEGIN
		SET @Type = 'operator'
    END
    
    IF EXISTS (SELECT 1 FROM Administrators WHERE FK_User_Id = @UserId)
    BEGIN
		SET @Type = 'administrator'
    END
    
    RETURN @Type;
END
GO
/****** Object:  Table [dbo].[Reservations]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservations](
	[PK_Reservation_Id] [int] IDENTITY(1,1) NOT NULL,
	[Start_Time] [datetime] NOT NULL,
	[End_Time] [datetime] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_Professor_Id] [int] NOT NULL,
	[FK_Group_Id] [int] NULL,
	[FK_Laboratory_Id] [int] NOT NULL,
	[FK_Software_Id] [int] NULL,
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED 
(
	[PK_Reservation_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_Authenticate]    Script Date: 08/11/2015 20:18:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[sp_Authenticate]
(
@username VARCHAR(70),    -- The username.
@password VARCHAR(70)     -- The password.
)
AS
BEGIN
	SELECT TOP 1
		U.PK_User_Id, U.Username, U.Name, U.Last_Name_1, U.Last_Name_2, U.Gender, U.Email,
		U.Phone, U.Created_At, U.Updated_At, dbo.fn_UserType(U.PK_User_Id) AS Type
	FROM Users AS U
	INNER JOIN States AS S ON S.PK_State_Id = U.FK_State_Id 
	WHERE U.Username = @username AND U.Password = @password AND S.Name = 'ACTIVE';
END
GO
/****** Object:  Default [DF_Appointments_Created_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Appointments_Updated_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Courses_Created_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Courses] ADD  CONSTRAINT [DF_Courses_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Courses_Updated_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Courses] ADD  CONSTRAINT [DF_Courses_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Groups_Period_Year]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Period_Year]  DEFAULT (datepart(year,getdate())) FOR [Period_Year]
GO
/****** Object:  Default [DF_Groups_Created_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Groups_Updated_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Laboratories_Created_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Laboratories_Updated_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Logs_Date]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [DF_Logs_Date]  DEFAULT (getdate()) FOR [Date]
GO
/****** Object:  Default [DF_Logs_IP_Address]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [DF_Logs_IP_Address]  DEFAULT ('dbo.fn_CurrentIP') FOR [IP_Address]
GO
/****** Object:  Default [DF_Operators_Period_Year]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Operators] ADD  CONSTRAINT [DF_Operators_Period_Year]  DEFAULT (datepart(year,getdate())) FOR [Period_Year]
GO
/****** Object:  Default [DF_Reservations_Created_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Reservations] ADD  CONSTRAINT [DF_Reservations_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Reservations_Updated_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Reservations] ADD  CONSTRAINT [DF_Reservations_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Software_Created_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Software] ADD  CONSTRAINT [DF_Software_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Software_Updated_At]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Software] ADD  CONSTRAINT [DF_Software_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Users_created_at]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_created_at]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Users_updated_at]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_updated_at]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  ForeignKey [FK_Administrators_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Administrators]  WITH CHECK ADD  CONSTRAINT [FK_Administrators_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Administrators] CHECK CONSTRAINT [FK_Administrators_States]
GO
/****** Object:  ForeignKey [FK_Administrators_Users]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Administrators]  WITH CHECK ADD  CONSTRAINT [FK_Administrators_Users] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Users] ([PK_User_Id])
GO
ALTER TABLE [dbo].[Administrators] CHECK CONSTRAINT [FK_Administrators_Users]
GO
/****** Object:  ForeignKey [FK_Appointments_Laboratories]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Laboratories]
GO
/****** Object:  ForeignKey [FK_Appointments_Software]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Software] FOREIGN KEY([FK_Software_Id])
REFERENCES [dbo].[Software] ([PK_Software_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Software]
GO
/****** Object:  ForeignKey [FK_Appointments_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_States]
GO
/****** Object:  ForeignKey [FK_Appointments_Students]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Students] FOREIGN KEY([FK_Student_Id])
REFERENCES [dbo].[Students] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Students]
GO
/****** Object:  ForeignKey [FK_Courses_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_States]
GO
/****** Object:  ForeignKey [FK_Groups_Courses]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Courses] FOREIGN KEY([FK_Course_Id])
REFERENCES [dbo].[Courses] ([PK_Course_Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Courses]
GO
/****** Object:  ForeignKey [FK_Groups_Periods]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Periods] FOREIGN KEY([FK_Period_Id])
REFERENCES [dbo].[Periods] ([PK_Period_Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Periods]
GO
/****** Object:  ForeignKey [FK_Groups_Professors]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Professors] FOREIGN KEY([FK_Professor_Id])
REFERENCES [dbo].[Professors] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Professors]
GO
/****** Object:  ForeignKey [FK_Laboratories_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Laboratories]  WITH CHECK ADD  CONSTRAINT [FK_Laboratories_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Laboratories] CHECK CONSTRAINT [FK_Laboratories_States]
GO
/****** Object:  ForeignKey [FK_Logs_TransactionTypes]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD  CONSTRAINT [FK_Logs_TransactionTypes] FOREIGN KEY([FK_Transaction_Type_Id])
REFERENCES [dbo].[TransactionTypes] ([PK_Transaction_Type_Id])
GO
ALTER TABLE [dbo].[Logs] CHECK CONSTRAINT [FK_Logs_TransactionTypes]
GO
/****** Object:  ForeignKey [FK_Logs_Users]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD  CONSTRAINT [FK_Logs_Users] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Users] ([PK_User_Id])
GO
ALTER TABLE [dbo].[Logs] CHECK CONSTRAINT [FK_Logs_Users]
GO
/****** Object:  ForeignKey [FK_Operators_Periods]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_Periods] FOREIGN KEY([FK_Period_Id])
REFERENCES [dbo].[Periods] ([PK_Period_Id])
GO
ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_Periods]
GO
/****** Object:  ForeignKey [FK_Operators_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_States]
GO
/****** Object:  ForeignKey [FK_Operators_Students]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_Students] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Students] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_Students]
GO
/****** Object:  ForeignKey [FK_Periods_PeriodTypes]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Periods]  WITH CHECK ADD  CONSTRAINT [FK_Periods_PeriodTypes] FOREIGN KEY([FK_Period_Type_Id])
REFERENCES [dbo].[PeriodTypes] ([PK_Period_Type_Id])
GO
ALTER TABLE [dbo].[Periods] CHECK CONSTRAINT [FK_Periods_PeriodTypes]
GO
/****** Object:  ForeignKey [FK_Professors_Users]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Professors]  WITH CHECK ADD  CONSTRAINT [FK_Professors_Users] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Users] ([PK_User_Id])
GO
ALTER TABLE [dbo].[Professors] CHECK CONSTRAINT [FK_Professors_Users]
GO
/****** Object:  ForeignKey [FK_Reservations_Groups]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Groups] FOREIGN KEY([FK_Group_Id])
REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Groups]
GO
/****** Object:  ForeignKey [FK_Reservations_Laboratories]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Laboratories]
GO
/****** Object:  ForeignKey [FK_Reservations_Professors]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Professors] FOREIGN KEY([FK_Professor_Id])
REFERENCES [dbo].[Professors] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Professors]
GO
/****** Object:  ForeignKey [FK_Reservations_Software]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Software] FOREIGN KEY([FK_Software_Id])
REFERENCES [dbo].[Software] ([PK_Software_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Software]
GO
/****** Object:  ForeignKey [FK_Reservations_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_States]
GO
/****** Object:  ForeignKey [FK_Software_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Software]  WITH CHECK ADD  CONSTRAINT [FK_Software_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Software] CHECK CONSTRAINT [FK_Software_States]
GO
/****** Object:  ForeignKey [FK_SoftwareByLaboratory_Laboratories]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[SoftwareByLaboratory]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareByLaboratory_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO
ALTER TABLE [dbo].[SoftwareByLaboratory] CHECK CONSTRAINT [FK_SoftwareByLaboratory_Laboratories]
GO
/****** Object:  ForeignKey [FK_SoftwareByLaboratory_Software]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[SoftwareByLaboratory]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareByLaboratory_Software] FOREIGN KEY([FK_Software_Id])
REFERENCES [dbo].[Software] ([PK_Software_Id])
GO
ALTER TABLE [dbo].[SoftwareByLaboratory] CHECK CONSTRAINT [FK_SoftwareByLaboratory_Software]
GO
/****** Object:  ForeignKey [FK_Students_Users]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Users] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Users] ([PK_User_Id])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Users]
GO
/****** Object:  ForeignKey [FK_StudentsByGroup_Groups]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[StudentsByGroup]  WITH CHECK ADD  CONSTRAINT [FK_StudentsByGroup_Groups] FOREIGN KEY([FK_Group_Id])
REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO
ALTER TABLE [dbo].[StudentsByGroup] CHECK CONSTRAINT [FK_StudentsByGroup_Groups]
GO
/****** Object:  ForeignKey [FK_StudentsByGroup_Students]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[StudentsByGroup]  WITH CHECK ADD  CONSTRAINT [FK_StudentsByGroup_Students] FOREIGN KEY([FK_Student_Id])
REFERENCES [dbo].[Students] ([FK_User_Id])
GO
ALTER TABLE [dbo].[StudentsByGroup] CHECK CONSTRAINT [FK_StudentsByGroup_Students]
GO
/****** Object:  ForeignKey [FK_Users_States]    Script Date: 08/11/2015 20:18:52 ******/
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_States]
GO



/****** #####             ##### ******/
/****** ##### CREATE USER ##### ******/
/****** #####             ##### ******/


USE [master]
GO

CREATE LOGIN [WebServiceLogin] WITH PASSWORD = 'password', DEFAULT_DATABASE=[SiLabI], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
GO

USE [SiLabI]
GO

CREATE USER [WebService] FOR LOGIN [WebServiceLogin]
GO

GRANT EXECUTE TO WebService
GO

EXEC sp_addrolemember 'db_datareader', 'WebService'
GO

USE [master]
GO