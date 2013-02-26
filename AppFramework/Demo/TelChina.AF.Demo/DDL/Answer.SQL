if not exists (select * from sysobjects where id = object_id(N'[Answer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
Begin
CREATE TABLE [Answer](
[ID] [uniqueidentifier ]  PRIMARY KEY NOT NULL,
[SysVersion] [Int] NOT NULL,
[CreatedOn] [DateTime] NULL,
[CreatedBy] [nvarchar] (Max) NULL,
[UpdatedOn] [DateTime] NULL,
[UpdatedBy] [nvarchar] (Max) NULL,
	[Name] [nvarchar] (Max) NULL,
	[Question_ID] [uniqueidentifier ] NULL,
) ON [PRIMARY]
End
GO
	
