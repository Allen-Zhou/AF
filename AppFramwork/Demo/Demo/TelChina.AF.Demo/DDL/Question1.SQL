if not exists (select * from sysobjects where id = object_id(N'[Question1]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
Begin
CREATE TABLE [Question1](
[ID] [uniqueidentifier ]  PRIMARY KEY NOT NULL,
[SysVersion] [Int] NOT NULL,
[CreatedOn] [DateTime] NULL,
[CreatedBy] [nvarchar] (300) NULL,
[UpdatedOn] [DateTime] NULL,
[UpdatedBy] [nvarchar] (300) NULL,
	[Name] [nvarchar] (300) NULL,
) ON [PRIMARY]
End
GO
	
