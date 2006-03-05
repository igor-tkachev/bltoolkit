--CREATE DATABASE BLToolkitData ON PRIMARY
--(NAME=N'BLToolkitTest',     FILENAME=N'C:\Data\MSSQL.1\MSSQL\DATA\BLToolkitData.mdf',     SIZE=3072KB, FILEGROWTH=1024KB)
--LOG ON 
--(NAME=N'BLToolkitTest_log', FILENAME=N'C:\Data\MSSQL.1\MSSQL\DATA\BLToolkitData_log.ldf', SIZE=1024KB, FILEGROWTH=10%)
--GO

-- Person Table

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

-- Person_SelectByKey

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Person_SelectByKey')
BEGIN
	DROP  Procedure  Person_SelectByKey
END
GO

CREATE Procedure Person_SelectByKey
	@id int
AS

SELECT * FROM Person WHERE PersonID = @id

GO

GRANT EXEC ON Person_SelectByKey TO PUBLIC
GO

-- Person_SelectAll

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Person_SelectAll')
BEGIN
	DROP  Procedure  Person_SelectAll
END
GO

CREATE Procedure Person_SelectAll
AS

SELECT * FROM Person

GO

GRANT EXEC ON Person_SelectAll TO PUBLIC
GO

-- Person_SelectByName

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Person_SelectByName')
BEGIN
	DROP  Procedure  Person_SelectByName
END
GO

CREATE Procedure Person_SelectByName
	@firstName nvarchar(50),
	@lastName  nvarchar(50)
AS

SELECT
	*
FROM
	Person
WHERE
	FirstName = @firstName AND LastName = @lastName

GO

GRANT EXEC ON Person_SelectByName TO PUBLIC
GO

-- Person_SelectListByName

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Person_SelectListByName')
BEGIN
	DROP  Procedure  Person_SelectListByName
END
GO

CREATE Procedure Person_SelectListByName
	@firstName nvarchar(50),
	@lastName  nvarchar(50)
AS

SELECT
	*
FROM
	Person
WHERE
	FirstName like @firstName AND LastName like @lastName

GO

GRANT EXEC ON Person_SelectByName TO PUBLIC
GO

-- Person_Insert

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Person_Insert')
BEGIN
	DROP  Procedure  Person_Insert
END
GO

CREATE Procedure Person_Insert
	@FirstName  nvarchar(50),
	@LastName   nvarchar(50),
	@MiddleName nvarchar(50),
	@Gender     char(1)
AS

INSERT INTO Person
	( LastName,  FirstName,  MiddleName,  Gender)
VALUES
	(@LastName, @FirstName, @MiddleName, @Gender)

SELECT Cast(SCOPE_IDENTITY() as int)

GO

GRANT EXEC ON Person_Insert TO PUBLIC
GO

-- Person_Update

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Person_Update')
BEGIN
	DROP  Procedure  Person_Update
END
GO

CREATE Procedure Person_Update
	@PersonID   int,
	@FirstName  nvarchar(50),
	@LastName   nvarchar(50),
	@MiddleName nvarchar(50),
	@Gender     char(1)
AS

UPDATE
	Person
SET
	LastName   = @LastName,
	FirstName  = @FirstName,
	MiddleName = @MiddleName,
	Gender     = @Gender
WHERE
	PersonID = @PersonID

GO

GRANT EXEC ON Person_Update TO PUBLIC
GO

-- Person_Delete

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Person_Delete')
BEGIN
	DROP  Procedure  Person_Delete
END
GO

CREATE Procedure Person_Delete
	@PersonID int
AS

DELETE FROM Person WHERE PersonID = @PersonID

GO

GRANT EXEC ON Person_Delete TO PUBLIC
GO

-- BinaryData Table

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('BinaryData') AND type in (N'U'))
BEGIN
	DROP TABLE BinaryData
END
CREATE TABLE BinaryData
(
	BinaryDataID int             NOT NULL IDENTITY(1,1) CONSTRAINT PK_BinaryData PRIMARY KEY CLUSTERED,
	Stamp        timestamp       NOT NULL,
	Data         varbinary(1024) NOT NULL)
ON [PRIMARY]
GO
