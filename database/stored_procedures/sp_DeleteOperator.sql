IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteOperator]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_DeleteOperator]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Set an operator as inactive.
-- =============================================
CREATE PROCEDURE [dbo].[sp_DeleteOperator]
(
	@requester_id	INT,	-- The identity of the requester user.
	@operator_id	INT		-- The operator identity.
)
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Operators
	SET FK_State_Id = dbo.fn_GetStateID('OPERATOR', 'Inactivo'),
		Updated_At = SYSDATETIMEOFFSET()
	WHERE
		PK_Operator_Id = @operator_id;
END
GO


