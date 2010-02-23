DROP TABLE Person
GO

CREATE TABLE Person
(
	PersonID   SERIAL       NOT NULL,
	FirstName  NVARCHAR(50) NOT NULL,
	LastName   NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50),
	Gender     CHAR(1)      NOT NULL,

	PRIMARY KEY(PersonID)
)
GO

INSERT INTO Person (FirstName, LastName, Gender) VALUES ('John',   'Pupkin',    'M')
GO
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Tester', 'Testerson', 'M')
GO


DROP TABLE Parent
GO
DROP TABLE Child
GO
DROP TABLE GrandChild
GO

CREATE TABLE Parent      (ParentID int, Value1 int)
GO
CREATE TABLE Child       (ParentID int, ChildID int)
GO
CREATE TABLE GrandChild  (ParentID int, ChildID int, GrandChildID int)
GO


DROP TABLE LinqDataTypes
GO

CREATE TABLE LinqDataTypes
(
	ID            int,
	MoneyValue    decimal(10,4),
	DateTimeValue datetime year to fraction(3),
	BoolValue     boolean,
	GuidValue     char(36),
	BinaryValue   byte
)
GO
