IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Professors_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Professors]'))
	ALTER TABLE [dbo].[Professors] DROP CONSTRAINT [FK_Professors_Users]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Professors]') AND type in (N'U'))
	DROP TABLE [dbo].[Professors]
GO

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

ALTER TABLE [dbo].[Professors]  WITH CHECK ADD  CONSTRAINT [FK_Professors_Users] FOREIGN KEY([FK_User_Id])
	REFERENCES [dbo].[Users] ([PK_User_Id])
GO

ALTER TABLE [dbo].[Professors] CHECK CONSTRAINT [FK_Professors_Users]
GO


