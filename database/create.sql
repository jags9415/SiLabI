/****** #####                 ##### ******/
/****** ##### CREATE DATABASE ##### ******/
/****** #####                 ##### ******/


USE [master]
GO

IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'SiLabI')
BEGIN
/****** Object:  Database [SiLabI]    Script Date: 08/11/2015 10:51:00 ******/
CREATE DATABASE [SiLabI] ON  PRIMARY 
( NAME = N'SiLabI', FILENAME = N'C:\SiLabI\SiLabI.mdf' , SIZE = 5072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SiLabI_log', FILENAME = N'C:\SiLabI\SiLabI_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END
GO

ALTER DATABASE [SiLabI] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
BEGIN
	EXEC [SiLabI].[dbo].[sp_fulltext_database] @action = 'enable'
END
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


/****** #####                ##### ******/
/****** ##### CREATE OBJECTS ##### ******/
/****** #####                ##### ******/


USE [SiLabI]
GO
/****** Object:  UserDefinedTableType [dbo].[SoftwareList]    Script Date: 10/14/2015 08:20:27 ******/
CREATE TYPE [dbo].[SoftwareList] AS TABLE(
	[Code] [varchar](20) NULL
)
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAll]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAll]
(
	@table		VARCHAR(MAX),	-- The table name.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX);
	DECLARE @start INT, @end INT;

	-- SET default values.	
	SET @fields = COALESCE(@fields, '');
	SET @order_by = COALESCE(@order_by, '');
	SET @where = COALESCE(@where, '');
	SET @page = COALESCE(@page, 1);
	SET @limit = COALESCE(@limit, 20);
	
	-- SET the start and end rows.
	SET @start = ((@page - 1) * @limit) + 1;
	SET @end = @page * @limit;
	
	-- SET default fields to retrieve.
	IF (@fields = '')
	BEGIN
		SET @fields = '*';
	END
	
	-- SET default ORDER BY query.
	IF (@order_by = '') SET @order_by = 'RAND()';
	
	-- SET default WHERE query.
	IF (@where = '') SET @where = '1=1';

	SET @sql =	'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_by + ') AS rn, ' +
				@fields + ' FROM ' + @table +
				' WHERE ' + @where + ') AS Sub WHERE rn >= ' + CAST(@start AS VARCHAR) +
				' AND (rn <= ' + CAST(@end AS VARCHAR) + ' OR ' + CAST(@limit AS VARCHAR) + ' < 0)';
				
	EXECUTE(@sql);
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCount]
(
	@table		VARCHAR(MAX),	-- The table name.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX);

	-- SET default values.	
	IF (COALESCE(@where, '') = '')
		SET @where = '1=1';

	SET @sql =	'SELECT COUNT(1) AS count FROM ' + @table + ' WHERE ' + @where;
				
	EXECUTE(@sql); 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetOne]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetOne]
(
	@table		VARCHAR(MAX),	-- The table name.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX);
	DECLARE @start INT, @end INT;

	-- SET default values.	
	SET @fields = COALESCE(@fields, '');
	SET @where = COALESCE(@where, '');
	
	-- SET default fields to retrieve.
	IF (@fields = '') SET @fields = '*';
	
	-- SET default WHERE query.
	IF (@where = '') SET @where = '1=1';

	SET @sql = 'SELECT TOP 1 ' + @fields + ' FROM ' + @table + ' WHERE ' + @where;
				
	EXECUTE(@sql);
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_IsDateInPeriod]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_IsDateInPeriod]
(
	@date         DATE,        -- The day to check.
	@period_value INT,         -- The period value (1, 2, 3, ...)
	@period_type  VARCHAR(50), -- The period type (Bimestre, Trimestre, ...)
	@period_year  INT          -- The period year (2015, 2016, ...)
)
RETURNS BIT
AS
BEGIN
	DECLARE @start_date DATE, @end_date DATE, @offset INT;
	
	SELECT @start_date = CAST(CAST(@period_year AS VARCHAR) AS DATE)
	SELECT @end_date = CAST(CAST(@period_year AS VARCHAR) AS DATE)
	
	SELECT @offset =
      CASE @period_type
         WHEN 'BIMESTRE' THEN 2
         WHEN 'TRIMESTRE' THEN 3
         WHEN 'CUATRIMESTRE' THEN 4
         WHEN 'SEMESTRE' THEN 6
      END;
	
	SET @start_date = DATEADD(MONTH, (@offset * (@period_value - 1)), @start_date)
	SET @end_date = DATEADD(MONTH, (@offset * @period_value), @end_date)
	SET @end_date = DATEADD(DAY, -1, @end_date)

	IF (@date BETWEEN @start_date AND @end_date) RETURN 1
	
	RETURN 0
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_IsNull]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 * Recieve a value and a table field.
 * If the value is empty then return NULL.
 * If the value is NULL then return the field.
 * Else return the value.
*/
CREATE FUNCTION [dbo].[fn_IsNull]
(
	@param sql_variant,
	@field sql_variant
)
RETURNS sql_variant
AS
BEGIN
	DECLARE @value sql_variant;
	
	SELECT @value =
	CASE 
		WHEN @param IS NULL THEN @field
		WHEN @param = '' THEN NULL
		WHEN @param = 0 THEN NULL
		ELSE @param
	END
	
	RETURN @value;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetCurrentIPAddress]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetCurrentIPAddress] ()
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
/****** Object:  UserDefinedFunction [dbo].[fn_DateCeiling]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_DateCeiling]
(
  @seed DATETIME
, @part VARCHAR(2)
)
RETURNS DATETIME
AS
BEGIN


   DECLARE
       @second     DATETIME,
       @minute     DATETIME,
       @hour       DATETIME,
       @day        DATETIME,
       @month      DATETIME,
       @year       DATETIME,
       @retDate    DATETIME


   -- NOTE: at first this looks like this code would be more efficient if the
   -- code for each date part were split into IF..THEN..ELSE
   --  but it is necessary to perform the central block so that the effect
   --  is to subtract all of the smalller date part values from the starting point
   -- ie for Minute ceiling:
   -- Add a minute
   -- Subtract All Seconds
   -- Subtract all Milliseconds


   --add one unit of the date part to be rounded
   SELECT @seed =    CASE
                       WHEN @part = 'ss' THEN DATEADD(ss,1,@seed)
                       WHEN @part = 'mi' THEN DATEADD(mi,1,@seed)
                       WHEN @part = 'hh' THEN DATEADD(hh,1,@seed)
                       WHEN @part = 'dd' THEN DATEADD(dd,1,@seed)      
                       WHEN @part = 'mm' THEN DATEADD(mm,1,@seed)
                       WHEN @part = 'yy' THEN DATEADD(yy,1,@seed)
                   END

   -- now for each level of date part, subtract all of the units of the next smallest date part
   SELECT @second = DATEADD(ms,-1*DATEPART(ms,@seed ), @seed)
   SELECT @minute = DATEADD(ss,-1*DATEPART(ss,@second), @second)
   SELECT @hour = DATEADD(mi,-1*DATEPART(mi,@minute), @minute)
   SELECT @day = DATEADD(hh,-1*DATEPART(hh,@hour ), @hour)
   SELECT @month = DATEADD(dd,-1*(DAY(@day)-1), @day)
   SELECT @year = DATEADD(mm,-1*(MONTH(@month)-1), @month)

   -- and load the return variable with the date part required
   SELECT @retDate = CASE
                           WHEN @part = 'ss' THEN @second
                           WHEN @part = 'mi' THEN @minute
                           WHEN @part = 'hh' THEN @hour
                           WHEN @part = 'dd' THEN @day
                           WHEN @part = 'mm' THEN @month
                           WHEN @part = 'yy' THEN @year
                       END

   RETURN @retDate
END
GO
/****** Object:  Table [dbo].[PeriodTypes]    Script Date: 10/14/2015 08:20:27 ******/
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
/****** Object:  Table [dbo].[Logs]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Logs](
	[PK_Log_Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Operation] [char](1) NOT NULL,
	[Table] [varchar](50) NOT NULL,
	[Old_Data] [xml] NOT NULL,
	[New_Data] [xml] NOT NULL,
	[Date] [datetime] NOT NULL,
	[FK_User_Id] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[States]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[States](
	[PK_State_Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](30) NOT NULL,
	[Name] [varchar](30) NOT NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[PK_State_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedTableType [dbo].[UserList]    Script Date: 10/14/2015 08:20:27 ******/
