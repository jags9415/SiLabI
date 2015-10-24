IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareByLaboratory_Laboratories]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareByLaboratory]'))
	ALTER TABLE [dbo].[SoftwareByLaboratory] DROP CONSTRAINT [FK_SoftwareByLaboratory_Laboratories]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SoftwareByLaboratory_Software]') AND parent_object_id = OBJECT_ID(N'[dbo].[SoftwareByLaboratory]'))
	ALTER TABLE [dbo].[SoftwareByLaboratory] DROP CONSTRAINT [FK_SoftwareByLaboratory_Software]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SoftwareByLaboratory]') AND type in (N'U'))
	DROP TABLE [dbo].[SoftwareByLaboratory]
GO

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
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SoftwareByLaboratory]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareByLaboratory_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
	REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO

ALTER TABLE [dbo].[SoftwareByLaboratory] CHECK CONSTRAINT [FK_SoftwareByLaboratory_Laboratories]
GO

ALTER TABLE [dbo].[SoftwareByLaboratory]  WITH CHECK ADD  CONSTRAINT [FK_SoftwareByLaboratory_Software] FOREIGN KEY([FK_Software_Id])
	REFERENCES [dbo].[Software] ([PK_Software_Id])
GO

ALTER TABLE [dbo].[SoftwareByLaboratory] CHECK CONSTRAINT [FK_SoftwareByLaboratory_Software]
GO


