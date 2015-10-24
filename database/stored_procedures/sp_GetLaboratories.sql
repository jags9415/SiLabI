IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetLaboratories]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetLaboratories]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get the list of laboratories that match a query.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLaboratories]
(
	@requester_id	INT,			-- The identity of the requester user.
	@fields			VARCHAR(MAX),	-- The fields to include in the SELECT clause. [Nullable]
	@order_by		VARCHAR(MAX),	-- The fields to include in the ORDER BY clause. [Nullable]
	@where			VARCHAR(MAX),	-- The fields to include in the WHERE clause. [Nullable]
	@page			INT,			-- The page number. [Nullable]
	@limit			INT				-- The rows per page. [Nullable]
)
AS
BEGIN
	EXEC dbo.sp_GetAll 'vw_Laboratories', @fields, @order_by, @where, @page, @limit;
END
GO


