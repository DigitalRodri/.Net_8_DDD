DROP TABLE [account].[Account]

CREATE TABLE [account].[Account](
	UUID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
	Email varchar(50) NOT NULL,
	Password BINARY(32) NOT NULL,
	Name varchar(25) NOT NULL,
	Surname varchar(25) NOT NULL,
	Title varchar(5) NULL,
	UTCCreatedDateTime DATETIME2 DEFAULT getdate() NOT NULL,
    UTCUpdatedDateTime DATETIME2 DEFAULT getdate() NOT NULL)