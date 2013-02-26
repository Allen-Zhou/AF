declare v_cnt Number; 
begin
	select count(*) into v_cnt from user_tables where table_name = 'AF_USERESOURCE'; 
	if v_cnt>0 then 
		execute immediate 'Drop Table AF_USERESOURCE';    
	end If; 
End; 
/
CREATE TABLE AF_USERESOURCE(
ID RAW(16)  primary key  NOT NULL,
SysVersion NUMBER(10,0) NOT NULL,
ResourceType NVARCHAR2(300) ,
ResourceCode NVARCHAR2(300) ,
ResourceName NVARCHAR2(300) ,
IsVisible NUMBER(1,0),
OrderNo  NVARCHAR2(300)
);
	
