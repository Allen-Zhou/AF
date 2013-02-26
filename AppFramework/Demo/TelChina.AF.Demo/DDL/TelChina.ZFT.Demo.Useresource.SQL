if not exists (select * from sysobjects where id = object_id(N'[Useresource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
Begin
CREATE TABLE [Useresource](
	[ID] [uniqueidentifier ]  PRIMARY KEY NOT NULL,
	[SysVersion] [Int] NOT NULL,
	[ClassName] [nvarchar] (300) NULL,
	[ColumnName] [nvarchar] (300) NULL,
	[ColumnDescribe] [nvarchar] (300) NULL,
	[IsVisible] [bit] NULL,
) ON [PRIMARY]
End
GO
	
