IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'SoftwareList' AND ss.name = N'dbo')
	DROP TYPE [dbo].[SoftwareList]
GO

CREATE TYPE [dbo].[SoftwareList] AS TABLE([Code] [varchar](20) NULL)
GO