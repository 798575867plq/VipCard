﻿use master
go

if(DB_ID('MvcOne') is not null) drop database MvcOne
go

create database MvcOne
go

use MvcOne
go


/*会员卡信息*/
create table TblVipCard
(
	vcid int identity primary key not null,
	username nvarchar(50) default '' not null,
	phone nvarchar(20) unique,
	cardno varchar(50) not null,
	/*是否启用*/
	isenable char(1) default 'y' not null,
	/*余额*/
	balance decimal(8,2) check(balance>=0) not null,
	/*开卡的时间*/
	createDate datetime default getdate() not null
)
go

select * from TblVipCard
go