CREATE TYPE [dbo].[UserList] AS TABLE(
	[Username] [varchar](70) NULL
)
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProfessorsCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetProfessorsCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Professors', @where; 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProfessors]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetProfessors]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Professors', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProfessorByUsername]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetProfessorByUsername]
(
	@requester_id INT,		-- The identity of the requester user.
	@username VARCHAR(70),	-- The username.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'username=''' + @username + '''';
	EXEC dbo.sp_GetOne 'vw_Professors', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProfessor]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetProfessor]
(
	@requester_id INT,		-- The identity of the requester user.
	@id INT,				-- The identification.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Professors', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetOperatorsCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetOperatorsCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Operators', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetOperators]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetOperators]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Operators', @fields, @order_by, @where, @page, @limit; 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetOperator]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetOperator]
(
	@requester_id INT,		-- The identity of the requester user.
	@operator_id INT,		-- The Operator Id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@operator_id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Operators', @fields, @where;
END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/14/2015 08:20:27 ******/
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
/****** Object:  Table [dbo].[Laboratories]    Script Date: 10/14/2015 08:20:27 ******/
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
	[Appointment_Priority] [int] NULL,
	[Reservation_Priority] [int] NULL,
 CONSTRAINT [PK_Laboratories] PRIMARY KEY CLUSTERED 
(
	[PK_Laboratory_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Periods]    Script Date: 10/14/2015 08:20:27 ******/
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
/****** Object:  UserDefinedFunction [dbo].[fn_GetStateID]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetStateID]
(
	@state_type VARCHAR(30),
	@state_name VARCHAR(30)
)
RETURNS INT
AS
BEGIN
	DECLARE @state_id INT;
	
	SELECT @state_id = PK_State_Id FROM States
	WHERE Type = @state_type AND Name = @state_name;
	
	RETURN @state_id
END
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 10/14/2015 08:20:27 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Courses_Code] UNIQUE NONCLUSTERED 
(
	[PK_Course_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUsersCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetUsersCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Users', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUsers]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetUsers]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Users', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUserByUsername]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetUserByUsername]
(
	@requester_id INT,			-- The identity of the requester user.
	@username VARCHAR(70),	-- The Username.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'username=''' + @username + '''';
	EXEC dbo.sp_GetOne 'vw_Users', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUser]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetUser]
(
	@requester_id INT,		-- The identity of the requester user.
	@id		INT,			-- The user identification.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Users', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudentsCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetStudentsCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Students', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudentsByGroup]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetStudentsByGroup]
(
	@requester_id INT,			-- The identity of the requester user.
	@group		INT,			-- The group id.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	DECLARE @where_statement VARCHAR(MAX);
	SET @where = COALESCE(@where, '');
	SET @where_statement = 'id IN (SELECT FK_Student_Id FROM StudentsByGroup WHERE FK_Group_Id = ' + CAST(@group AS VARCHAR) + ')';
	IF (@where <> '') SET @where_statement = @where_statement + ' AND ' + @where;
	EXEC dbo.sp_GetAll 'vw_Students', @fields, @order_by, @where_statement, NULL, -1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudents]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetStudents]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Students', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudentByUsername]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetStudentByUsername]
(
	@requester_id INT,			-- The identity of the requester user.
	@username VARCHAR(70),	-- The username.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'username=''' + @username + '''';
	EXEC dbo.sp_GetOne 'vw_Students', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudent]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetStudent]
(
	@requester_id INT,		-- The identity of the requester user.
	@id INT,				-- The identification.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Students', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSoftwaresCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSoftwaresCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Software', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSoftwares]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSoftwares]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Software', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSoftwareByLaboratory]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSoftwareByLaboratory]
(
	@requester_id INT,			-- The identity of the requester user.
	@laboratory		INT,		-- The laboratory id.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	DECLARE @where_statement VARCHAR(MAX);
	SET @where = COALESCE(@where, '');
	SET @where_statement = 'id IN (SELECT FK_Software_Id FROM SoftwareByLaboratory WHERE FK_Laboratory_Id = ' + CAST(@laboratory AS VARCHAR) + ')';
	IF (@where <> '') SET @where_statement = @where_statement + ' AND ' + @where;
	EXEC dbo.sp_GetAll 'vw_Software', @fields, @order_by, @where_statement, NULL, -1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSoftwareByCode]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSoftwareByCode]
(
	@requester_id INT,		-- The identity of the requester user.
	@code VARCHAR(20),		-- The software code.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'code=''' + @code + '''';
	EXEC dbo.sp_GetOne 'vw_Software', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSoftware]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSoftware]
(
	@requester_id INT,		-- The identity of the requester user.
	@id		INT,			-- The software id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Software', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetReservationsCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetReservationsCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Reservations', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetReservations]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetReservations]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Reservations', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLaboratory]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetLaboratory]
(
	@requester_id INT,		-- The identity of the requester user.
	@id	INT,				-- The laboratory id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Laboratories', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLaboratoriesCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetLaboratoriesCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Laboratories', @where; 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLaboratories]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetLaboratories]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Laboratories', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetGroupsCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetGroupsCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Groups', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAppointmentsCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAppointmentsCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Appointments', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAppointments]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAppointments]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Appointments', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetGroups]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetGroups]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Groups', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetGroup]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetGroup]
(
	@requester_id INT,		-- The identity of the requester user.
	@id		INT,			-- The group id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Groups', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCoursesCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCoursesCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Courses', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCourses]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCourses]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Courses', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCourse]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCourse]
(
	@requester_id INT,		-- The identity of the requester user.
	@id		INT,			-- The course id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Courses', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAdministratorsCount]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAdministratorsCount]
(
	@requester_id INT,			-- The identity of the requester user.
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause.
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Administrators', @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAdministrators]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAdministrators]
(
	@requester_id INT,			-- The identity of the requester user.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page		INT,			-- The page number. [Nullable]
	@limit		INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Administrators', @fields, @order_by, @where, @page, @limit;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAdministrator]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAdministrator]
(
	@requester_id INT,		-- The identity of the requester user.
	@user_id INT,			-- The user id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@user_id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Administrators', @fields, @where;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetDateTimeRange]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetDateTimeRange]
(
	@start DATETIME,	-- The start date.
	@weeks INT			-- The weeks offset.
)
RETURNS @daterange TABLE ([date] DATETIME)
AS
BEGIN 
	DECLARE @start_date DATETIME = dbo.fn_DateCeiling(@start, 'hh');
	DECLARE @end_date DATETIME = DATEADD(WEEK, @weeks, @start_date);
	DECLARE @datetime DATETIME, @weekday INT;

	IF DATEPART(HOUR, @start_date) < 8
	BEGIN
		SET @start_date = CAST(@start_date AS DATE)
		SET @start_date = DATEADD(HOUR, 8, @start_date)
	END

	WHILE @start_date < @end_date
	BEGIN
		SET @weekday = DATEPART(dw, @start_date)
		
		-- Do not include Saturdays or Sundays.
		IF @weekday != 1 AND @weekday != 7
		BEGIN
			SET @datetime = @start_date;
			WHILE DATEPART(HOUR, @datetime) <= 17
			BEGIN
				INSERT INTO @daterange VALUES (@datetime);
				SET @datetime = DATEADD(HOUR, 1, @datetime);
			END
		END
		
		SET @start_date = DATEADD(DAY, 1, CAST(@start_date AS DATE))
		SET @start_date = DATEADD(HOUR, 8, @start_date)
	END
	
	RETURN
END
GO
/****** Object:  Table [dbo].[Software]    Script Date: 10/14/2015 08:20:27 ******/
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
/****** Object:  Table [dbo].[SoftwareByLaboratory]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoftwareByLaboratory](
	[FK_Software_Id] [int] NOT NULL,
	[FK_Laboratory_Id] [int] NOT NULL,
 CONSTRAINT [IX_SoftwareByLaboratory] UNIQUE NONCLUSTERED 
(
	[FK_Software_Id] ASC,
	[FK_Laboratory_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Professors]    Script Date: 10/14/2015 08:20:27 ******/
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
/****** Object:  StoredProcedure [dbo].[sp_CreateCourse]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateCourse]
(
	@requester_id	INT,			-- The identity of the requester user.
	@name			VARCHAR(500),	-- The course name.
	@code			VARCHAR(20)		-- The course code.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if course exists.
	IF EXISTS (SELECT 1 FROM Courses WHERE Code = @code)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un curso con el código %s.', 15, 1, @code);
		RETURN -1;
	END
	
	DECLARE @id INT;
	INSERT INTO Courses (Name, Code, FK_State_Id) VALUES
	(@name, @code, dbo.fn_GetStateID('COURSE', 'Activo'));
	SET @id = SCOPE_IDENTITY();
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetCourse @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateSoftware]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateSoftware]
(
	@requester_id INT,		-- The identity of the requester user.
	@name	VARCHAR(500),	-- The software name.
	@code	VARCHAR(20)		-- The software code.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if software exists.
	IF EXISTS (SELECT 1 FROM Software WHERE Code = @code)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un software con el código %s.', 15, 1, @code);
		RETURN -1;
	END
	
	DECLARE @id INT;
	INSERT INTO Software (Name, Code, FK_State_Id) VALUES
	(@name, @code, dbo.fn_GetStateID('SOFTWARE', 'Activo'));
	SET @id = SCOPE_IDENTITY();
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetSoftware @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAvailableAppointments]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAvailableAppointments]
(
	@requester_id INT,			-- The identity of the requester user.
	@username	VARCHAR(70),    -- The username of the student who is making the appointment.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @id INT, @sql NVARCHAR(MAX);
	SELECT TOP 1 @id = PK_User_Id FROM Users WHERE Username = @username;
	
	IF @id IS NULL
	BEGIN
		RAISERROR('Usuario no encontrado.', 15, 1);
		RETURN -1;
	END
	
	-- SET default values.	
	SET @fields = COALESCE(@fields, '');
	SET @order_by = COALESCE(@order_by, '');
	SET @where = COALESCE(@where, '');
	
	-- SET default fields to retrieve.
	IF (@fields = '') SET @fields = '*';
	
	-- SET default ORDER BY query.
	IF (@order_by = '') SET @order_by = '[date] ASC';
	
	-- SET default WHERE query.
	IF (@where = '') SET @where = '1=1';
	
	SET @sql = 'SELECT ' + @fields + ' FROM dbo.fn_GetAvailableAppointments(' + CAST(@id AS VARCHAR) + ') WHERE ' + @where + ' ORDER BY ' + @order_by;
				
	EXECUTE(@sql);
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteCourse]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteCourse]
(
	@requester_id INT,	-- The identity of the requester user.
	@id		INT			-- The course id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if course exists.
	IF NOT EXISTS (SELECT 1 FROM Courses WHERE PK_Course_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Curso no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Courses
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('COURSE', 'Inactivo'), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE [PK_Course_Id] = @id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteLaboratory]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteLaboratory]
(
	@requester_id INT,	-- The identity of the requester user.
	@id		INT			-- The laboratory id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if laboratory exists.
	IF NOT EXISTS (SELECT 1 FROM Laboratories WHERE PK_Laboratory_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Laboratorio no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Laboratories
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('LABORATORY', 'Inactivo'), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE [PK_Laboratory_Id] = @id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateUser]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateUser]
(
	@id				INT OUTPUT,		-- The inserted row id will be saved here.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20)		-- The phone. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO Users (Name, Last_Name_1, Last_Name_2, Username, Password, Email, Phone, Gender, FK_State_Id, Created_At, Updated_At)
	SELECT 
	@name, 
	@last_name_1, 
	CAST(dbo.fn_IsNull(@last_name_2, NULL) AS VARCHAR(70)), 
	@username, 
	@password, 
	CAST(dbo.fn_IsNull(@email, NULL) AS VARCHAR(100)),
	CAST(dbo.fn_IsNull(@phone, NULL) AS VARCHAR(20)),
	@gender, 
	S.PK_State_Id, 
	GETDATE(),
	GETDATE()
	FROM States AS S
	WHERE S.Type = 'USER' AND S.Name = 'Activo';
	
	SET @id = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteSoftware]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteSoftware]
(
	@requester_id	INT,	-- The identity of the requester user.
	@id				INT		-- The software id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if software exists.
	IF NOT EXISTS (SELECT 1 FROM Software WHERE PK_Software_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Software no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Software
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('SOFTWARE', 'Inactivo'), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE [PK_Software_Id] = @id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  Table [dbo].[Administrators]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administrators](
	[FK_User_Id] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
 CONSTRAINT [IX_Administrators_User_Id] UNIQUE NONCLUSTERED 
(
	[FK_User_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Laboratories]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Laboratories]
AS
SELECT     L.PK_Laboratory_Id AS id, L.Name, L.Seats, S.Name AS state, L.Created_At, L.Updated_At, L.Appointment_Priority, L.Reservation_Priority
FROM         dbo.Laboratories AS L INNER JOIN
                      dbo.States AS S ON L.FK_State_Id = S.PK_State_Id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "L"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 179
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "S"
            Begin Extent = 
               Top = 6
               Left = 257
               Bottom = 99
               Right = 408
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Laboratories'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Laboratories'
GO
/****** Object:  View [dbo].[vw_Software]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Software] AS
SELECT S.PK_Software_Id AS [id], S.Code AS [code], S.Name AS [name], E.Name AS [state],
S.Created_At AS [created_at], S.Updated_At AS [updated_at] 
FROM Software AS S
INNER JOIN States AS E ON S.FK_State_Id = E.PK_State_Id;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateCourse]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateCourse]
(
	@requester_id INT,			-- The identity of the requester user.
	@id		INT,			-- The course id.
	@name	VARCHAR(500),	-- The course name.
	@code	VARCHAR(20),	-- The course code.
	@state	VARCHAR(30)		-- The course state.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if course exists.
	IF NOT EXISTS (SELECT 1 FROM Courses WHERE PK_Course_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Curso no encontrado.', 15, 1);
		RETURN -1;
	END
	
	-- Check if course exists.
	IF EXISTS (SELECT 1 FROM Courses WHERE Code = @code AND PK_Course_Id <> @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un curso con el código %s.', 15, 1, @code);
		RETURN -1;
	END
	
	UPDATE Courses
	SET [Code] = ISNULL(@code, [Code]),
	[Name] = ISNULL(@name, [Name]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('COURSE', @state), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE [PK_Course_Id] = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetCourse @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateSoftware]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateSoftware]
(
	@requester_id INT,		-- The identity of the requester user.
	@id		INT,			-- The software id.
	@name	VARCHAR(500),	-- The software name.
	@code	VARCHAR(20),	-- The software code.
	@state	VARCHAR(30)		-- The software state.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if software exists.
	IF NOT EXISTS (SELECT 1 FROM Software WHERE PK_Software_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Software no encontrado.', 15, 1);
		RETURN -1;
	END
	
	-- Check if software exists.
	IF EXISTS (SELECT 1 FROM Software WHERE Code = @code AND PK_Software_Id <> @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un software con el código %s.', 15, 1, @code);
		RETURN -1;
	END
	
	UPDATE Software
	SET [Code] = ISNULL(@code, [Code]),
	[Name] = ISNULL(@name, [Name]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('SOFTWARE', @state), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE [PK_Software_Id] = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetSoftware @requester_id, @id, '*';
END
GO
/****** Object:  View [dbo].[vw_Courses]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Courses] AS
SELECT C.PK_Course_Id AS Id, C.Code, C.Name, C.Created_At, C.Updated_At, S.Name AS State
FROM Courses AS C
INNER JOIN States AS S ON C.FK_State_Id = S.PK_State_Id;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUserData]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateUserData]
(
	@requester_id   INT,            -- The identity of the requester user.
	@id				INT,			-- The user id.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20),	-- The phone. [Nullable]
	@state			VARCHAR(30)		-- The state. ('active', 'disabled', 'blocked')
)
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS (SELECT 1 FROM Users WHERE Username = @username AND PK_User_Id <> @id)
	BEGIN
		RAISERROR('Ya existe un usuario con identificado como %s.', 15, 1, @username);
		RETURN -1;
	END

	UPDATE Users
	SET[Name] = ISNULL(@name, [Name]),
	[Last_Name_1] = ISNULL(@last_name_1, [Last_Name_1]),
	[Last_Name_2] =  CAST(dbo.fn_IsNull(@last_name_2, [Last_Name_2]) AS VARCHAR(70)),
	[Username] = ISNULL(@username, [Username]),
	[Password] = ISNULL(@password, [Password]),
	[Email] = CAST(dbo.fn_IsNull(@email, [Email]) AS VARCHAR(100)),
	[Phone] =  CAST(dbo.fn_IsNull(@phone, [Phone]) AS VARCHAR(20)),
	[Gender] = ISNULL(@gender, [Gender]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('USER', @state), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE PK_User_Id = @id;
END
GO
/****** Object:  Table [dbo].[Students]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[FK_User_Id] [int] NOT NULL,
 CONSTRAINT [IX_Students_User_Id] UNIQUE NONCLUSTERED 
(
	[FK_User_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_RemoveSoftwareFromLaboratory]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_RemoveSoftwareFromLaboratory]
(
	@requester_id INT,						-- The identity of the requester user.
	@laboratory INT,						-- The laboratory id.
	@softwares AS SoftwareList READONLY		-- The list of software codes.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DELETE SoftwareByLaboratory
	WHERE FK_Laboratory_Id = @laboratory
	AND FK_Software_Id IN (SELECT S2.PK_Software_Id FROM @softwares AS S1 INNER JOIN Software AS S2 ON S1.Code = S2.Code);
	
	UPDATE Laboratories
	SET Updated_At = GETDATE()
	WHERE PK_Laboratory_Id = @laboratory;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteStudent]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteStudent]
(
	@requester_id	INT,	-- The identity of the requester user.
	@user_id		INT		-- The User Id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	IF NOT EXISTS (SELECT 1 FROM Students WHERE FK_User_Id = @user_id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Estudiante no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Users
	SET FK_State_Id = dbo.fn_GetStateID('USER', 'Inactivo'),
	Updated_At = GETDATE()
	WHERE PK_User_Id = @user_id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  View [dbo].[vw_Administrators]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Administrators]
AS
SELECT     U.PK_User_Id AS id, U.Name, U.Last_Name_1, U.Last_Name_2, RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS full_name, U.Email, 
                      U.Gender, U.Phone, U.Username, A.Created_At, A.Updated_At, S.Name AS state
FROM         dbo.Administrators AS A INNER JOIN
                      dbo.Users AS U ON A.FK_User_Id = U.PK_User_Id INNER JOIN
                      dbo.States AS S ON A.FK_State_Id = S.PK_State_Id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "U"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "S"
            Begin Extent = 
               Top = 68
               Left = 512
               Bottom = 161
               Right = 663
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Administrators'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Administrators'
GO
/****** Object:  View [dbo].[vw_Students]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Students]
AS
SELECT     U.PK_User_Id AS id, U.Name, U.Last_Name_1, U.Last_Name_2, RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS full_name, U.Email, 
                      U.Gender, U.Phone, U.Username, U.Created_At, U.Updated_At, E.Name AS state
FROM         dbo.Students AS S INNER JOIN
                      dbo.Users AS U ON S.FK_User_Id = U.PK_User_Id INNER JOIN
                      dbo.States AS E ON U.FK_State_Id = E.PK_State_Id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "S"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 69
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "U"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "E"
            Begin Extent = 
               Top = 72
               Left = 38
               Bottom = 165
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Students'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Students'
GO
/****** Object:  View [dbo].[vw_Professors]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Professors]
AS
SELECT     U.PK_User_Id AS id, U.Name, U.Last_Name_1, U.Last_Name_2, RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS full_name, U.Email, 
                      U.Gender, U.Phone, U.Username, U.Created_At, U.Updated_At, S.Name AS state
FROM         dbo.Professors AS P INNER JOIN
                      dbo.Users AS U ON P.FK_User_Id = U.PK_User_Id INNER JOIN
                      dbo.States AS S ON U.FK_State_Id = S.PK_State_Id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "P"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 69
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "U"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "S"
            Begin Extent = 
               Top = 72
               Left = 38
               Bottom = 165
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Professors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Professors'
GO
/****** Object:  Table [dbo].[Operators]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operators](
	[PK_Operator_Id] [int] IDENTITY(1,1) NOT NULL,
	[FK_User_Id] [int] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Period_Year] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	[FK_Period_Id] [int] NOT NULL,
 CONSTRAINT [PK_Operators] PRIMARY KEY CLUSTERED 
(
	[PK_Operator_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Operators] UNIQUE NONCLUSTERED 
(
	[FK_User_Id] ASC,
	[FK_Period_Id] ASC,
	[Period_Year] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 10/14/2015 08:20:27 ******/
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
	[FK_State_Id] [int] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[PK_Group_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Groups] UNIQUE NONCLUSTERED 
(
	[Number] ASC,
	[FK_Professor_Id] ASC,
	[FK_Course_Id] ASC,
	[FK_Period_Id] ASC,
	[Period_Year] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetGroupsByStudent]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetGroupsByStudent]
(
	@requester_id INT,			-- The identity of the requester user.
	@student	VARCHAR(70),	-- The student username.
	@fields		VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by	VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where		VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	DECLARE @studentId INT, @where_statement VARCHAR(MAX);
	
	-- Get the student identity.
	SELECT @studentId = FK_User_Id FROM Students AS S INNER JOIN Users AS U ON S.FK_User_Id = U.PK_User_Id WHERE U.Username = @student;
	
	IF @studentId IS NOT NULL
	BEGIN
		SET @where = COALESCE(@where, '');
		SET @where_statement = 'id IN (SELECT FK_Group_Id FROM StudentsByGroup WHERE FK_Student_Id = ' + CAST(@studentId AS VARCHAR) + ')';
		IF (@where <> '') SET @where_statement = @where_statement + ' AND ' + @where;
		
		EXEC dbo.sp_GetAll 'vw_Groups', @fields, @order_by, @where_statement, NULL, -1;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteProfessor]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteProfessor]
(
	@requester_id	INT,	-- The identity of the requester user.
	@user_id		INT		-- The User Id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	IF NOT EXISTS (SELECT 1 FROM Professors WHERE FK_User_Id = @user_id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Docente no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Users
	SET FK_State_Id = dbo.fn_GetStateID('USER', 'Inactivo'),
	Updated_At = GETDATE()
	WHERE PK_User_Id = @user_id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateStudent]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateStudent]
(
	@requester_id	INT,			-- The identity of the requester user.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20)		-- The phone. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if user exists.
	IF EXISTS (SELECT 1 FROM Users WHERE Username = @username)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un usuario con el username [%s].', 15, 1, @username);
		RETURN -1;
	END
	
	DECLARE @id INT;
	EXEC dbo.sp_CreateUser @id OUTPUT, @name, @last_name_1, @last_name_2, @gender, @username, @password, @email, @phone;
	INSERT INTO Students (FK_User_Id) VALUES(@id);
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetStudentByUsername @requester_id, @username, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddSoftwareToLaboratory]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AddSoftwareToLaboratory]
(
	@requester_id INT,						-- The identity of the requester user.
	@laboratory INT,						-- The laboratory id.
	@softwares AS SoftwareList READONLY		-- The list of software codes.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	INSERT INTO SoftwareByLaboratory(FK_Laboratory_Id, FK_Software_Id)
	SELECT @laboratory, S2.PK_Software_Id
	FROM @softwares AS S1
	INNER JOIN Software AS S2 ON S1.Code = S2.Code
	WHERE NOT EXISTS (SELECT 1 FROM SoftwareByLaboratory WHERE FK_Laboratory_Id = @laboratory AND FK_Software_Id = S2.PK_Software_Id);
	
	UPDATE Laboratories
	SET Updated_At = GETDATE()
	WHERE PK_Laboratory_Id = @laboratory;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateAdministrator]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateAdministrator]
(
	@requester_id INT,			-- The identity of the requester user.
	@user_id INT				-- The user ID.
)
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Administrator already exists, only has to be activated.
	IF EXISTS (SELECT 1 FROM Administrators WHERE FK_User_Id = @user_id)
	BEGIN
		UPDATE Administrators
		SET FK_State_Id = dbo.fn_GetStateID('ADMINISTRATOR', 'Activo'),
			Updated_At = GETDATE()
		WHERE FK_User_Id = @user_id
	END
	-- Administrator doesn't exist yet, has to be inserted.
	ELSE
	BEGIN
		INSERT INTO Administrators (FK_User_Id, FK_State_Id)
		SELECT @user_id, dbo.fn_GetStateID('ADMINISTRATOR', 'Activo');
	END
	
	EXEC dbo.sp_GetAdministrator @requester_id, @user_id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateProfessor]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateProfessor]
(
	@requester_id	INT,			-- The identity of the requester user.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20)		-- The phone. [Nullable]
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if user exists.
	IF EXISTS (SELECT 1 FROM Users WHERE Username = @username)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un usuario con el username [%s].', 15, 1, @username);
		RETURN -1;
	END
	
	DECLARE @id INT;
	EXEC dbo.sp_CreateUser @id OUTPUT, @name, @last_name_1, @last_name_2, @gender, @username, @password, @email, @phone;
	INSERT INTO Professors (FK_User_Id) VALUES(@id);
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetProfessorByUsername @requester_id, @username, '*';
END
GO
/****** Object:  Table [dbo].[Reservations]    Script Date: 10/14/2015 08:20:27 ******/
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
CREATE NONCLUSTERED INDEX [IX_Reservations_EndTime] ON [dbo].[Reservations] 
(
	[End_Time] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Reservations_StartTime] ON [dbo].[Reservations] 
(
	[Start_Time] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateOperator]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateOperator]
(
	@requester_id   INT,		 -- The identity of the requester user.
	@user_id		INT,		 -- The user ID.
	@period_value	INT,		 -- The period value. (1, 2, ...)
	@period_type	VARCHAR(50), -- The period type ('Semestre', 'Bimestre', ...)
	@period_year	INT			 -- The year. (2014, 2015, ...)
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DECLARE @period_id INT, @operator_id INT;
	
	SELECT @period_id = P.PK_Period_Id FROM Periods AS P 
	INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id 
	WHERE P.Value = @period_value AND PT.Name = @period_type;
	
	-- Check if student exists.
	IF NOT EXISTS (SELECT 1 FROM Students WHERE FK_User_Id = @user_id)
	BEGIN
		RAISERROR('Estudiante no encontrado.', 15, 1);
		ROLLBACK TRANSACTION T;
		RETURN -1;
	END

	-- Check for valid period.
	IF @period_id IS NULL
	BEGIN
		RAISERROR('Periodo inválido.', 15, 1);
		ROLLBACK TRANSACTION T;
		RETURN -1;
	END
	
	SELECT @operator_id = PK_Operator_Id FROM Operators
	WHERE FK_User_Id = @user_id AND FK_Period_Id = @period_id AND Period_Year = @period_year;
	
	-- Check if operator exists.
	IF @operator_id IS NOT NULL
	BEGIN
		-- Operator already exists, only has to be activated.
		UPDATE Operators
		SET FK_State_Id = dbo.fn_GetStateID('OPERATOR', 'Activo'),
			Updated_At = GETDATE()
		WHERE FK_User_Id = @user_id AND FK_Period_Id = @period_id AND Period_Year = @period_year
	END
	ELSE
	BEGIN
		-- Operator doesn't exist yet, has to be inserted.
		INSERT INTO Operators (FK_User_Id, FK_Period_Id, Period_Year, FK_State_Id)
		SELECT @user_id, @period_id, @period_year, dbo.fn_GetStateID('OPERATOR', 'Activo');
		SET @operator_id = SCOPE_IDENTITY();
	END
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetOperator @requester_id, @operator_id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateLaboratory]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateLaboratory]
(
	@requester_id	INT,						-- The identity of the requester user.
	@name			VARCHAR(500),				-- The laboratory name.
	@seats			INT,						-- The available seats.
	@appointment_priority INT,					-- The appointment priority.
	@reservation_priority INT,					-- The reservation_priority.
	@software		AS SoftwareList READONLY	-- The list of software codes.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DECLARE @id INT;
	INSERT INTO Laboratories (Name, Seats, Appointment_Priority, Reservation_Priority, FK_State_Id) VALUES
	(@name, @seats, @appointment_priority, @reservation_priority, dbo.fn_GetStateID('LABORATORY', 'Activo'));
	SET @id = SCOPE_IDENTITY();
	
	IF EXISTS (SELECT 1 FROM @software)
	BEGIN
		EXEC dbo.sp_AddSoftwareToLaboratory @requester_id, @id, @software;
	END
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetLaboratory @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteGroup]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteGroup]
(
	@requester_id	INT,	-- The identity of the requester user.
	@id				INT		-- The group id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if group exists.
	IF NOT EXISTS (SELECT 1 FROM Groups WHERE PK_Group_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Grupo no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Groups
	SET FK_State_Id = dbo.fn_GetStateID('GROUP', 'Inactivo'),
	Updated_At = GETDATE()
	WHERE PK_Group_Id = @id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteOperator]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteOperator]
(
	@requester_id	INT,	-- The identity of the requester user.
	@operator_id	INT		-- The operator ID.
)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Operators
	SET FK_State_Id = dbo.fn_GetStateID('OPERATOR', 'Inactivo'),
		Updated_At = GETDATE()
	WHERE
		PK_Operator_Id = @operator_id;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetUserType]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetUserType](@UserId INT)
RETURNS VARCHAR(30)
AS
BEGIN
	IF EXISTS 
	(
		SELECT 1 FROM Administrators AS A
		INNER JOIN States AS S ON S.PK_State_Id = A.FK_State_Id
		WHERE A.FK_User_Id = @UserId AND S.Name = 'Activo'
	)
    RETURN 'Administrador'

    IF EXISTS
	(
		SELECT 1 FROM Operators AS O
		INNER JOIN States AS S ON S.PK_State_Id = O.FK_State_Id
		INNER JOIN Periods AS P ON P.PK_Period_Id = O.FK_Period_Id
		INNER JOIN PeriodTypes AS PT ON PT.PK_Period_Type_Id = P.FK_Period_Type_Id
		WHERE O.FK_User_Id = @UserId AND S.Name = 'Activo' AND dbo.fn_IsDateInPeriod(GETDATE(), P.Value, PT.Name, O.Period_Year) = 1
	)
	RETURN 'Operador'
    
    IF EXISTS (SELECT 1 FROM Students WHERE FK_User_Id = @UserId)
    RETURN 'Estudiante'
    
    IF EXISTS (SELECT 1 FROM Professors WHERE FK_User_Id = @UserId)
    RETURN 'Docente'
    
    RETURN NULL
END
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[PK_Appointment_Id] [int] IDENTITY(1,1) NOT NULL,
	[Attendance] [bit] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Created_At] [datetime] NOT NULL,
	[Updated_At] [datetime] NOT NULL,
	[FK_Student_Id] [int] NOT NULL,
	[FK_Laboratory_Id] [int] NOT NULL,
	[FK_Software_Id] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	[FK_Group_Id] [int] NOT NULL,
 CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
(
	[PK_Appointment_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Appointments_Date] ON [dbo].[Appointments] 
(
	[Date] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Operators]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Operators]
AS
SELECT     O.PK_Operator_Id AS id, U.Name, U.Last_Name_1, U.Last_Name_2, RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS full_name, U.Email, 
                      U.Gender, U.Phone, U.Username, O.Created_At, O.Updated_At, P.Value AS [period.value], PT.Name AS [period.type], O.Period_Year AS [period.year], 
                      S.Name AS state
FROM         dbo.Operators AS O INNER JOIN
                      dbo.Users AS U ON O.FK_User_Id = U.PK_User_Id INNER JOIN
                      dbo.Periods AS P ON O.FK_Period_Id = P.PK_Period_Id INNER JOIN
                      dbo.PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id INNER JOIN
                      dbo.States AS S ON O.FK_State_Id = S.PK_State_Id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "O"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 196
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "U"
            Begin Extent = 
               Top = 6
               Left = 234
               Bottom = 114
               Right = 385
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "P"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 207
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PT"
            Begin Extent = 
               Top = 114
               Left = 250
               Bottom = 207
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "S"
            Begin Extent = 
               Top = 210
               Left = 38
               Bottom = 303
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Operators'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Operators'
GO
/****** Object:  View [dbo].[vw_Groups]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Groups]
AS
SELECT     [Group].id, [Group].number, [Group].[period.value], [Group].[period.type], [Group].[period.year], [Group].state, [Group].created_at, [Group].updated_at, Course.[course.id], 
                      Course.[course.code], Course.[course.name], Course.[course.state], Course.[course.created_at], Course.[course.updated_at], Professor.[professor.id], 
                      Professor.[professor.name], Professor.[professor.last_name_1], Professor.[professor.last_name_2], Professor.[professor.full_name], Professor.[professor.email], 
                      Professor.[professor.gender], Professor.[professor.phone], Professor.[professor.username], Professor.[professor.state], Professor.[professor.created_at], 
                      Professor.[professor.updated_at]
FROM         (SELECT     G.PK_Group_Id AS id, G.Number AS number, G.FK_Course_Id AS course, G.FK_Professor_Id AS professor, P.Value AS [period.value], 
                                              PT.Name AS [period.type], G.Period_Year AS [period.year], S.Name AS state, G.Created_At AS created_at, G.Updated_At AS updated_at
                       FROM          dbo.Groups AS G INNER JOIN
                                              dbo.States AS S ON G.FK_State_Id = S.PK_State_Id INNER JOIN
                                              dbo.Periods AS P ON G.FK_Period_Id = P.PK_Period_Id INNER JOIN
                                              dbo.PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id) AS [Group] INNER JOIN
                          (SELECT     C.PK_Course_Id AS [course.id], C.Code AS [course.code], C.Name AS [course.name], S.Name AS [course.state], C.Created_At AS [course.created_at], 
                                                   C.Updated_At AS [course.updated_at]
                            FROM          dbo.Courses AS C INNER JOIN
                                                   dbo.States AS S ON C.FK_State_Id = S.PK_State_Id) AS Course ON [Group].course = Course.[course.id] INNER JOIN
                          (SELECT     U.PK_User_Id AS [professor.id], U.Name AS [professor.name], U.Last_Name_1 AS [professor.last_name_1], U.Last_Name_2 AS [professor.last_name_2], 
                                                   RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [professor.full_name], U.Email AS [professor.email], 
                                                   U.Gender AS [professor.gender], U.Phone AS [professor.phone], U.Username AS [professor.username], S.Name AS [professor.state], 
                                                   U.Created_At AS [professor.created_at], U.Updated_At AS [professor.updated_at]
                            FROM          dbo.Professors AS P INNER JOIN
                                                   dbo.Users AS U ON P.FK_User_Id = U.PK_User_Id INNER JOIN
                                                   dbo.States AS S ON U.FK_State_Id = S.PK_State_Id) AS Professor ON [Group].professor = Professor.[professor.id]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Group"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Course"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 399
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Professor"
            Begin Extent = 
               Top = 6
               Left = 437
               Bottom = 114
               Right = 628
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Groups'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Groups'
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateStudent]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateStudent]
(
	@requester_id	INT,			-- The identity of the requester user.
	@id				INT,			-- The user id.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20),	-- The phone. [Nullable]
	@state			VARCHAR(30)		-- The state. ('active', 'disabled', 'blocked')
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if student exists.
	IF NOT EXISTS (SELECT 1 FROM vw_Students WHERE Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Estudiante no encontrado.', 15, 1);
		RETURN -1;
	END
	ELSE
	BEGIN
		EXEC dbo.sp_UpdateUserData @requester_id, @id, @name, @last_name_1, @last_name_2, @gender, @username, @password, @email, @phone, @state;
		COMMIT TRANSACTION T;
	END
	
	EXEC dbo.sp_GetStudent @requester_id, @id, '*';
END
GO
/****** Object:  Table [dbo].[StudentsByGroup]    Script Date: 10/14/2015 08:20:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentsByGroup](
	[FK_Student_Id] [int] NOT NULL,
	[FK_Group_Id] [int] NOT NULL,
 CONSTRAINT [IX_StudentsByGroup] UNIQUE NONCLUSTERED 
(
	[FK_Student_Id] ASC,
	[FK_Group_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateProfessor]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateProfessor]
(
	@requester_id	INT,			-- The identity of the requester user.
	@id				INT,			-- The user id.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20),	-- The phone. [Nullable]
	@state			VARCHAR(30)		-- The state. ('active', 'disabled', 'blocked')
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if professor exists.
	IF NOT EXISTS (SELECT 1 FROM vw_Professors WHERE Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Docente no encontrado.', 15, 1);
		RETURN -1;
	END
	
	EXEC dbo.sp_UpdateUserData @requester_id, @id, @name, @last_name_1, @last_name_2, @gender, @username, @password, @email, @phone, @state;
	COMMIT TRANSACTION T;
	
	EXEC dbo.sp_GetProfessor @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateLaboratorySoftware]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateLaboratorySoftware]
(
	@requester_id INT,			-- The identity of the requester user.
	@laboratory INT,						-- The laboratory id.
	@softwares AS SoftwareList READONLY		-- The list of software codes.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DELETE SoftwareByLaboratory
	WHERE FK_Laboratory_Id = @laboratory;
	
	EXEC dbo.sp_AddSoftwareToLaboratory @requester_id, @laboratory, @softwares;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteAdministrator]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteAdministrator]
(
	@requester_id INT,		-- The identity of the requester user.
	@user_id INT			-- The User Id.
)
AS
BEGIN
	BEGIN TRANSACTION T;
	DECLARE @count INT, @state VARCHAR(30);
	SELECT @count = COUNT(*) FROM vw_Administrators WHERE [state] = 'Activo';
	SELECT @state = [state] FROM vw_Administrators WHERE [id] = @user_id;
	
	IF (@count = 1 AND @state = 'Activo')
	BEGIN
		RAISERROR('No se puede eliminar este administrador.', 15, 1);
		ROLLBACK TRANSACTION T;
		RETURN -1;
	END
	
	UPDATE Administrators
	SET FK_State_Id = dbo.fn_GetStateID('ADMINISTRATOR', 'Inactivo'),
		Updated_At = GETDATE()
	WHERE FK_User_Id = @user_id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_RemoveStudentsFromGroup]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_RemoveStudentsFromGroup]
(
	@requester_id INT,				-- The identity of the requester user.
	@group INT,						-- The group id.
	@students AS UserList READONLY	-- The list of students usernames.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DELETE StudentsByGroup
	WHERE FK_Group_Id = @group
	AND FK_Student_Id IN (SELECT U.PK_User_Id FROM @students AS S INNER JOIN Users AS U ON S.Username = U.Username);
	
	UPDATE Groups
	SET Updated_At = GETDATE()
	WHERE PK_Group_Id = @group;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateLaboratory]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateLaboratory]
(
	@requester_id INT,			-- The identity of the requester user.
	@id		INT,						-- The laboratory id.
	@name	VARCHAR(500),				-- The laboratory name.
	@seats	INT,						-- The available seats.
	@appointment_priority INT,					-- The appointment priority.
	@reservation_priority INT,					-- The reservation_priority.
	@state	VARCHAR(30),				-- The laboratory state.
	@software AS SoftwareList READONLY	-- The list of software codes.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if laboratory exists.
	IF NOT EXISTS (SELECT 1 FROM Laboratories WHERE PK_Laboratory_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Laboratorio no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Laboratories
	SET [Seats] = ISNULL(@seats, [Seats]),
	[Name] = ISNULL(@name, [Name]),
	[Appointment_Priority] = ISNULL(@appointment_priority, [Appointment_Priority]),
	[Reservation_Priority] = ISNULL(@reservation_priority, [Reservation_Priority]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('LABORATORY', @state), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE [PK_Laboratory_Id] = @id;
	
	IF EXISTS (SELECT 1 FROM @software)
	BEGIN
		EXEC dbo.sp_UpdateLaboratorySoftware @requester_id, @id, @software;
	END
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetLaboratory @requester_id, @id, '*';
END
GO
/****** Object:  View [dbo].[vw_Users]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Users]
AS
SELECT     U.PK_User_Id AS id, U.Name, U.Last_Name_1, U.Last_Name_2, RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS Full_Name, 
                      U.Username, U.Email, U.Gender, U.Phone, U.Created_At, U.Updated_At, dbo.fn_GetUserType(U.PK_User_Id) AS Type, S.Name AS State
FROM         dbo.Users AS U INNER JOIN
                      dbo.States AS S ON U.FK_State_Id = S.PK_State_Id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "U"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "S"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 99
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Users'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Users'
GO
/****** Object:  View [dbo].[vw_Reservations]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Reservations]
AS
SELECT     Reservation.id, Reservation.start_time, Reservation.end_time, Reservation.created_at, Reservation.updated_at, Reservation.state, Professor.[professor.id], 
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
                      [Group].[group.professor.created_at], [Group].[group.professor.updated_at]
FROM         (SELECT     R.PK_Reservation_Id AS id, R.Start_Time AS start_time, R.End_Time AS end_time, R.Created_At AS created_at, R.Updated_At AS updated_at, 
                                              S.Name AS state, R.FK_Group_Id AS [group], R.FK_Laboratory_Id AS laboratory, R.FK_Professor_Id AS professor, R.FK_Software_Id AS software
                       FROM          dbo.Reservations AS R INNER JOIN
                                              dbo.States AS S ON R.FK_State_Id = S.PK_State_Id) AS Reservation INNER JOIN
                          (SELECT     U.PK_User_Id AS [professor.id], U.Name AS [professor.name], U.Last_Name_1 AS [professor.last_name_1], U.Last_Name_2 AS [professor.last_name_2], 
                                                   U.Email AS [professor.email], RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [professor.full_name], 
                                                   U.Gender AS [professor.gender], U.Phone AS [professor.phone], U.Username AS [professor.username], U.Created_At AS [professor.created_at], 
                                                   U.Updated_At AS [professor.updated_at], S.Name AS [professor.state]
                            FROM          dbo.Professors AS P INNER JOIN
                                                   dbo.Users AS U ON P.FK_User_Id = U.PK_User_Id INNER JOIN
                                                   dbo.States AS S ON U.FK_State_Id = S.PK_State_Id) AS Professor ON Reservation.professor = Professor.[professor.id] INNER JOIN
                          (SELECT     L.PK_Laboratory_Id AS [laboratory.id], L.Name AS [laboratory.name], L.Seats AS [laboratory.seats], S.Name AS [laboratory.state], 
                                                   L.Created_At AS [laboratory.created_at], L.Updated_At AS [laboratory.updated_at], L.Appointment_Priority AS [laboratory.appointment_priority], 
                                                   L.Reservation_Priority AS [laboratory.reservation_priority]
                            FROM          dbo.Laboratories AS L INNER JOIN
                                                   dbo.States AS S ON L.FK_State_Id = S.PK_State_Id) AS Laboratory ON Reservation.laboratory = Laboratory.[laboratory.id] LEFT OUTER JOIN
                          (SELECT     S.PK_Software_Id AS [software.id], S.Code AS [software.code], S.Name AS [software.name], E.Name AS [software.state], 
                                                   S.Created_At AS [software.created_at], S.Updated_At AS [software.updated_at]
                            FROM          dbo.Software AS S INNER JOIN
                                                   dbo.States AS E ON S.FK_State_Id = E.PK_State_Id) AS Software ON Reservation.software = Software.[software.id] LEFT OUTER JOIN
                          (SELECT     [Group].id AS [group.id], [Group].number AS [group.number], [Group].[period.value] AS [group.period.value], [Group].[period.type] AS [group.period.type], 
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
                            FROM          (SELECT     G.PK_Group_Id AS id, G.Number AS number, G.FK_Course_Id AS course, G.FK_Professor_Id AS professor, P.Value AS [period.value], 
                                                                           PT.Name AS [period.type], G.Period_Year AS [period.year], S.Name AS state, G.Created_At AS created_at, G.Updated_At AS updated_at
                                                    FROM          dbo.Groups AS G INNER JOIN
                                                                           dbo.States AS S ON G.FK_State_Id = S.PK_State_Id INNER JOIN
                                                                           dbo.Periods AS P ON G.FK_Period_Id = P.PK_Period_Id INNER JOIN
                                                                           dbo.PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id) AS [Group] INNER JOIN
                                                       (SELECT     C.PK_Course_Id AS [course.id], C.Code AS [course.code], C.Name AS [course.name], S.Name AS [course.state], 
                                                                                C.Created_At AS [course.created_at], C.Updated_At AS [course.updated_at]
                                                         FROM          dbo.Courses AS C INNER JOIN
                                                                                dbo.States AS S ON C.FK_State_Id = S.PK_State_Id) AS Course ON [Group].course = Course.[course.id] INNER JOIN
                                                       (SELECT     U.PK_User_Id AS [professor.id], U.Name AS [professor.name], U.Last_Name_1 AS [professor.last_name_1], 
                                                                                U.Last_Name_2 AS [professor.last_name_2], RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) 
                                                                                AS [professor.full_name], U.Email AS [professor.email], U.Gender AS [professor.gender], U.Phone AS [professor.phone], 
                                                                                U.Username AS [professor.username], S.Name AS [professor.state], U.Created_At AS [professor.created_at], 
                                                                                U.Updated_At AS [professor.updated_at]
                                                         FROM          dbo.Professors AS P INNER JOIN
                                                                                dbo.Users AS U ON P.FK_User_Id = U.PK_User_Id INNER JOIN
                                                                                dbo.States AS S ON U.FK_State_Id = S.PK_State_Id) AS Professor ON [Group].professor = Professor.[professor.id]) AS [Group] ON 
                      Reservation.[group] = [Group].[group.id]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[35] 4[4] 2[42] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -324
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Reservation"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Professor"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 418
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "Laboratory"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "Software"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Group"
            Begin Extent = 
               Top = 330
               Left = 38
               Bottom = 438
               Right = 261
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Reservations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Reservations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Reservations'
GO
/****** Object:  View [dbo].[vw_Appointments]    Script Date: 10/14/2015 08:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Appointments]
AS
SELECT     TOP (100) PERCENT Appointment.id, Appointment.date, Appointment.created_at, Appointment.updated_at, Appointment.state, Student.[student.id], 
                      Student.[student.name], Student.[student.last_name_1], Student.[student.last_name_2], Student.[student.email], Student.[student.full_name], Student.[student.gender], 
                      Student.[student.phone], Student.[student.username], Student.[student.created_at], Student.[student.updated_at], Student.[student.state], Laboratory.[laboratory.id], 
                      Laboratory.[laboratory.name], Laboratory.[laboratory.seats], Laboratory.[laboratory.state], Laboratory.[laboratory.created_at], Laboratory.[laboratory.updated_at], 
                      Laboratory.[laboratory.appointment_priority], Laboratory.[laboratory.reservation_priority], Software.[software.id], Software.[software.code], Software.[software.name], 
                      Software.[software.state], Software.[software.created_at], Software.[software.updated_at], [Group].[group.id], [Group].[group.number], [Group].[group.period.value], 
                      [Group].[group.period.type], [Group].[group.period.year], [Group].[group.state], [Group].[group.created_at], [Group].[group.updated_at], Course.[group.course.id], 
                      Course.[group.course.code], Course.[group.course.name], Course.[group.course.state], Course.[group.course.created_at], Course.[group.course.updated_at], 
                      Professor.[group.professor.id], Professor.[group.professor.name], Professor.[group.professor.last_name_1], Professor.[group.professor.last_name_2], 
                      Professor.[group.professor.email], Professor.[group.professor.full_name], Professor.[group.professor.gender], Professor.[group.professor.phone], 
                      Professor.[group.professor.username], Professor.[group.professor.state], Professor.[group.professor.created_at], Professor.[group.professor.updated_at], 
                      Appointment.attendance
FROM         (SELECT     TOP (100) PERCENT A.PK_Appointment_Id AS id, A.Date AS date, A.Attendance AS attendance, A.Created_At AS created_at, A.Updated_At AS updated_at, 
                                              S.Name AS state, A.FK_Laboratory_Id AS laboratory, A.FK_Software_Id AS software, A.FK_Student_Id AS student, A.FK_Group_Id AS [group]
                       FROM          dbo.Appointments AS A INNER JOIN
                                              dbo.States AS S ON A.FK_State_Id = S.PK_State_Id) AS Appointment INNER JOIN
                          (SELECT     U.PK_User_Id AS [student.id], U.Name AS [student.name], U.Last_Name_1 AS [student.last_name_1], U.Last_Name_2 AS [student.last_name_2], 
                                                   U.Email AS [student.email], RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [student.full_name], 
                                                   U.Gender AS [student.gender], U.Phone AS [student.phone], U.Username AS [student.username], U.Created_At AS [student.created_at], 
                                                   U.Updated_At AS [student.updated_at], E.Name AS [student.state]
                            FROM          dbo.Students AS S INNER JOIN
                                                   dbo.Users AS U ON S.FK_User_Id = U.PK_User_Id INNER JOIN
                                                   dbo.States AS E ON U.FK_State_Id = E.PK_State_Id) AS Student ON Appointment.student = Student.[student.id] INNER JOIN
                          (SELECT     L.PK_Laboratory_Id AS [laboratory.id], L.Name AS [laboratory.name], L.Seats AS [laboratory.seats], S.Name AS [laboratory.state], 
                                                   L.Created_At AS [laboratory.created_at], L.Updated_At AS [laboratory.updated_at], L.Appointment_Priority AS [laboratory.appointment_priority], 
                                                   L.Reservation_Priority AS [laboratory.reservation_priority]
                            FROM          dbo.Laboratories AS L INNER JOIN
                                                   dbo.States AS S ON L.FK_State_Id = S.PK_State_Id) AS Laboratory ON Appointment.laboratory = Laboratory.[laboratory.id] INNER JOIN
                          (SELECT     S.PK_Software_Id AS [software.id], S.Code AS [software.code], S.Name AS [software.name], E.Name AS [software.state], 
                                                   S.Created_At AS [software.created_at], S.Updated_At AS [software.updated_at]
                            FROM          dbo.Software AS S INNER JOIN
                                                   dbo.States AS E ON S.FK_State_Id = E.PK_State_Id) AS Software ON Appointment.software = Software.[software.id] INNER JOIN
                          (SELECT     G.PK_Group_Id AS [group.id], G.Number AS [group.number], G.FK_Course_Id AS [group.course], G.FK_Professor_Id AS [group.professor], 
                                                   P.Value AS [group.period.value], PT.Name AS [group.period.type], G.Period_Year AS [group.period.year], S.Name AS [group.state], 
                                                   G.Created_At AS [group.created_at], G.Updated_At AS [group.updated_at]
                            FROM          dbo.Groups AS G INNER JOIN
                                                   dbo.States AS S ON G.FK_State_Id = S.PK_State_Id INNER JOIN
                                                   dbo.Periods AS P ON G.FK_Period_Id = P.PK_Period_Id INNER JOIN
                                                   dbo.PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id) AS [Group] ON Appointment.[group] = [Group].[group.id] INNER JOIN
                          (SELECT     C.PK_Course_Id AS [group.course.id], C.Code AS [group.course.code], C.Name AS [group.course.name], S.Name AS [group.course.state], 
                                                   C.Created_At AS [group.course.created_at], C.Updated_At AS [group.course.updated_at]
                            FROM          dbo.Courses AS C INNER JOIN
                                                   dbo.States AS S ON C.FK_State_Id = S.PK_State_Id) AS Course ON [Group].[group.course] = Course.[group.course.id] INNER JOIN
                          (SELECT     U.PK_User_Id AS [group.professor.id], U.Name AS [group.professor.name], U.Last_Name_1 AS [group.professor.last_name_1], 
                                                   U.Last_Name_2 AS [group.professor.last_name_2], U.Email AS [group.professor.email], 
                                                   RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS [group.professor.full_name], U.Gender AS [group.professor.gender], 
                                                   U.Phone AS [group.professor.phone], U.Username AS [group.professor.username], S.Name AS [group.professor.state], 
                                                   U.Created_At AS [group.professor.created_at], U.Updated_At AS [group.professor.updated_at]
                            FROM          dbo.Professors AS P INNER JOIN
                                                   dbo.Users AS U ON P.FK_User_Id = U.PK_User_Id INNER JOIN
                                                   dbo.States AS S ON U.FK_State_Id = S.PK_State_Id) AS Professor ON [Group].[group.professor] = Professor.[group.professor.id]
ORDER BY Appointment.date
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -192
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Appointment"
            Begin Extent = 
               Top = 102
               Left = 447
               Bottom = 210
               Right = 598
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Student"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 114
               Right = 409
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "Laboratory"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "Software"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Group"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Course"
            Begin Extent = 
               Top = 330
               Left = 38
               Bottom = 438
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Professor"
            Begin Extent = 
               Top = 438
               Left = 38
               Bottom = 546
               Right = 261
            End
            DisplayFlag' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Appointments'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N's = 280
            TopColumn = 8
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Appointments'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Appointments'
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetLaboratoryAvailableSeats]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* Get the amount of seats available in a laboratory on a certain hour. */
CREATE FUNCTION [dbo].[fn_GetLaboratoryAvailableSeats]
(
	@id INT,		-- The laboratory identity.
	@date DATETIME  -- The datetime.
)
RETURNS INT
AS
BEGIN
	DECLARE @appointments INT, @seats INT;
	
	SELECT @seats = Seats FROM Laboratories WHERE PK_Laboratory_Id = @id;
	
	SELECT @appointments = COUNT(1) FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
	WHERE A.FK_Laboratory_Id = @id AND A.Date = @date AND S.Name <> 'Cancelada';
	
	RETURN @seats - @appointments;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetStudentWeeklyAppointmentsCount]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* Get the amount of appointments an user has made in a week. */
CREATE FUNCTION [dbo].[fn_GetStudentWeeklyAppointmentsCount]
(
	@id INT,		-- The student identity.
	@date DATETIME  -- The appointment datetime.
)
RETURNS INT
AS
BEGIN
	DECLARE @count INT;
	
	SELECT @count = COUNT(1) FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
	WHERE A.FK_Student_Id = @id AND S.Name <> 'Cancelada' AND
	DATEDIFF(week, A.Date, @date) = 0 AND DATEDIFF(year, A.Date, @date) = 0
	
	RETURN @count
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_IsLaboratoryReserved]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_IsLaboratoryReserved]
(
	@id INT,				-- The laboratory identity.
	@start_date DATETIME,	-- The start datetime.
	@end_date DATETIME		-- The end datetime.
)
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT = 0;
	
	IF EXISTS
	(
		SELECT 1 FROM Reservations AS R
		INNER JOIN States AS S ON R.FK_State_Id = S.PK_State_Id
		WHERE R.FK_Laboratory_Id = @id AND S.Name <> 'Cancelada' AND
			  (
				(@start_date >= R.Start_Time AND @start_date < R.End_Time) OR
				(@end_date > R.Start_Time AND @end_date < R.End_Time)
			  )
	)
	BEGIN
		SET @result = 1
	END

	RETURN @result
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetAppointmentsBetween]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetAppointmentsBetween]
(
	@laboratory INT,		-- The laboratory identity.
	@start_date DATETIME,	-- The start datetime.
	@end_date DATETIME		-- The end datetime.
)
RETURNS INT
AS
BEGIN
	DECLARE @count INT;
	
	SELECT @count = COUNT(1) FROM Appointments AS A
	INNER JOIN States AS S ON A.FK_State_Id = S.PK_State_Id
	WHERE S.Name <> 'Cancelada' AND A.Date >= @start_date AND A.Date < @end_date
	
	RETURN @count
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetReservation]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetReservation]
(
	@requester_id INT,		-- The requester user identity.
	@id		INT,			-- The appointment id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	DECLARE @professor_id INT;
	
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Docente')
	BEGIN
		SELECT @professor_id = FK_Professor_Id FROM Reservations WHERE PK_Reservation_Id = @id; 
		IF (@professor_id <> @requester_id)
		BEGIN
			RAISERROR('La reservación ingresada pertenece a otro docente.', 15, 1);
			RETURN -1;
		END
	END
	
	EXEC dbo.sp_GetOne 'vw_Reservations', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAppointment]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAppointment]
(
	@requester_id INT,		-- The requester user identity.
	@id		INT,			-- The appointment id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@id AS VARCHAR);
	DECLARE @student_id INT;
	
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Estudiante')
	BEGIN
		SELECT @student_id = FK_Student_Id FROM Appointments WHERE PK_Appointment_Id = @id; 
		IF (@student_id <> @requester_id)
		BEGIN
			RAISERROR('La cita ingresada pertenece a otro estudiante.', 15, 1);
			RETURN -1;
		END
	END
	
	EXEC dbo.sp_GetOne 'vw_Appointments', @fields, @where;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteReservation]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteReservation]
(
	@requester_id INT,		-- The requester user identity.
	@id		INT				-- The reservation id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	DECLARE @professor_id INT;
	
	-- Check if appointment exists.
	IF NOT EXISTS (SELECT 1 FROM Reservations WHERE PK_Reservation_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Reservación no encontrada.', 15, 1);
		RETURN -1;
	END
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Docente')
	BEGIN
		SELECT @professor_id = FK_Professor_Id FROM Reservations WHERE PK_Reservation_Id = @id; 
		IF (@professor_id <> @requester_id)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('La reservación ingresada pertenece a otro docente.', 15, 1);
			RETURN -1;
		END
	END
	
	UPDATE Reservations
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('RESERVATION', 'Cancelada'), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE PK_Reservation_Id = @id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteAppointment]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteAppointment]
(
	@requester_id INT,		-- The requester user identity.
	@id		INT				-- The appointment id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	DECLARE @student_id INT;
	
	-- Check if appointment exists.
	IF NOT EXISTS (SELECT 1 FROM Appointments WHERE PK_Appointment_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Cita no encontrada.', 15, 1);
		RETURN -1;
	END
	
	IF (dbo.fn_GetUserType(@requester_id) = 'Estudiante')
	BEGIN
		SELECT @student_id = FK_Student_Id FROM Appointments WHERE PK_Appointment_Id = @id; 
		IF (@student_id <> @requester_id)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('La cita ingresada pertenece a otro estudiante.', 15, 1);
			RETURN -1;
		END
	END
	
	UPDATE Appointments
	SET [FK_State_Id] = ISNULL(dbo.fn_GetStateID('APPOINTMENT', 'Cancelada'), [FK_State_Id]),
	[Updated_At] = GETDATE()
	WHERE [PK_Appointment_Id] = @id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Authenticate]    Script Date: 10/14/2015 08:20:25 ******/
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
	SET NOCOUNT ON;
	
	SELECT TOP 1
		U.PK_User_Id AS id, U.Username AS username, U.Name AS name, 
		U.Last_Name_1 AS last_name_1, U.Last_Name_2 AS last_name_2,
		RTRIM(U.Name + ' ' + U.Last_Name_1 + ' ' + ISNULL(U.Last_Name_2, '')) AS full_name,
		U.Gender AS gender, U.Email AS email, U.Phone AS phone,
		U.Created_At AS created_at, U.Updated_At AS updated_at,
		dbo.fn_GetUserType(U.PK_User_Id) AS type
	FROM Users AS U
	INNER JOIN States AS S ON S.PK_State_Id = U.FK_State_Id 
	WHERE U.Username = @username AND U.Password = @password AND S.Name = 'Activo';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddStudentsToGroup]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AddStudentsToGroup]
(
	@requester_id INT,				-- The identity of the requester user.
	@group INT,						-- The group id.
	@students AS UserList READONLY	-- The list of students usernames.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	INSERT INTO StudentsByGroup(FK_Group_Id, FK_Student_Id)
	SELECT @group, U.PK_User_Id
	FROM @students AS S
	INNER JOIN Users AS U ON U.Username = S.Username
	WHERE NOT EXISTS (SELECT 1 FROM StudentsByGroup WHERE FK_Group_Id = @group AND FK_Student_Id = U.PK_User_Id);
	
	UPDATE Groups
	SET Updated_At = GETDATE()
	WHERE PK_Group_Id = @group;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteUser]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteUser]
(
	@requester_id	INT,	-- The identity of the requester user.
	@user_id		INT		-- The User Id.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	IF NOT EXISTS (SELECT 1 FROM vw_Users WHERE id = @user_id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Usuario no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Users
	SET FK_State_Id = dbo.fn_GetStateID('USER', 'Inactivo'),
	Updated_At = GETDATE()
	WHERE PK_User_Id = @user_id;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateReservation]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateReservation]
(
	@requester_id INT,			-- The identity of the requester user.
	@professor VARCHAR(70),		-- The professor username.
	@start_time DATETIME,		-- The reservation start time.
	@end_time DATETIME,			-- The reservation end time.
	@group INT,					-- [nullable] The group identity.
	@laboratory VARCHAR(500),	-- [nullable] The laboratory name. If NULL then laboratory will be assigned automatically.
	@software VARCHAR(20)		-- [nullable] The software code.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	DECLARE @id INT, @professorId INT, @laboratoryId INT, @softwareId INT;
	DECLARE @isValidDate BIT;
	
	-- Get the professor identity.
	SELECT @professorId = FK_User_Id FROM Professors AS P INNER JOIN Users AS U ON P.FK_User_Id = U.PK_User_Id WHERE U.Username = @professor;
	IF @professorId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El docente ingresado no existe: %s.', 15, 1, @professor);
		RETURN -1;
	END
	-- Throw error if a professor is creating a reservation for other professor.
	IF dbo.fn_GetUserType(@requester_id) = 'Docente' AND @requester_id <> @professorId
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('No se permite realizar reservaciones para otros docentes.', 15, 1);
		RETURN -1;
	END
	
	-- Get the software identity.
	SELECT @softwareId = PK_Software_Id FROM Software WHERE Code = @software;
	IF @software IS NOT NULL AND @softwareId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El software ingresado no existe: %s.', 15, 1, @software);
		RETURN -1;
	END
	
	-- Get the laboratory identity.
	IF @laboratory IS NULL
	BEGIN
		SELECT TOP 1 @laboratoryId = PK_Laboratory_Id FROM Laboratories
		WHERE dbo.fn_IsLaboratoryReserved(PK_Laboratory_Id, @start_time, @end_time) = 0
		AND dbo.fn_GetAppointmentsBetween(PK_Laboratory_Id, @start_time, @end_time) = 0
		ORDER BY Reservation_Priority;

		IF @laboratoryId IS NULL
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('No se encuentran laboratorios disponibles durante el tiempo ingresado.', 15, 1);
			RETURN -1;
		END
	END
	ELSE
	BEGIN
		SELECT @laboratoryId = PK_Laboratory_Id FROM Laboratories WHERE Name = @laboratory;
		IF @laboratoryId IS NULL
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El laboratorio ingresado no existe: %s.', 15, 1, @laboratory);
			RETURN -1;
		END
		
		-- Check that the laboratory is available in the given time.
		IF NOT EXISTS
		(
			SELECT 1 FROM Laboratories
			WHERE PK_Laboratory_Id = @laboratoryId
			AND dbo.fn_IsLaboratoryReserved(PK_Laboratory_Id, @start_time, @end_time) = 0
			AND dbo.fn_GetAppointmentsBetween(PK_Laboratory_Id, @start_time, @end_time) = 0
		)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El laboratorio no se encuentra disponible durante el tiempo ingresado.', 15, 1);
			RETURN -1;
		END
	END
	
	IF @group IS NOT NULL
	BEGIN
		-- Check if the group belongs to the professor.
		IF NOT EXISTS (SELECT 1 FROM Groups WHERE PK_Group_Id = @group AND FK_Professor_Id = @professorId)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El grupo ingresado pertenece a otro docente.', 15, 1);
			RETURN -1;
		END
		
		-- Check if the date match the group period.
		SELECT TOP 1 @isValidDate = dbo.fn_IsDateInPeriod(@start_time, P.Value, PT.Name, G.Period_Year) & dbo.fn_IsDateInPeriod(@end_time, P.Value, PT.Name, G.Period_Year)
		FROM Groups AS G
		INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
		INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
		WHERE G.PK_Group_Id = @group;
		
		IF @isValidDate = 0
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('La fecha ingresada no coincide con el periodo lectivo del grupo.', 15, 1);
			RETURN -1;
		END
	END
	
	-- Insert reservation.
	INSERT INTO Reservations(Start_Time, End_Time, FK_Professor_Id, FK_Group_Id, FK_Laboratory_Id, FK_Software_Id, FK_State_Id) VALUES
	(@start_time, @end_time, @professorId, @group, @laboratoryId, @softwareId, dbo.fn_GetStateID('RESERVATION', 'Por iniciar'));
	SET @id = SCOPE_IDENTITY();
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetReservation @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateAppointment]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateAppointment]
(
	@requester_id INT,			-- The identity of the requester user.
	@student VARCHAR(70),		-- The student username.
	@laboratory VARCHAR(500),	-- [nullable] The laboratory name. If NULL then laboratory will be assigned automatically.
	@software VARCHAR(20),		-- The software code.
	@group INT,					-- The group identity.
	@date DATETIME				-- The appointment datetime.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	DECLARE @id INT, @studentId INT, @laboratoryId INT, @softwareId INT;
	DECLARE @isValidDate BIT;
	
	-- Get the student identity.
	SELECT @studentId = FK_User_Id FROM Students AS S INNER JOIN Users AS U ON S.FK_User_Id = U.PK_User_Id WHERE U.Username = @student;
	IF @studentId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El estudiante ingresado no existe: %s.', 15, 1, @student);
		RETURN -1;
	END
	-- Throw error if a student is creating an appointment for other student.
	IF dbo.fn_GetUserType(@requester_id) = 'Estudiante' AND @requester_id <> @studentId
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('No se permite realizar citas para otros estudiantes.', 15, 1);
		RETURN -1;
	END
	
	-- Check if appointment exists.
	IF EXISTS (SELECT 1 FROM Appointments WHERE FK_Student_Id = @studentId AND Date = @date)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe una cita con los datos proporcionados.', 15, 1);
		RETURN -1;
	END
	
	-- Check if user exceed the maximun weekly appointments.
	IF (dbo.fn_GetStudentWeeklyAppointmentsCount(@studentId, @date) >= 2)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Se alcanzó el máximo de citas disponibles para la semana ingresada.', 15, 1);
		RETURN -1;
	END

	-- Get the software identity.
	SELECT @softwareId = PK_Software_Id FROM Software WHERE Code = @software;
	IF @softwareId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El software ingresado no existe: %s.', 15, 1, @software);
		RETURN -1;
	END
	
	-- Get the laboratory identity.
	IF @laboratory IS NULL
	BEGIN
		-- Select the first laboratory, ordered by the appointment priority, that is not reserved and have available seats.
		SELECT TOP 1 @laboratoryId = PK_Laboratory_Id FROM Laboratories
		WHERE dbo.fn_IsLaboratoryReserved(PK_Laboratory_Id, @date, @date) = 0
		AND dbo.fn_GetLaboratoryAvailableSeats(PK_Laboratory_Id, @date) > 0
		ORDER BY Appointment_Priority;

		IF @laboratoryId IS NULL
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('No se encuentran laboratorios disponibles en la hora ingresada.', 15, 1);
			RETURN -1;
		END
	END
	ELSE
	BEGIN
		SELECT @laboratoryId = PK_Laboratory_Id FROM Laboratories WHERE Name = @laboratory;
		IF @laboratoryId IS NULL
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El laboratorio ingresado no existe: %s.', 15, 1, @laboratory);
			RETURN -1;
		END
		
		-- Check if the laboratory have available seats.
		IF (dbo.fn_GetLaboratoryAvailableSeats(@laboratoryId, @date) <= 0)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('No hay espacios disponibles en este laboratorio en la hora ingresada.', 15, 1);
			RETURN -1;
		END
		
		-- Check if laboratory is reserved.
		IF (dbo.fn_IsLaboratoryReserved(@laboratoryId, @date, @date) = 1)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El laboratorio se encuentra reservado en la hora ingresada.', 15, 1);
			RETURN -1;
		END
	END

	-- Check that the group belongs to the user.
	IF NOT EXISTS (SELECT 1 FROM StudentsByGroup WHERE FK_Group_Id = @group AND FK_Student_Id = @studentId)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El estudiante no pertenece al grupo ingresado.', 15, 1);
		RETURN -1;
	END
	
	-- Check if the date match the group period.
	SELECT TOP 1 @isValidDate = dbo.fn_IsDateInPeriod(@date, P.Value, PT.Name, G.Period_Year)
	FROM Groups AS G
	INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
	INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
	WHERE G.PK_Group_Id = @group;
	
	IF @isValidDate = 0
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('La fecha ingresada no coincide con el periodo lectivo del grupo.', 15, 1);
		RETURN -1;
	END
	
	-- Insert appointment.
	INSERT INTO Appointments(Date, FK_Student_Id, FK_Laboratory_Id, FK_Software_Id, FK_Group_Id, FK_State_Id) VALUES
	(@date, @studentId, @laboratoryId, @softwareId, @group, dbo.fn_GetStateID('APPOINTMENT', 'Por iniciar'));
	SET @id = SCOPE_IDENTITY();
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetAppointment @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateGroup]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateGroup]
(
	@requester_id	INT,					-- The identity of the requester user.
	@course			VARCHAR(20),			-- The course code.
	@number			INT,					-- The group number.
	@professor		VARCHAR(70),			-- The professor username.
	@period_value	INT,					-- The period value. (1, 2, ...)
	@period_type	VARCHAR(50),			-- The period type ('Semestre', 'Bimestre', ...)
	@period_year	INT,					-- The year. (2014, 2015, ...)
	@students		AS UserList READONLY	-- The list of students usernames.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DECLARE @groupId INT, @courseId INT, @professorId INT, @periodId INT;
	SELECT @courseId = PK_Course_Id FROM Courses WHERE Code = @course;
	SELECT @professorId = PK_User_Id FROM Users WHERE Username = @professor;
	SELECT @periodId = PK_Period_Id FROM Periods INNER JOIN PeriodTypes ON FK_Period_Type_Id = PK_Period_Type_Id WHERE Value = @period_value AND Name = @period_type;
	
	IF @courseId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El curso ingresado no existe: %s.', 15, 1, @course);
		RETURN -1;
	END
	
	IF @professorId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El profesor ingresado no existe: %s.', 15, 1, @professor);
		RETURN -1;
	END
	
	IF @courseId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El periodo ingresado es inválido.', 15, 1);
		RETURN -1;
	END
	
	-- Check if group exists.
	IF EXISTS (SELECT 1 FROM Groups WHERE FK_Course_Id = @courseId AND FK_Professor_Id = @professorId AND FK_Period_Id = @periodId AND Period_Year = @period_year AND Number = @number)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un grupo con estos datos.', 15, 1);
		RETURN -1;
	END
	
	INSERT INTO Groups(FK_Course_Id, FK_Professor_Id, FK_Period_Id, Period_Year, Number, FK_State_Id) VALUES
	(@courseId, @professorId, @periodId, @period_year, @number, dbo.fn_GetStateID('GROUP', 'Activo'));
	SET @groupId = SCOPE_IDENTITY();
	
	IF EXISTS (SELECT 1 FROM @students)
	BEGIN
		EXEC dbo.sp_AddStudentsToGroup @requester_id, @groupId, @students;
	END
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetGroup @requester_id, @groupId, '*';
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetAvailableAppointments]    Script Date: 10/14/2015 08:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_GetAvailableAppointments]
(
	@id INT -- The id of the student who is making the appointment.
) RETURNS TABLE
AS RETURN
(
	SELECT * FROM
	(
		SELECT ROW_NUMBER() OVER ( PARTITION BY [Date] ORDER BY [laboratory.appointment_priority]) RN, *  
		FROM
		(
			SELECT R.Date, dbo.fn_GetLaboratoryAvailableSeats(L.PK_Laboratory_Id, R.Date) AS [spaces],
			L.PK_Laboratory_Id AS [laboratory.id], L.Name AS [laboratory.name], L.Seats AS [laboratory.seats], 
			S.Name AS [laboratory.state], L.Created_At AS [laboratory.created_at], 
			L.Updated_At AS [laboratory.updated_at], L.Appointment_Priority AS [laboratory.appointment_priority], 
            L.Reservation_Priority AS [laboratory.reservation_priority]
			FROM dbo.fn_GetDateTimeRange(GETDATE(), 2) AS R, Laboratories AS L
			INNER JOIN States AS S ON L.FK_State_Id = S.PK_State_Id
			WHERE dbo.fn_GetLaboratoryAvailableSeats(L.PK_Laboratory_Id, R.Date) > 0
			AND dbo.fn_IsLaboratoryReserved(L.PK_Laboratory_Id, R.Date, R.Date) = 0
			AND dbo.fn_GetStudentWeeklyAppointmentsCount(@id, R.Date) < 2
		) InnerDateRange
	) DateRange
	WHERE RN = 1
)
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateReservation]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateReservation]
(
	@requester_id INT,			-- The identity of the requester user.
	@id INT,					-- The reservation identity.
	@professor VARCHAR(70),		-- The professor username.
	@start_time DATETIME,		-- The reservation start time.
	@end_time DATETIME,			-- The reservation end time.
	@group INT,					-- The group identity.
	@laboratory VARCHAR(500),	-- The laboratory name. If NULL then laboratory will be assigned automatically.
	@software VARCHAR(20),		-- The software code.
	@state VARCHAR(30)			-- The reservation state.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DECLARE @professorId INT, @laboratoryId INT, @softwareId INT;
	DECLARE @currentProfessorId INT, @currentLaboratoryId INT, @currentGroupId INT, @currentStartTime DATETIME, @currentEndTime DATETIME;
	DECLARE @isValidDate BIT;
	
	SELECT @currentProfessorId = R.FK_Professor_Id, @currentLaboratoryId = R.FK_Laboratory_Id, @currentGroupId = R.FK_Group_Id, @currentStartTime = R.Start_Time, @currentEndTime = R.End_Time FROM Reservations AS R WHERE R.PK_Reservation_Id = @id;
	
	-- Get the professor identity.
	SELECT @professorId = FK_User_Id FROM Professors AS P INNER JOIN Users AS U ON P.FK_User_Id = U.PK_User_Id WHERE U.Username = @professor;
	IF @professor IS NOT NULL AND @professorId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El docente ingresado no existe: %s.', 15, 1, @professor);
		RETURN -1;
	END
	
	-- Get the laboratory identity.
	SELECT @laboratoryId = PK_Laboratory_Id FROM Laboratories WHERE Name = @laboratory;
	IF @laboratoryId IS NULL AND @laboratory IS NOT NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El laboratorio ingresado no existe: %s.', 15, 1, @laboratory);
		RETURN -1;
	END
	
	-- Get the software identity.
	IF @software = ''
	BEGIN
		SET @softwareId = 0;
	END
	ELSE
	BEGIN
		IF @software IS NOT NULL
		BEGIN
			SELECT @softwareId = PK_Software_Id FROM Software WHERE Code = @software;
			IF @softwareId IS NULL
			BEGIN
				ROLLBACK TRANSACTION T;
				RAISERROR('El software ingresado no existe: %s.', 15, 1, @software);
				RETURN -1;
			END
		END
	END
	
	-- Check if the group belongs to the professor.
	IF @group IS NOT NULL AND @group <> 0
	BEGIN
		SET @professorId = COALESCE(@professorId, @currentProfessorId);
		IF NOT EXISTS (SELECT 1 FROM Groups WHERE PK_Group_Id = @group AND FK_Professor_Id = @professorId)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El grupo ingresado pertenece a otro docente.', 15, 1);
			RETURN -1;
		END
	END
	
	-- Check that the laboratory is available in the given time.
	IF (@start_time IS NOT NULL OR @end_time IS NOT NULL OR @laboratory IS NOT NULL)
	BEGIN
		SET @start_time = COALESCE(@start_time, @currentStartTime);
		SET @end_time = COALESCE(@end_time, @currentEndTime);
		SET @laboratoryId = COALESCE(@laboratoryId, @currentLaboratoryId);
		SET @group = COALESCE(@group, @currentGroupId);
		
		IF (@start_time <> @currentStartTime OR @end_time <> @currentEndTime OR @laboratoryId <> @currentLaboratoryId)
		BEGIN
			IF dbo.fn_IsLaboratoryReserved(@laboratoryId, @start_time, @end_time) = 1 OR
			   dbo.fn_GetAppointmentsBetween(@laboratoryId, @start_time, @end_time) > 0
			BEGIN
				ROLLBACK TRANSACTION T;
				RAISERROR('El laboratorio no se encuentra disponible durante el tiempo ingresado.', 15, 1);
				RETURN -1;
			END
		END
		
		IF @group IS NOT NULL
		BEGIN
			-- Check if the date match the group period.
			SELECT TOP 1
			@isValidDate = dbo.fn_IsDateInPeriod(@start_time, P.Value, PT.Name, G.Period_Year) &
						   dbo.fn_IsDateInPeriod(@end_time, P.Value, PT.Name, G.Period_Year)
			FROM Groups AS G
			INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
			INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
			WHERE G.PK_Group_Id = @group;
			
			IF @isValidDate = 0
			BEGIN
				ROLLBACK TRANSACTION T;
				RAISERROR('La fecha ingresada no coincide con el periodo lectivo del grupo.', 15, 1);
				RETURN -1;
			END
		END
	END
	
	UPDATE Reservations SET
	Start_Time = ISNULL(@start_time, Start_Time),
	End_Time = ISNULL(@end_time, End_Time),
	FK_Professor_Id = ISNULL(@professorId, FK_Professor_Id),
	FK_Laboratory_Id = ISNULL(@laboratoryId, FK_Laboratory_Id),
	FK_Software_Id = CAST(dbo.fn_IsNull(@softwareId, FK_Software_Id) AS INT),
	FK_Group_Id = CAST(dbo.fn_IsNull(@group, FK_Group_Id) AS INT),
	FK_State_Id = ISNULL(dbo.fn_GetStateID('RESERVATION', @state), FK_State_Id),
	Updated_At = GETDATE()
	WHERE PK_Reservation_Id = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetReservation @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateAppointment]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateAppointment]
(
	@requester_id INT,
	@id INT,
	@student VARCHAR(70),
	@laboratory VARCHAR(500),
	@software VARCHAR(20),
	@attendance BIT,
	@date DATETIME,
	@group INT,
	@state VARCHAR(30)
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DECLARE @studentId INT, @laboratoryId INT, @softwareId INT;
	DECLARE @currentStudentId INT, @currentLaboratoryId INT, @currentDate DATETIME, @isValidDate BIT;
	
	SELECT @studentId = FK_User_Id FROM Students AS S INNER JOIN Users AS U ON S.FK_User_Id = U.PK_User_Id WHERE U.Username = @student;
	SELECT @laboratoryId = PK_Laboratory_Id FROM Laboratories WHERE Name = @laboratory;
	SELECT @softwareId = PK_Software_Id FROM Software WHERE Code = @software;
	
	SELECT @currentDate = [Date], @currentStudentId = FK_Student_Id, @currentLaboratoryId = [FK_Laboratory_Id] FROM Appointments WHERE PK_Appointment_Id = @id;

	IF @student IS NOT NULL AND @studentId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El estudiante ingresado no existe: %s.', 15, 1, @student);
		RETURN -1;
	END
	
	IF @laboratoryId IS NULL AND @laboratory IS NOT NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El laboratorio ingresado no existe: %s.', 15, 1, @laboratory);
		RETURN -1;
	END
	
	IF @software IS NOT NULL AND @softwareId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El software ingresado no existe: %s.', 15, 1, @software);
		RETURN -1;
	END
	
	IF (@date IS NOT NULL OR @laboratory IS NOT NULL)
	BEGIN
		SET @date = COALESCE(@date, @currentDate);
		SET @laboratoryId = COALESCE(@laboratoryId, @currentLaboratoryId);
		
		IF (@date <> @currentDate OR @laboratoryId <> @currentLaboratoryId)
		BEGIN
			IF (dbo.fn_IsLaboratoryReserved(@laboratoryId, @date, @date) = 1 OR
			dbo.fn_GetLaboratoryAvailableSeats(@laboratoryId, @date) = 0)
			BEGIN
				ROLLBACK TRANSACTION T;
				RAISERROR('El laboratorio no se encuentra disponible en la hora indicada.', 15, 1);
				RETURN -1;
			END
		END
	END
	
	IF @group IS NOT NULL
	BEGIN
		SET @date = COALESCE(@date, @currentDate);
		SET @studentId = COALESCE(@studentId, @currentStudentId);

		-- Check that the group belongs to the user.
		IF NOT EXISTS (SELECT 1 FROM StudentsByGroup WHERE FK_Group_Id = @group AND FK_Student_Id = @studentId)
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('El estudiante no pertenece al grupo ingresado.', 15, 1);
			RETURN -1;
		END
		
		-- Check if the date match the group period.
		SELECT TOP 1 @isValidDate = dbo.fn_IsDateInPeriod(@date, P.Value, PT.Name, G.Period_Year)
		FROM Groups AS G
		INNER JOIN Periods AS P ON G.FK_Period_Id = P.PK_Period_Id
		INNER JOIN PeriodTypes AS PT ON P.FK_Period_Type_Id = PT.PK_Period_Type_Id
		WHERE G.PK_Group_Id = @group;
		
		IF @isValidDate = 0
		BEGIN
			ROLLBACK TRANSACTION T;
			RAISERROR('La fecha ingresada no coincide con el periodo lectivo del grupo.', 15, 1);
			RETURN -1;
		END
	END
	
	SET @date = COALESCE(@date, @currentDate);
	IF @attendance IS NOT NULL AND @date > GETDATE()
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('No se permite modificar la asistencia de citas futuras.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Appointments SET
	[Date] = ISNULL(@date, [Date]),
	Attendance = ISNULL(@attendance, Attendance),
	FK_Student_Id = ISNULL(@studentId, FK_Student_Id),
	FK_Laboratory_Id = ISNULL(@laboratoryId, FK_Laboratory_Id),
	FK_Software_Id = ISNULL(@softwareId, FK_Software_Id),
	FK_Group_Id = ISNULL(@group, FK_Group_Id),
	FK_State_Id = ISNULL(dbo.fn_GetStateID('APPOINTMENT', @state), FK_State_Id),
	Updated_At = GETDATE()
	WHERE PK_Appointment_Id = @id;
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetAppointment @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUser]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateUser]
(
	@requester_id   INT,            -- The identity of the requester user.
	@id				INT,			-- The user id.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20),	-- The phone. [Nullable]
	@state			VARCHAR(30)		-- The state. ('active', 'disabled', 'blocked')
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if user exists.
	IF NOT EXISTS (SELECT 1 FROM vw_Users WHERE Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Usuario no encontrado.', 15, 1);
		RETURN -1;
	END
	ELSE
	BEGIN
		EXEC dbo.sp_UpdateUserData @requester_id, @id, @name, @last_name_1, @last_name_2, @gender, @username, @password, @email, @phone, @state;
		COMMIT TRANSACTION T;
	END
	
	EXEC dbo.sp_GetUser @requester_id, @id, '*';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateGroupStudents]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateGroupStudents]
(
	@requester_id INT,			-- The identity of the requester user.
	@group INT,						-- The group id.
	@students AS UserList READONLY	-- The list of students usernames.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	DELETE StudentsByGroup
	WHERE FK_Group_Id = @group;
	
	EXEC dbo.sp_AddStudentsToGroup @requester_id, @group, @students;
	
	COMMIT TRANSACTION T;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateGroup]    Script Date: 10/14/2015 08:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateGroup]
(
	@requester_id INT,				-- The identity of the requester user.
	@id	INT,						-- The group id.
	@course	VARCHAR(20),			-- The course code.
	@number	INT,					-- The group number.
	@professor VARCHAR(70),			-- The professor username.
	@period_value	INT,			-- The period value. (1, 2, ...)
	@period_type	VARCHAR(50),	-- The period type ('Semestre', 'Bimestre', ...)
	@period_year	INT,			-- The year. (2014, 2015, ...)
	@state VARCHAR(30),				-- The group state.
	@students AS UserList READONLY	-- The list of students usernames.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if group exists.
	IF NOT EXISTS (SELECT 1 FROM Groups WHERE PK_Group_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Grupo no encontrado.', 15, 1);
		RETURN -1;
	END
	
	DECLARE @courseId INT, @professorId INT, @periodId INT;
	SELECT @courseId = PK_Course_Id FROM Courses WHERE Code = @course;
	SELECT @professorId = PK_User_Id FROM Users WHERE Username = @professor;
	SELECT @periodId = PK_Period_Id FROM Periods INNER JOIN PeriodTypes ON FK_Period_Type_Id = PK_Period_Type_Id WHERE Value = @period_value AND Name = @period_type;
	
	IF @course IS NOT NULL AND @courseId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El curso ingresado no existe: %s.', 15, 1, @course);
		RETURN -1;
	END
	
	IF @professor IS NOT NULL AND @professorId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El profesor ingresado no existe: %s.', 15, 1, @professor);
		RETURN -1;
	END
	
	IF (@period_value IS NOT NULL OR @period_type IS NOT NULL) AND @periodId IS NULL
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('El periodo ingresado es inválido.', 15, 1);
		RETURN -1;
	END
	
	-- Check if group exists.
	IF EXISTS (SELECT 1 FROM Groups WHERE FK_Course_Id = @courseId AND FK_Professor_Id = @professorId AND FK_Period_Id = @periodId AND Period_Year = @period_year AND Number = @number AND PK_Group_Id <> @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Ya existe un grupo con estos datos.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Groups
	SET FK_Course_Id = ISNULL(@courseId, FK_Course_Id),
	FK_Professor_Id = ISNULL(@professorId, FK_Professor_Id),
	FK_Period_Id = ISNULL(@periodId, FK_Period_Id),
	FK_State_Id = ISNULL(dbo.fn_GetStateID('GROUP', @state), FK_State_Id),
	Number = ISNULL(@number, Number),
	Period_Year = ISNULL(@period_year, Period_Year),
	Updated_At = GETDATE()
	WHERE PK_Group_Id = @id;
	
	IF EXISTS (SELECT 1 FROM @students)
	BEGIN
		EXEC dbo.sp_UpdateGroupstudents @requester_id, @id, @students;
	END
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetGroup @requester_id, @id, '*';
END
GO
/****** Object:  Default [DF_Administrators_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Administrators] ADD  CONSTRAINT [DF_Administrators_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Administrators_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Administrators] ADD  CONSTRAINT [DF_Administrators_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Appointments_Attendance]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Attendance]  DEFAULT ((0)) FOR [Attendance]
GO
/****** Object:  Default [DF_Appointments_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Appointments_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Courses_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Courses] ADD  CONSTRAINT [DF_Courses_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Courses_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Courses] ADD  CONSTRAINT [DF_Courses_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Groups_Period_Year]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Period_Year]  DEFAULT (datepart(year,getdate())) FOR [Period_Year]
GO
/****** Object:  Default [DF_Groups_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Groups_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Laboratories_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Laboratories_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Laboratories_Appointment_Priority]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Appointment_Priority]  DEFAULT ((1)) FOR [Appointment_Priority]
GO
/****** Object:  Default [DF_Laboratories_Reservation_Priority]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Reservation_Priority]  DEFAULT ((1)) FOR [Reservation_Priority]
GO
/****** Object:  Default [DF_Operators_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Operators] ADD  CONSTRAINT [DF_Operators_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Operators_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Operators] ADD  CONSTRAINT [DF_Operators_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Operators_Period_Year]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Operators] ADD  CONSTRAINT [DF_Operators_Period_Year]  DEFAULT (datepart(year,getdate())) FOR [Period_Year]
GO
/****** Object:  Default [DF_Reservations_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations] ADD  CONSTRAINT [DF_Reservations_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Reservations_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations] ADD  CONSTRAINT [DF_Reservations_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Software_Created_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Software] ADD  CONSTRAINT [DF_Software_Created_At]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Software_Updated_At]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Software] ADD  CONSTRAINT [DF_Software_Updated_At]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Default [DF_Users_created_at]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_created_at]  DEFAULT (getdate()) FOR [Created_At]
GO
/****** Object:  Default [DF_Users_updated_at]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_updated_at]  DEFAULT (getdate()) FOR [Updated_At]
GO
/****** Object:  Check [CK_Reservations_Time]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [CK_Reservations_Time] CHECK  (([End_Time]>[Start_Time]))
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [CK_Reservations_Time]
GO
/****** Object:  ForeignKey [FK_Administrators_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Administrators]  WITH CHECK ADD  CONSTRAINT [FK_Administrators_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Administrators] CHECK CONSTRAINT [FK_Administrators_States]
GO
/****** Object:  ForeignKey [FK_Administrators_Users]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Administrators]  WITH CHECK ADD  CONSTRAINT [FK_Administrators_Users] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Users] ([PK_User_Id])
GO
ALTER TABLE [dbo].[Administrators] CHECK CONSTRAINT [FK_Administrators_Users]
GO
/****** Object:  ForeignKey [FK_Appointments_Groups]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Groups] FOREIGN KEY([FK_Group_Id])
REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Groups]
GO
/****** Object:  ForeignKey [FK_Appointments_Laboratories]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Laboratories]
GO
/****** Object:  ForeignKey [FK_Appointments_Software]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Software] FOREIGN KEY([FK_Software_Id])
REFERENCES [dbo].[Software] ([PK_Software_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Software]
GO
/****** Object:  ForeignKey [FK_Appointments_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_States]
GO
/****** Object:  ForeignKey [FK_Appointments_Students]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Students] FOREIGN KEY([FK_Student_Id])
REFERENCES [dbo].[Students] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Students]
GO
/****** Object:  ForeignKey [FK_Courses_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_States]
GO
/****** Object:  ForeignKey [FK_Groups_Courses]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Courses] FOREIGN KEY([FK_Course_Id])
REFERENCES [dbo].[Courses] ([PK_Course_Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Courses]
GO
/****** Object:  ForeignKey [FK_Groups_Periods]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Periods] FOREIGN KEY([FK_Period_Id])
REFERENCES [dbo].[Periods] ([PK_Period_Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Periods]
GO
/****** Object:  ForeignKey [FK_Groups_Professors]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Professors] FOREIGN KEY([FK_Professor_Id])
REFERENCES [dbo].[Professors] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Professors]
GO
/****** Object:  ForeignKey [FK_Groups_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_States]
GO
/****** Object:  ForeignKey [FK_Laboratories_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Laboratories]  WITH CHECK ADD  CONSTRAINT [FK_Laboratories_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Laboratories] CHECK CONSTRAINT [FK_Laboratories_States]
GO
/****** Object:  ForeignKey [FK_Operators_Periods]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_Periods] FOREIGN KEY([FK_Period_Id])
REFERENCES [dbo].[Periods] ([PK_Period_Id])
GO
ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_Periods]
GO
/****** Object:  ForeignKey [FK_Operators_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_States]
GO
/****** Object:  ForeignKey [FK_Operators_Students]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_Students] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Students] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_Students]
GO
/****** Object:  ForeignKey [FK_Periods_PeriodTypes]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Periods]  WITH CHECK ADD  CONSTRAINT [FK_Periods_PeriodTypes] FOREIGN KEY([FK_Period_Type_Id])
REFERENCES [dbo].[PeriodTypes] ([PK_Period_Type_Id])
GO
ALTER TABLE [dbo].[Periods] CHECK CONSTRAINT [FK_Periods_PeriodTypes]
GO
/****** Object:  ForeignKey [FK_Professors_Users]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Professors]  WITH CHECK ADD  CONSTRAINT [FK_Professors_Users] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Users] ([PK_User_Id])
GO
ALTER TABLE [dbo].[Professors] CHECK CONSTRAINT [FK_Professors_Users]
GO
/****** Object:  ForeignKey [FK_Reservations_Groups]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Groups] FOREIGN KEY([FK_Group_Id])
REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Groups]
GO
/****** Object:  ForeignKey [FK_Reservations_Laboratories]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Laboratories]
GO
/****** Object:  ForeignKey [FK_Reservations_Professors]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Professors] FOREIGN KEY([FK_Professor_Id])
REFERENCES [dbo].[Professors] ([FK_User_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Professors]
GO
/****** Object:  ForeignKey [FK_Reservations_Software]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Software] FOREIGN KEY([FK_Software_Id])
REFERENCES [dbo].[Software] ([PK_Software_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Software]
GO
/****** Object:  ForeignKey [FK_Reservations_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_States]
GO
/****** Object:  ForeignKey [FK_Software_States]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Software]  WITH CHECK ADD  CONSTRAINT [FK_Software_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO
ALTER TABLE [dbo].[Software] CHECK CONSTRAINT [FK_Software_States]
GO
/****** Object:  ForeignKey [FK_SoftwareByLaboratory_Laboratories]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[SoftwareByLaboratory]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareByLaboratory_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO
ALTER TABLE [dbo].[SoftwareByLaboratory] CHECK CONSTRAINT [FK_SoftwareByLaboratory_Laboratories]
GO
/****** Object:  ForeignKey [FK_SoftwareByLaboratory_Software]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[SoftwareByLaboratory]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareByLaboratory_Software] FOREIGN KEY([FK_Software_Id])
REFERENCES [dbo].[Software] ([PK_Software_Id])
GO
ALTER TABLE [dbo].[SoftwareByLaboratory] CHECK CONSTRAINT [FK_SoftwareByLaboratory_Software]
GO
/****** Object:  ForeignKey [FK_Students_Users]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Users] FOREIGN KEY([FK_User_Id])
REFERENCES [dbo].[Users] ([PK_User_Id])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Users]
GO
/****** Object:  ForeignKey [FK_StudentsByGroup_Groups]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[StudentsByGroup]  WITH CHECK ADD  CONSTRAINT [FK_StudentsByGroup_Groups] FOREIGN KEY([FK_Group_Id])
REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO
ALTER TABLE [dbo].[StudentsByGroup] CHECK CONSTRAINT [FK_StudentsByGroup_Groups]
GO
/****** Object:  ForeignKey [FK_StudentsByGroup_Students]    Script Date: 10/14/2015 08:20:27 ******/
ALTER TABLE [dbo].[StudentsByGroup]  WITH CHECK ADD  CONSTRAINT [FK_StudentsByGroup_Students] FOREIGN KEY([FK_Student_Id])
REFERENCES [dbo].[Students] ([FK_User_Id])
GO
ALTER TABLE [dbo].[StudentsByGroup] CHECK CONSTRAINT [FK_StudentsByGroup_Students]
GO
/****** Object:  ForeignKey [FK_Users_States]    Script Date: 10/14/2015 08:20:27 ******/
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

IF NOT EXISTS (SELECT loginname FROM syslogins WHERE name = 'WebServiceLogin' and dbname = 'SiLabI')
BEGIN
	CREATE LOGIN [WebServiceLogin] WITH PASSWORD = 'password', DEFAULT_DATABASE=[SiLabI], CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
END
GO

USE [SiLabI]
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'WebService')
BEGIN
	CREATE USER [WebService] FOR LOGIN [WebServiceLogin] WITH DEFAULT_SCHEMA=[dbo]
	GRANT EXECUTE TO WebService
	EXEC sp_addrolemember 'db_datareader', 'WebService'
END
GO

USE [master]
GO