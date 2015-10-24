IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'UserList' AND ss.name = N'dbo')
	DROP TYPE [dbo].[UserList]
GO

CREATE TYPE [dbo].[UserList] AS TABLE([Username] [varchar](70) NULL)
GO


