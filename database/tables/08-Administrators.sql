IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Administrators_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Administrators]'))
	ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [FK_Administrators_States]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Administrators_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Administrators]'))
	ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [FK_Administrators_Users]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Administrators_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [DF_Administrators_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Administrators_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Administrators] DROP CONSTRAINT [DF_Administrators_Updated_At]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Administrators]') AND type in (N'U'))
	DROP TABLE [dbo].[Administrators]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Administrators](
	[FK_User_Id] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
	
	CONSTRAINT [IX_Administrators_User_Id] UNIQUE NONCLUSTERED 
	(
		[FK_User_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Administrators]  WITH CHECK ADD  CONSTRAINT [FK_Administrators_States] FOREIGN KEY([FK_State_Id])
	REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Administrators] CHECK CONSTRAINT [FK_Administrators_States]
GO

ALTER TABLE [dbo].[Administrators]  WITH CHECK ADD  CONSTRAINT [FK_Administrators_Users] FOREIGN KEY([FK_User_Id])
	REFERENCES [dbo].[Users] ([PK_User_Id])
GO

ALTER TABLE [dbo].[Administrators] CHECK CONSTRAINT [FK_Administrators_Users]
GO

ALTER TABLE [dbo].[Administrators] ADD  CONSTRAINT [DF_Administrators_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Administrators] ADD  CONSTRAINT [DF_Administrators_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO


