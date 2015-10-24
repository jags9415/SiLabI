IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reservations_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservations]'))
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_Groups]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reservations_Laboratories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservations]'))
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_Laboratories]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reservations_Professors]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservations]'))
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_Professors]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reservations_Software]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservations]'))
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_Software]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reservations_States]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservations]'))
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_Reservations_States]
GO

IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Reservations_Time]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservations]'))
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [CK_Reservations_Time]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Reservations_Attendance]') AND type = 'D')
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [DF_Reservations_Attendance]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Reservations_Created_At]') AND type = 'D')
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [DF_Reservations_Created_At]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Reservations_Updated_At]') AND type = 'D')
	ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [DF_Reservations_Updated_At]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reservations]') AND type in (N'U'))
	DROP TABLE [dbo].[Reservations]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Reservations](
	[PK_Reservation_Id] [int] IDENTITY(1,1) NOT NULL,
	[Start_Time] [datetimeoffset](3) NOT NULL,
	[End_Time] [datetimeoffset](3) NOT NULL,
	[Attendance] [bit] NOT NULL,
	[Created_At] [datetimeoffset](3) NOT NULL,
	[Updated_At] [datetimeoffset](3) NOT NULL,
	[FK_Professor_Id] [int] NOT NULL,
	[FK_Group_Id] [int] NULL,
	[FK_Laboratory_Id] [int] NOT NULL,
	[FK_Software_Id] [int] NULL,
	[FK_State_Id] [int] NOT NULL,
	
	CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED 
	(
		[PK_Reservation_Id] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Groups] FOREIGN KEY([FK_Group_Id])
	REFERENCES [dbo].[Groups] ([PK_Group_Id])
GO

ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Groups]
GO

ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Laboratories] FOREIGN KEY([FK_Laboratory_Id])
	REFERENCES [dbo].[Laboratories] ([PK_Laboratory_Id])
GO

ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Laboratories]
GO

ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Professors] FOREIGN KEY([FK_Professor_Id])
	REFERENCES [dbo].[Professors] ([FK_User_Id])
GO

ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Professors]
GO

ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Software] FOREIGN KEY([FK_Software_Id])
	REFERENCES [dbo].[Software] ([PK_Software_Id])
GO

ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Software]
GO

ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_States] FOREIGN KEY([FK_State_Id])
REFERENCES [dbo].[States] ([PK_State_Id])
GO

ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_States]
GO

ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [CK_Reservations_Time] CHECK  (([End_Time]>[Start_Time]))
GO

ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [CK_Reservations_Time]
GO

ALTER TABLE [dbo].[Reservations] ADD  CONSTRAINT [DF_Reservations_Attendance]  DEFAULT ((0)) FOR [Attendance]
GO

ALTER TABLE [dbo].[Reservations] ADD  CONSTRAINT [DF_Reservations_Created_At]  DEFAULT (sysdatetimeoffset()) FOR [Created_At]
GO

ALTER TABLE [dbo].[Reservations] ADD  CONSTRAINT [DF_Reservations_Updated_At]  DEFAULT (sysdatetimeoffset()) FOR [Updated_At]
GO


