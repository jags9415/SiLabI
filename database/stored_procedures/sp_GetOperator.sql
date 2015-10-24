IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetOperator]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetOperator]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get an operator data.
-- =============================================

CREATE PROCEDURE [dbo].[sp_GetOperator]
(
	@requester_id INT,		-- The identity of the requester user.
	@operator_id INT,		-- The Operator Id.
	@fields VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'id=' + CAST(@operator_id AS VARCHAR);
	EXEC dbo.sp_GetOne 'vw_Operators', @fields, @where;
END
GO


