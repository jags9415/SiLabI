IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetSoftwareByCode]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_GetSoftwareByCode]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: José Andrés García Sáenz
-- Create date: 24/10/2015
-- Description:	Get a software data.
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetSoftwareByCode]
(
	@requester_id	INT,			-- The identity of the requester user.
	@code			VARCHAR(20),	-- The software code.
	@fields			VARCHAR(MAX)	-- The fields to include in the SELECT clause. [Nullable]
)
AS
BEGIN
	DECLARE @where VARCHAR(MAX) = 'code=''' + @code + '''';
	EXEC dbo.sp_GetOne 'vw_Software', @fields, @where;
END
GO


