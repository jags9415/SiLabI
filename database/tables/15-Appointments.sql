IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [FK_Appointments_Groups]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Laboratories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [FK_Appointments_Laboratories]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Software]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [FK_Appointments_Software]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [FK_Appointments_States]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointments_Students]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointments]'))
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [FK_Appointments_Students]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Appointments_Attendance]') AND type = 'D')
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [DF_Appointments_Attendance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Appointments_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [DF_Appointments_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Appointments_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Appointments] DROP CONSTRAINT [DF_Appointments_Updated_At]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Appointments]') AND type in (N'U'))
	DROP TABLE [dbo].[Appointments]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Appointments](
	[PK_Appointment_Id] [int] IDENTITY(1,1) NOT NULL,
	[Attendance] [bit] NOT NULL,
	[Date] [datetimeoffset](3) NOT NULL,
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
	[FK_Student_Id] [int] NOT NULL,
	[FK_Laboratory_Id] [int] NOT NULL,
	[FK_Software_Id] [int] NOT NULL,
	[FK_State_Id] [int] NOT NULL,
	[FK_Group_Id] [int] NOT NULL,
	
	CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
	(
		[PK_Appointment_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Groups] FOREIGN KEY([FK_Group_Id])
	REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO

ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Groups]
GO

ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
	REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO

ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Laboratories]
GO

ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Software] FOREIGN KEY([FK_Software_Id])
	REFERENCES [dbo].[Software] ([PK_Software_Id])
GO

ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Software]
GO

ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_States] FOREIGN KEY([FK_State_Id])
	REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_States]
GO

ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointments_Students] FOREIGN KEY([FK_Student_Id])
	REFERENCES [dbo].[Students] ([FK_User_Id])
GO

ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Students]
GO

ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Attendance]  DEFAULT ((0)) FOR [Attendance]
GO

ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO


