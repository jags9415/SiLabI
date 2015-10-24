IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateUserData]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_UpdateUserData]
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
CREATE PROCEDURE [dbo].[sp_UpdateUserData]
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
	
	IF EXISTS (SELECT 1 FROM Users WHERE Username = @username AND PK_User_Id <> @id)
	BEGIN
		RAISERROR('Ya existe un usuario con identificado como %s.', 15, 1, @username);
		RETURN -1;
	END

	UPDATE Users
	SET[Name] = ISNULL(@name, [Name]),
	[Last_Name_1] = ISNULL(@last_name_1, [Last_Name_1]),
	[Last_Name_2] =  CAST(dbo.fn_IsNull(@last_name_2, [Last_Name_2]) AS VARCHAR(70)),
	[Username] = ISNULL(@username, [Username]),
	[Password] = ISNULL(@password, [Password]),
	[Email] = CAST(dbo.fn_IsNull(@email, [Email]) AS VARCHAR(100)),
	[Phone] =  CAST(dbo.fn_IsNull(@phone, [Phone]) AS VARCHAR(20)),
	[Gender] = ISNULL(@gender, [Gender]),
	[FK_State_Id] = ISNULL(dbo.fn_GetStateID('USER', @state), [FK_State_Id]),
	[Updated_At] = SYSDATETIMEOFFSET()
	WHERE PK_User_Id = @id;
END
GO


