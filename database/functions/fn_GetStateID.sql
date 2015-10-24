IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetStateID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetStateID]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the identity of a state.
-- =============================================
CREATE FUNCTION [dbo].[fn_GetStateID]
(
	@state_type VARCHAR(30),	-- The object type.
	@state_name VARCHAR(30)		-- The state name.
)
RETURNS INT
AS
BEGIN
	DECLARE @state_id INT;
	
	SELECT @state_id = [PK_State_Id] FROM States
	WHERE [Type] = @state_type AND [Name] = @state_name;
	
	RETURN @state_id
END

GO


