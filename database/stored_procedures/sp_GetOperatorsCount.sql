IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetOperatorsCount]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetOperatorsCount]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Count the number of operators that match a query.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetOperatorsCount]
(
	@requester_id	INT,			-- The identity of the requester user.
	@where			VARCHAR(MAX)	-- The fields to include in the WHERE clause. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetCount 'vw_Operators', @where;
END
GO


