IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudentsByGroup_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudentsByGroup]'))
	ALTER TABLE [dbo].[StudentsByGroup] DROP CONSTRAINT [FK_StudentsByGroup_Groups]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudentsByGroup_Students]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudentsByGroup]'))
	ALTER TABLE [dbo].[StudentsByGroup] DROP CONSTRAINT [FK_StudentsByGroup_Students]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudentsByGroup]') AND type in (N'U'))
	DROP TABLE [dbo].[StudentsByGroup]
GO

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
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentsByGroup]  WITH CHECK ADD  CONSTRAINT [FK_StudentsByGroup_Groups] FOREIGN KEY([FK_Group_Id])
	REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO

ALTER TABLE [dbo].[StudentsByGroup] CHECK CONSTRAINT [FK_StudentsByGroup_Groups]
GO

ALTER TABLE [dbo].[StudentsByGroup]  WITH CHECK ADD  CONSTRAINT [FK_StudentsByGroup_Students] FOREIGN KEY([FK_Student_Id])
	REFERENCES [dbo].[Students] ([FK_User_Id])
GO

ALTER TABLE [dbo].[StudentsByGroup] CHECK CONSTRAINT [FK_StudentsByGroup_Students]
GO


