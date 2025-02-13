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
	bookedId int constraint PK_bookedTicket primary key Identity,
	eventDate datetime not null,
	quota int not null,
	ticketCode varchar(5) constraint FK_bookedTicket_availableTicket 
	foreign key references availableTicket,
	ticketName varchar(255) not null,
	categoryName varchar(255) not null,
	seat varchar(4) not null,
	price int not null
)

INSERT INTO availableTicket (eventDate, quota, ticketCode, ticketName, categoryName, seat, price) VALUES
('2025-02-17 10:00:00.000', 30, 'C003', 'Spider-Man 3', 'Cinema', 'K05', 55000),
('2025-02-18 12:45:00.000', 40, 'TD003', 'Bus Jakarta-Surabaya', 'Transportasi Darat', 'N011', 175000),
('2025-02-19 09:15:00.000', 20, 'TU002', 'Pesawat Bali-Makassar', 'Transportasi Udara', 'J20', 600000),
('2025-02-20 18:30:00.000', 15, 'C004', 'The Batman', 'Cinema', 'G07', 48000),
('2025-02-21 14:20:00.000', 35, 'TD004', 'Bus Bandung-Yogyakarta', 'Transportasi Darat', 'P015', 160000);


INSERT INTO bookedTicket (eventDate, quota, ticketCode, ticketName, categoryName, seat, price) VALUES
('2025-02-11 14:58:56.263', 1, 'TU001', 'Pesawat Jakarta-Bangka', 'Transportasi Udara', 'K22', 500000),
('2025-02-11 15:20:03.997', 5, 'C002', 'Avengers 10', 'Cinema', 'M12', 50000),
('2025-02-11 22:07:49.190', 4, 'C001', 'Fantastic 4', 'Cinema', 'M17', 45000),
('2025-02-12 11:20:20.823', 2, 'TU001', 'Pesawat Jakarta-Bangka', 'Transportasi Udara', 'L09', 500000),
('2025-02-13 09:45:33.512', 3, 'TD001', 'Bus Jawa-Kalimantan', 'Transportasi Darat', 'M002', 150000),
('2025-02-14 17:10:45.821', 1, 'TD002', 'Bus Jawa-Bali', 'Transportasi Darat', 'M012', 125000),
('2025-02-15 20:05:55.600', 2, 'C003', 'Spider-Man 3', 'Cinema', 'K05', 55000),
('2025-02-16 13:25:48.732', 4, 'TD003', 'Bus Jakarta-Surabaya', 'Transportasi Darat', 'N011', 175000),
('2025-02-17 11:30:15.900', 3, 'TU002', 'Pesawat Bali-Makassar', 'Transportasi Udara', 'J20', 600000),
('2025-02-18 19:40:10.789', 1, 'C004', 'The Batman', 'Cinema', 'G07', 48000);


