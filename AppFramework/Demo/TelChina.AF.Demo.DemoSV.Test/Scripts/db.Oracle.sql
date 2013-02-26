
    drop table Category cascade constraints;

    drop table Department cascade constraints;

    drop table LogClass cascade constraints;

    drop table Person cascade constraints;

    drop table Product cascade constraints;

    create table Category (
       ID RAW(16) primary key,
       Name NVARCHAR2(255),
       Description NVARCHAR2(255),
       CreatedOn TIMESTAMP(4),
       UpdatedOn TIMESTAMP(4),
       Size NUMBER(10,0)
       
    );

    create table Department (
        ID RAW(16)  primary key (ID),
       SysVersion NUMBER(10,0) not null,
       CreatedOn TIMESTAMP(4),
       CreatedBy NVARCHAR2(16),
       UpdatedOn TIMESTAMP(4),
       UpdatedBy NVARCHAR2(16),
       Code NVARCHAR2(16) not null,
       Name NVARCHAR2(16) not null,
       Disabled NUMBER(1,0),
       IsEndNode NUMBER(1,0),
       Depth NUMBER(10,0),
       InId NVARCHAR2(50),
       idParent RAW(16)
      
    );

    create table LogClass (
        ID RAW(16) primary key (ID),
       SysVersion NUMBER(10,0) not null,
       CreatedOn TIMESTAMP(4),
       CreatedBy NVARCHAR2(16),
       UpdatedOn TIMESTAMP(4),
       UpdatedBy NVARCHAR2(16),
       ByteImage RAW(50),
       Operating NVARCHAR2(16) not null
      
    );

    create table Person (
        ID RAW(16)       primary key (ID),
       SysVersion NUMBER(10,0) not null,
       CreatedOn TIMESTAMP(4),
       CreatedBy NVARCHAR2(16),
       UpdatedOn TIMESTAMP(4),
       UpdatedBy NVARCHAR2(16),
       Code NVARCHAR2(16) not null,
       Name NVARCHAR2(16) not null,
       Telphone NVARCHAR2(16),
       Address NVARCHAR2(50),
       PostCode NVARCHAR2(50),
       Gender NUMBER(1,0),
       Disabled NUMBER(1,0),
       NativePlace NVARCHAR2(50)
    );

    create table Product (
        ID RAW(16) primary key,
       Code NVARCHAR2(16) not null,
       Name NVARCHAR2(16) not null,
       "SIZE" NUMBER(10,0),
       CategoryID RAW(16)
       
    );

    alter table Department 
        add constraint FK6BAC779684D65026 
        foreign key (idParent) 
        references Department;

    alter table Product 
        add constraint FK1F94D86A773C8DFE 
        foreign key (CategoryID) 
        references Category;
