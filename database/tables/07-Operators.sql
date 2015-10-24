IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Operators_Periods]') AND parent_object_id = OBJECT_ID(N'[dbo].[Operators]'))
	ALTER TABLE [dbo].[Operators] DROP CONSTRAINT [FK_Operators_Periods]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Operators_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Operators]'))
	ALTER TABLE [dbo].[Operators] DROP CONSTRAINT [FK_Operators_States]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Operators_Students]') AND parent_object_id = OBJECT_ID(N'[dbo].[Operators]'))
	ALTER TABLE [dbo].[Operators] DROP CONSTRAINT [FK_Operators_Students]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Operators_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Operators] DROP CONSTRAINT [DF_Operators_Updated_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Operators_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Operators] DROP CONSTRAINT [DF_Operators_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Operators_Period_Year]') AND type = 'D')
	ALTER TABLE [dbo].[Operators] DROP CONSTRAINT [DF_Operators_Period_Year]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Operators]') AND type in (N'U'))
	DROP TABLE [dbo].[Operators]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Operators](
	[PK_Operator_Id] [int] IDENTITY(1,1) NOT NULL,
	[FK_User_Id] [int] NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Period_Year] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	[FK_Period_Id] [int] NOT NULL,
	
	CONSTRAINT [PK_Operators] PRIMARY KEY CLUSTERED 
	(
		[PK_Operator_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
	
	CONSTRAINT [IX_Operators] UNIQUE NONCLUSTERED 
	(
		[FK_User_Id] ASC,
		[FK_Period_Id] ASC,
		[Period_Year] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_Periods] FOREIGN KEY([FK_Period_Id])
	REFERENCES [dbo].[Periods] ([PK_Period_Id])
GO

ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_Periods]
GO

ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_States] FOREIGN KEY([FK_State_Id])
	REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_States]
GO

ALTER TABLE [dbo].[Operators]  WITH CHECK ADD  CONSTRAINT [FK_Operators_Students] FOREIGN KEY([FK_User_Id])
	REFERENCES [dbo].[Students] ([FK_User_Id])
GO

ALTER TABLE [dbo].[Operators] CHECK CONSTRAINT [FK_Operators_Students]
GO

ALTER TABLE [dbo].[Operators] ADD  CONSTRAINT [DF_Operators_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO

ALTER TABLE [dbo].[Operators] ADD  CONSTRAINT [DF_Operators_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Operators] ADD  CONSTRAINT [DF_Operators_Period_Year]  DEFAULT (datepart(year,sysdatetimeoffset())) FOR [Period_Year]
GO


