IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Laboratories_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Laboratories]'))
	ALTER TABLE [dbo].[Laboratories] DROP CONSTRAINT [FK_Laboratories_States]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Laboratories_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Laboratories] DROP CONSTRAINT [DF_Laboratories_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Laboratories_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Laboratories] DROP CONSTRAINT [DF_Laboratories_Updated_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Laboratories_Appointment_Priority]') AND type = 'D')
	ALTER TABLE [dbo].[Laboratories] DROP CONSTRAINT [DF_Laboratories_Appointment_Priority]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Laboratories_Reservation_Priority]') AND type = 'D')
	ALTER TABLE [dbo].[Laboratories] DROP CONSTRAINT [DF_Laboratories_Reservation_Priority]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Laboratories]') AND type in (N'U'))
	DROP TABLE [dbo].[Laboratories]
GO

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
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
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

ALTER TABLE [dbo].[Laboratories]  WITH CHECK ADD  CONSTRAINT [FK_Laboratories_States] FOREIGN KEY([FK_State_Id])
	REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Laboratories] CHECK CONSTRAINT [FK_Laboratories_States]
GO

ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO

ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Appointment_Priority]  DEFAULT ((1)) FOR [Appointment_Priority]
GO

ALTER TABLE [dbo].[Laboratories] ADD  CONSTRAINT [DF_Laboratories_Reservation_Priority]  DEFAULT ((1)) FOR [Reservation_Priority]
GO


