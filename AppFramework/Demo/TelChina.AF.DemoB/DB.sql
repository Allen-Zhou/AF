
if   NOT exists(select   1   from   master..sysdatabases   where   name='DemoB')
CREATE DATABASE   DemoB
 
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Vehicle]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Vehicle]	 
	create table [dbo].[Vehicle] (
	   ID UNIQUEIDENTIFIER not null,
	   [Name] NVARCHAR(255) null,
	   [Description] NVARCHAR(255) null,
	   CreatedOn DateTime null,
	   UpdatedOn DateTime null,
	   Size Int null,
	   primary key (ID)
	)	       
