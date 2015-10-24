IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Groups_Courses]') AND parent_object_id = OBJECT_ID(N'[dbo].[Groups]'))
	ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_Courses]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Groups_Periods]') AND parent_object_id = OBJECT_ID(N'[dbo].[Groups]'))
	ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_Periods]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Groups_Professors]') AND parent_object_id = OBJECT_ID(N'[dbo].[Groups]'))
	ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_Professors]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Groups_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Groups]'))
	ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [FK_Groups_States]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Groups_Period_Year]') AND type = 'D')
	ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [DF_Groups_Period_Year]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Groups_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [DF_Groups_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Groups_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Groups] DROP CONSTRAINT [DF_Groups_Updated_At]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Groups]') AND type in (N'U'))
	DROP TABLE [dbo].[Groups]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Groups](
	[PK_Group_Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[Period_Year] [int] NOT NULL,
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
	[FK_Course_Id] [int] NOT NULL,
	[FK_Professor_Id] [int] NOT NULL,
	[FK_Period_Id] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	
	CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
	(
		[PK_Group_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
	
	CONSTRAINT [IX_Groups] UNIQUE NONCLUSTERED 
	(
		[Number] ASC,
		[FK_Professor_Id] ASC,
		[FK_Course_Id] ASC,
		[FK_Period_Id] ASC,
		[Period_Year] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Courses] FOREIGN KEY([FK_Course_Id])
	REFERENCES [dbo].[Courses] ([PK_Course_Id])
GO

ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Courses]
GO

ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Periods] FOREIGN KEY([FK_Period_Id])
	REFERENCES [dbo].[Periods] ([PK_Period_Id])
GO

ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Periods]
GO

ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Professors] FOREIGN KEY([FK_Professor_Id])
	REFERENCES [dbo].[Professors] ([FK_User_Id])
GO

ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Professors]
GO

ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_States] FOREIGN KEY([FK_State_Id])
	REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_States]
GO

ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Period_Year]  DEFAULT (datepart(year,sysdatetimeoffset())) FOR [Period_Year]
GO

ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO


