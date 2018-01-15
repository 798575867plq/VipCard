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

insert into TbVipCardRecord(vcid,rtype,amount,info) values(1,1,10000,'内置金卡默认充值')
go

select * from TbVipCardRecord
go

select vc.cardno,vc.username,vc.phone,vc.info,
case vc.sex when 'm' then '男' when 'f' then '女' else '保密' end 'sex',
(select SUM(amount*rtype) from TbVipCardRecord where vcid=vc.vcid) 'balance',
CONVERT(varchar,createdDate,120) 'createdDate'
from TbVipCard vc
go

create table TbGoods
(
	gid int identity primary key not null,
	gname nvarchar(50) unique not null,
	price decimal(10,2) check(price>=0) not null,
	amount int check(amount>=0) not null
)
go

insert into TbGoods(gname,price,amount) values('可口可乐',3,2000)
insert into TbGoods(gname,price,amount) values('82拉菲',1200,100)
insert into TbGoods(gname,price,amount) values('老白干',200,1234)
go

select * from TbGoods
go

--select @@IDENTITY

create table TbBuyGoods
(
	bgid int identity primary key not null,
	gid int foreign key references TbGoods(gid) not null,
	amount int check(amount>=0) not null,
	vcrid int foreign key references TbVipCardRecord(vcrid) not null,
	btime datetime default getdate() not null
)
go

insert into TbVipCardRecord(vcid,rtype,amount,info) values(1,-1,2400,'商品购买')
insert into TbBuyGoods(gid,amount,vcrid) values(2,2,2)

select bg.bgid,bg.gid,bg.amount,bg.vcrid,
 CONVERT(varchar,bg.btime,120) 'btime',
 g.gname,g.price,vcr.amount 'vamount',vc.cardno,vc.username
 from TbBuyGoods bg
inner join TbGoods g on bg.gid=g.gid
inner join TbVipCardRecord vcr on bg.vcrid=vcr.vcrid
inner join TbVipCard vc on vcr.vcid=vc.vcid
go

select * from TbBuyGoods
go