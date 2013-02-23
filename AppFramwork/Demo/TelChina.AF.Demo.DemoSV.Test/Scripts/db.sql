
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK6742384A84D65026]') AND parent_object_id = OBJECT_ID('[Department]'))

alter table [Department]  drop constraint FK6742384A84D65026




    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK3697A4D365E5C362]') AND parent_object_id = OBJECT_ID('[dbo].[Product]'))

alter table [dbo].[Product]  drop constraint FK3697A4D365E5C362




    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Category]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Category]


    if exists (select * from dbo.sysobjects where id = object_id(N'[Department]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Department]


    if exists (select * from dbo.sysobjects where id = object_id(N'[LogClass]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [LogClass]


    if exists (select * from dbo.sysobjects where id = object_id(N'[Person]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Person]


    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Product]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Product]


    create table [dbo].[Category] (
        ID UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) null,
       Description NVARCHAR(255) null,
       CreatedOn DATETIME null,
       UpdatedOn DATETIME null,
       Size INT null,
       primary key (ID)
    )


    create table [Department] (
        ID UNIQUEIDENTIFIER not null,
       SysVersion INT not null,
       CreatedOn DATETIME null,
       CreatedBy NVARCHAR(16) null,
       UpdatedOn DATETIME null,
       UpdatedBy NVARCHAR(16) null,
       Code NVARCHAR(16) not null,
       Name NVARCHAR(16) not null,
       Disabled BIT null,
       IsEndNode BIT null,
       Depth INT null,
       InId NVARCHAR(50) null,
       idParent UNIQUEIDENTIFIER null,
       primary key (ID)
    )


    create table [LogClass] (
        ID UNIQUEIDENTIFIER not null,
       SysVersion INT not null,
       CreatedOn DATETIME null,
       CreatedBy NVARCHAR(16) null,
       UpdatedOn DATETIME null,
       UpdatedBy NVARCHAR(16) null,
       ByteImage VARBINARY(50) null,
       Operating NVARCHAR(16) not null,
       primary key (ID)
    )


    create table [Person] (
        ID UNIQUEIDENTIFIER not null,
       SysVersion INT not null,
       CreatedOn DATETIME null,
       CreatedBy NVARCHAR(16) null,
       UpdatedOn DATETIME null,
       UpdatedBy NVARCHAR(16) null,
       Code NVARCHAR(16) not null,
       Name NVARCHAR(16) not null,
       Telphone NVARCHAR(16) null,
       Address NVARCHAR(50) null,
       PostCode NVARCHAR(50) null,
       Gender BIT null,
       Disabled BIT null,
       NativePlace NVARCHAR(50) null,
       primary key (ID)
    )


    create table [dbo].[Product] (
        ID UNIQUEIDENTIFIER not null,
       Code NVARCHAR(16) not null,
       Name NVARCHAR(16) not null,
       Size INT null,
       [CategoryID] UNIQUEIDENTIFIER null,
       primary key (ID)
    )


    alter table [Department] 
        add constraint FK6742384A84D65026 
        foreign key (idParent) 
        references [Department]


    alter table [dbo].[Product] 
        add constraint FK3697A4D365E5C362 
        foreign key ([CategoryID]) 
        references [dbo].[Category]

