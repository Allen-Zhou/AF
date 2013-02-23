if not exists (select * from sysobjects where id = object_id(N'Question') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
Begin
CREATE TABLE Question(
ID uniqueidentifier   PRIMARY KEY NOT NULL,
SysVersion Int NOT NULL,
CreatedOn DateTime NULL,
CreatedBy nvarchar (Max) NULL,
UpdatedOn DateTime NULL,
UpdatedBy nvarchar (Max) NULL,
	Name nvarchar (Max) NULL	
) ON [PRIMARY]
End
GO
	
