IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Courses_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Courses]'))
	ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Courses_States]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Courses_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [DF_Courses_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Courses_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [DF_Courses_Updated_At]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND type in (N'U'))
	DROP TABLE [dbo].[Courses]
GO

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
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	
	CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
	(
		[PK_Course_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
	 
	CONSTRAINT [IX_Courses_Code] UNIQUE NONCLUSTERED 
	(
		[PK_Course_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_States] FOREIGN KEY([FK_State_Id])
	REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_States]
GO

ALTER TABLE [dbo].[Courses] ADD CONSTRAINT [DF_Courses_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Courses] ADD CONSTRAINT [DF_Courses_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO


