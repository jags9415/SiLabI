IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateLaboratory]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateLaboratory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update a laboratory data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateLaboratory]
(
	@requester_id			INT,						-- The identity of the requester user.
	@id						INT,						-- The laboratory identity.
	@name					VARCHAR(500),				-- The laboratory name.
	@seats					INT,						-- The available seats.
	@appointment_priority	INT,						-- The appointment priority.
	@reservation_priority	INT,						-- The reservation_priority.
	@state					VARCHAR(30),				-- The laboratory state.
	@software				AS SoftwareList READONLY	-- The list of software codes.
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if laboratory exists.
	IF NOT EXISTS (SELECT 1 FROM Laboratories WHERE PK_Laboratory_Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Laboratorio no encontrado.', 15, 1);
		RETURN -1;
	END
	
	UPDATE Laboratories
	SET [Seats] = ISNULL(@seats, [Seats]),
	[Name] = ISNULL(@name, [Name]),
	[Appointment_Priority] = ISNULL(@appointment_priority, [Appointment_Priority]),
	[Reservation_Priority] = ISNULL(@reservation_priority, [Reservation_Priority]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('LABORATORY', @state), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE [PK_Laboratory_Id] = @id;
	
	IF EXISTS (SELECT 1 FROM @software)
	BEGIN
		EXEC dbo.sp_UpdateLaboratorySoftware @requester_id, @id, @software;
	END
	
	COMMIT TRANSACTION T;
	EXEC dbo.sp_GetLaboratory @requester_id, @id, '*';
END
GO


