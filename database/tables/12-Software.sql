IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Software_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Software]'))
	ALTER TABLE [dbo].[Software] DROP CONSTRAINT [FK_Software_States]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Software_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Software] DROP CONSTRAINT [DF_Software_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Software_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Software] DROP CONSTRAINT [DF_Software_Updated_At]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Software]') AND type in (N'U'))
	DROP TABLE [dbo].[Software]
GO

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
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	
	CONSTRAINT [PK_Software] PRIMARY KEY CLUSTERED 
	(
		[PK_Software_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Software]  WITH CHECK ADD  CONSTRAINT [FK_Software_States] FOREIGN KEY([FK_State_Id])
	REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Software] CHECK CONSTRAINT [FK_Software_States]
GO

ALTER TABLE [dbo].[Software] ADD  CONSTRAINT [DF_Software_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Software] ADD  CONSTRAINT [DF_Software_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO


