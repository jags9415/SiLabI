IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAdministrator]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetAdministrator]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get an administrator data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAdministrator]
(
	@requester_id	INT,			-- The identity of the requester user.
	@user_id		INT,			-- The user identity.
	@fields			VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@user_id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Administrators', @fields, @where;
END
GO