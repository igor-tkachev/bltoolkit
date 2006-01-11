--CREATE DATABASE BLToolkitData ON PRIMARY
--( NAME = N'BLToolkitTest',     FILENAME = N'C:\Data\MSSQL.1\MSSQL\DATA\BLToolkitData.mdf' ,     SIZE = 3072KB , FILEGROWTH = 1024KB )
--LOG ON 
--( NAME = N'BLToolkitTest_log', FILENAME = N'C:\Data\MSSQL.1\MSSQL\DATA\BLToolkitData_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10% )
--GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('Person') AND type in (N'U'))
BEGIN
	DROP TABLE Person
END

CREATE TABLE Person
(
	PersonID   int          NOT NULL IDENTITY(1,1) CONSTRAINT PK_Person PRIMARY KEY CLUSTERED,
	FirstName  nvarchar(50) NOT NULL,
	LastName   nvarchar(50) NOT NULL,
	MiddleName nvarchar(50)     NULL,
	Gender     char(1)      NOT NULL CONSTRAINT CK_Person_Gender CHECK (Gender in ('M', 'F', 'U', 'O'))
)
ON [PRIMARY]
GO

INSERT INTO Person (FirstName, LastName, Gender)
VALUES             ('John',    'Pupkin', 'M')
GO
