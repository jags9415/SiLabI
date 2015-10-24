IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateUser]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateUser]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Update an user data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateUser]
(
	@requester_id   INT,            -- The identity of the requester user.
	@id				INT,			-- The user identity.
	@name			VARCHAR(70),	-- The first name.
	@last_name_1	VARCHAR(70),	-- The first last name.
	@last_name_2	VARCHAR(70),	-- The second last name. [Nullable]
	@gender			VARCHAR(10),	-- The gender.
	@username		VARCHAR(70),	-- The username.
	@password		VARCHAR(70),	-- The password.
	@email			VARCHAR(100),	-- The email. [Nullable]
	@phone			VARCHAR(20),	-- The phone. [Nullable]
	@state			VARCHAR(30)		-- The state. ('active', 'disabled', 'blocked')
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION T;
	
	-- Check if user exists.
	IF NOT EXISTS (SELECT 1 FROM vw_Users WHERE Id = @id)
	BEGIN
		ROLLBACK TRANSACTION T;
		RAISERROR('Usuario no encontrado.', 15, 1);
		RETURN -1;
	END
	ELSE
	BEGIN
		EXEC dbo.sp_UpdateUserData @requester_id, @id, @name, @last_name_1, @last_name_2, @gender, @username, @password, @email, @phone, @state;
		COMMIT TRANSACTION T;
	END
	
	EXEC dbo.sp_GetUser @requester_id, @id, '*';
END
GO


