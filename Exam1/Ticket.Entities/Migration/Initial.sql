create Database exam1
go

use exam1
go

create table availableTicket(
	eventDate datetime not null,
	quota int not null,
	ticketCode varchar(5) constraint PK_availTicket primary key,
	ticketName varchar(255) not null,
	categoryName varchar(255) not null,
	seat varchar(4) not null,
	price int not null
)

create table bookedTicket(
	bookedId int constraint PK_bookedTicket primary key identity,
	eventDate datetime not null,
	quota int not null,
	ticketCode varchar(5) constraint FK_bookedTicket_availableTicket 
	foreign key references availableTicket,
	ticketName varchar(255) not null,
	categoryName varchar(255) not null,
	seat varchar(4) not null,
	price int not null
)
