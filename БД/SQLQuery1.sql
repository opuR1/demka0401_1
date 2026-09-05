create table ProductTypes(
	Id int primary key identity(1,1),
	TypeName nvarchar(255),
	TypeCoef decimal(5,2)
);

create table Products(
	Id int primary key identity(1,1),
	ProductTypeId int foreign key references ProductTypes(Id) not null,
	ProductName nvarchar(255),
	ItemNumber int,
	MinimalPrice decimal(10,2)
);

create table PartnershipTypes(
	Id int primary key identity(1,1),
	TypeName nvarchar(255)
);

create table Regions(
	Id int primary key identity(1,1),
	RegionName nvarchar(255)
);

create table Cities(
	Id int primary key identity(1,1),
	CityName nvarchar(255)
);

create table Partners(
	Id int primary key identity(1,1),
	PartnershipId int foreign key references PartnershipTypes(Id) not null,
	Name nvarchar(255),
	DirectorLN nvarchar(255),
	DirectorFN nvarchar(255),
	DirectorMN nvarchar(255),
	Email nvarchar(100),
	Phone nvarchar(35),
	RegisterAddressIndex nvarchar(50),
	RegionId int foreign key references Regions(Id) not null,
	CityId int foreign key references Cities(Id) not null,
	RegisterAddressStreet nvarchar(255),
	RegisterAddressHouse nvarchar(10),
	INN nvarchar(30),
	Rating int
);

create table PartnerProducts(
	Id int primary key identity(1,1),
	ProductId int foreign key references Products(Id) not null,
	PartnerId int foreign key references Partners(Id) not null,
	ProductCount int
);

create table MaterialTypes(
	Id int primary key identity(1,1),
	MaterialType nvarchar(255),
	DefectRate decimal(5,2)
);