/*****************************************************************************
*																			*
* SQL - CreateDB-Script for TaskForce Version 1.0								*
*																			*
* Version 1.0: First Version,					11/2013 - 11/2013			*
*	Dominique Schapal														*
*																			*
*****************************************************************************/

set nocount on
use [master]
go

/********************************************
				Create Database
********************************************/

create database TaskForce

/********************************************
		Create Logins
********************************************/

create login [TFUser] with password = 'TFUser123', default_database= TaskForce , check_expiration=off, check_policy=off

exec sp_addsrvrolemember @loginame = [TFUser] , @rolename = N'sysadmin'

/********************************************
		Create Tables
********************************************/

use [TaskForce]
go

create table Account
(
	ID			int			identity(0,1) not null,
	LoginName	varchar(25)	not null,
	LastLogin	datetime	not null,

	constraint	PKAccount primary key (ID),
	constraint	ULoginName unique (ID)
)
go

insert into Account (LoginName, LastLogin) values ('Administrator', GETDATE())

create table Filter
(
	ID			int			identity(1,1) not null,
	Name		varchar(25)	not null,
	[Type]		int			not null,
	ProcessName	varchar(25) not null,
	Account		int			null,
	FilterGroup	int			null,

	constraint	PKFilter primary key (ID)
)
go

create table FilterStatus
(
	Account		int			not null,
	Filter		int			not null,

	constraint	PKFilterStatus primary key (Account, Filter)
)
go

create table FilterGroup
(
	ID			int			identity(1,1) not null,
	Name		varchar(25)	not null,
	Account		int			not null,

	constraint	PKFilterGroup primary key (ID)
)
go

create table Protocol
(
	ID			int			identity(1,1) not null,
	ClientIP	varchar(15)	not null,
	Created		datetime	not null,
	Account		int			not null,
	ProcessName varchar(25) not null,
	FilterType	int			not null,

	constraint	PKProtocol primary key (ID)
)
go

/********************************************
		Create Procedures
********************************************/

create procedure GetAccount
	@LoginName		varchar(25)
as
	select 
		ID,
		LoginName,
		LastLogin
	from Account
	where LoginName = @LoginName
go
grant execute on GetAccount to public
go

create procedure InsAccount
	@LoginName		varchar(25)
as
	if(not exists(select * from Account where LoginName = @LoginName))
	begin
		insert into Account(
			LoginName,
			LastLogin)
		values(
			@LoginName,
			GETDATE())

		insert into FilterStatus(
			Account,
			Filter)
		select
			SCOPE_IDENTITY(),
			f.ID
		from Filter f
		where Account = 0
	end
	else
		raiserror('LoginName already exists', 21, -1)
go
grant execute on InsAccount to public
go

create procedure DelAccount
	@LoginName		varchar(25)
as
	declare @ID int

	select @ID = ID
	from Account
	where
		LoginName = @LoginName

	delete
	from Account
	where
		ID = @ID

	delete
	from Filter
	where
		Account = @ID

	delete
	from FilterGroup
	where
		Account = @ID
	
	delete
	from FilterStatus
	where
		Account = @ID

	delete
	from Protocol
	where
		Account = @ID	
go
grant execute on DelAccount to public
go

create procedure InsFilter
	@Account		int,
	@Name			varchar(25),
	@FilterType		int,
	@ProcessName	varchar(25),
	@FilterGroup	int = null
as
	insert into Filter(
		Account,
		[Type],
		FilterGroup,
		Name,
		ProcessName)
	values(
		@Account,
		@FilterType,
		@FilterGroup,
		@Name,
		@ProcessName)

	if(@Account != 0)
		insert into FilterStatus(
			Account,
			Filter)
		values(
		@Account,
		SCOPE_IDENTITY())
go
grant execute on InsFilter to public
go

create procedure DelFilter
	@Filter		int
as
	delete
	from Filter
	where
		ID = @Filter

	delete
	from FilterStatus
	where
		Filter = @Filter	
go
grant execute on DelFilter to public
go

create procedure UpdFilterStatus
	@Account		int,
	@Filter			int,
	@Active			bit
as
	if(@Active = 1)
	begin
		if(not EXISTS(select * from FilterStatus where Account = @Account and Filter = @Filter))
		begin
			insert into FilterStatus(
				Account,
				Filter)
			values(
				@Account,
				@Filter)
		end
	end
	else
		delete from FilterStatus
		where 
			Account = @Account and
			Filter = @Filter
go
grant execute on UpdFilterStatus to public
go

create procedure LstForbiddenFilter
	@Account		int
as
	declare @Active bit

	select
		ID,
		[Type],
		ProcessName,
		Name,
		Account,
		FilterGroup,
		Active = isNull((select 1 from FilterStatus s where s.Account = @Account and s.Filter = f.ID), 0)
	from Filter f
	where
		(Account = @Account or Account = 0) and
		[Type] = 2
go
grant execute on LstForbiddenFilter to public
go

create procedure LstProtectedFilter
	@Account		int
as
	select
		ID,
		[Type],
		ProcessName,
		Name,
		Account,
		FilterGroup,
		Active = isNull((select 1 from FilterStatus s where s.Account = @Account and s.Filter = f.ID), 0)
	from Filter f
	where
		(Account = @Account or Account = 0) and
		[Type] = 1
go
grant execute on LstProtectedFilter to public
go

create procedure InsFilterGroup
	@Account		int,
	@Name			varchar(25)
as
	insert into FilterGroup(
		Account,
		Name)
	values (
		@Account,
		@Name)
go
grant execute on InsFilterGroup to public
go

create procedure DelFilterGroup
	@FilterGroup	int
as
	declare @Filter	int

	delete
	from FilterGroup
	where
		ID = @FilterGroup

	update Filter
	set FilterGroup = null
	where FilterGroup = @FilterGroup
	
go
grant execute on DelFilterGroup to public
go

create procedure LstFilterGroup
	@Account		int
as
	select
		ID,
		Name,
		Account
	from FilterGroup
	where
		Account = @Account and Account = 0
go
grant execute on LstFilterGroup to public
go

create procedure InsProtocol
	@Account		int,
	@Filter			int,
	@ClientIP		varchar(15)
as
	declare
		@ProcessName varchar(25),
		@FilterType	 int

	select
		@ProcessName = ProcessName,
		@FilterType = [Type]
	from Filter
	where
		ID = @Filter

	if(@ProcessName = null)
		raiserror('Filter doesn''t exist', 21, -1)

	insert into Protocol(
		Account,
		Created,
		ProcessName,
		ClientIP,
		FilterType)
	values(
		@Account,
		GETDATE(),
		@ProcessName,
		@ClientIP,
		@FilterType)
go
grant execute on InsProtocol to public
go

create procedure LstProtocol
	@Account		int,
	@From			datetime = null,
	@To				datetime = null
as
	select
		ID,
		ClientIP,
		Created,
		Account,
		ProcessName,
		FilterType
	from Protocol
	where
		Account = @Account and
		(Created >= @From or @From is null) and
		(Created <= @To or @To is null)
go

grant execute on LstProtocol to public
go