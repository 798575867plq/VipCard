use master
go

if(DB_ID('VipCard') is not null) drop database VipCard
go

create database VipCard
go

use VipCard
go

/*登录用户信息*/
create table TbUser
(
	uid int identity primary key not null,
	username nvarchar(50) unique not null,
	password nvarchar(50) not null,
	nickname nvarchar(50) not null
)
go
insert into TbUser(username,password,nickname) values('admin','123456','内置管理员')
go

select * from TbUser
go

/*会员卡信息*/
create table TbVipCard
(
	vcid int identity primary key not null,
	cardno varchar(50) unique not null,
	username nvarchar(50) not null,
	password nvarchar(50) not null,
	sex char(1) check(sex in ('m','f')) default 'm' not null,
	phone varchar(50) unique not null,
	info nvarchar(500) default '' not null,
	createdDate datetime default getdate() not null
)
go

insert into TbVipCard(cardno,username,password,phone,info) values('888888-888888-888888-888888-888888','内置金卡','888888','18888888888','内置金卡会员')

select * from TbVipCard
go

/*会员交易信息*/
create table TbVipCardRecord
(
	vcrid int identity primary key not null,
	vcid int foreign key references TbVipCard(vcid)  not null,
	rtype int check(rtype in (-1,0,1)) not null, --消费-1，充值1，其它0
	amount decimal(10,2) check(amount>=0) default 0 not null,
	info  nvarchar(500) default '' not null,
	rtime datetime default getdate() not null
)
go

select * from TbVipCardRecord
go

select vc.cardno,vc.username,vc.phone,
case vc.sex when 'm' then '男' when 'f' then '女' else '保密' end 'sex',
(select SUM(amount*rtype) from TbVipCardRecord where vcid=vc.vcid) 'balance',
CONVERT(varchar,createdDate,120) 'createdDate'
from TbVipCard vc
go

