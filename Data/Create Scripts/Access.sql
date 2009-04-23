-- SQL script generated using BatchAccess utility

-- Tables

CREATE TABLE Person (
    PersonID                AutoIncrement,
    FirstName               Text(50) WITH COMP NOT NULL,
    LastName                Text(50) WITH COMP NOT NULL,
    MiddleName              Text(50) WITH COMP,
    Gender                  Text(1) NOT NULL,

    CONSTRAINT PrimaryKey PRIMARY KEY (PersonID)
);

CREATE TABLE Doctor (
    PersonID                Long NOT NULL DEFAULT 0,
    Taxonomy                Text(50) WITH COMP NOT NULL,

    CONSTRAINT PrimaryKey PRIMARY KEY (PersonID)
);

CREATE TABLE Patient (
    PersonID                Long DEFAULT 0,
    Diagnosis               Text(255) WITH COMP,
    
    CONSTRAINT PrimaryKey PRIMARY KEY (PersonID)
);

ALTER TABLE Doctor
    ADD CONSTRAINT PersonDoctor FOREIGN KEY (PersonID) REFERENCES Person ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE Patient
    ADD CONSTRAINT PersonPatient FOREIGN KEY (PersonID) REFERENCES Person ON UPDATE CASCADE ON DELETE CASCADE;


CREATE TABLE BinaryData (
    BinaryDataID            AutoIncrement,
    Data                    Image NOT NULL,
    
    CONSTRAINT PrimaryKey PRIMARY KEY (BinaryDataID)
);

CREATE TABLE DataTypeTest (
    DataTypeID              AutoIncrement,
    Binary_                 Image,
    Boolean_                Long,
    Byte_                   Byte DEFAULT 0,
    Bytes_                  Image,
    Char_                   Text(1),
    DateTime_               DateTime,
    Decimal_                Currency DEFAULT 0,
    Double_                 Double DEFAULT 0,
    Guid_                   Uniqueidentifier,
    Int16_                  SmallInt DEFAULT 0,
    Int32_                  Long DEFAULT 0,
    Int64_                  Long DEFAULT 0,
    Money_                  Currency DEFAULT 0,
    SByte_                  Byte DEFAULT 0,
    Single_                 Single DEFAULT 0,
    Stream_                 Image,
    String_                 Text(50) WITH COMP,
    UInt16_                 SmallInt DEFAULT 0,
    UInt32_                 Long DEFAULT 0,
    UInt64_                 Long DEFAULT 0,    
    Xml_                    Text WITH COMP,
    
    CONSTRAINT PrimaryKey PRIMARY KEY (DataTypeID)
);


-- Dual table FOR supporting queryies LIKE:
-- SELECT 1 AS id => SELECT 1 AS "id" *FROM Dual*
CREATE TABLE Dual (
    Dummy                   Text(10) WITH COMP);

-- Data

INSERT INTO Person (FirstName, LastName, Gender) VALUES ("John",   "Pupkin",    "M");
INSERT INTO Person (FirstName, LastName, Gender) VALUES ("Tester", "Testerson", "M");

INSERT INTO Doctor (PersonID, Taxonomy)   VALUES (1, "Psychiatry");
INSERT INTO Patient (PersonID, Diagnosis) VALUES (2, "Hallucination with Paranoid Bugs' Delirium of Persecution");

INSERT INTO DataTypeTest
	(Binary_, Boolean_,   Byte_,  Bytes_,  Char_,  DateTime_, Decimal_,
	 Double_,    Guid_,  Int16_,  Int32_,  Int64_,    Money_,   SByte_,
	 Single_,  Stream_, String_, UInt16_, UInt32_,   UInt64_,     Xml_)
VALUES
	(   NULL,     NULL,    NULL,    NULL,    NULL,      NULL,     NULL,
	    NULL,     NULL,    NULL,    NULL,    NULL,      NULL,     NULL,
	    NULL,     NULL,    NULL,    NULL,    NULL,      NULL,     NULL);

INSERT INTO DataTypeTest
	(Binary_, Boolean_,   Byte_,  Bytes_,  Char_,  DateTime_, Decimal_,
	 Double_,    Guid_,  Int16_,  Int32_,  Int64_,    Money_,   SByte_,
	 Single_,  Stream_, String_, UInt16_, UInt32_,   UInt64_,
	 Xml_)
VALUES
	(1,        True,      255,  1,         "B",     Now(), 12345.67,
	1234.567,     1,    32767,  32768, 1000000,   12.3456,      127,
	1234.123,     "12345678", "string",  32767,   32768, 2000000000,
	"<root><element strattr='strvalue' intattr='12345'/></root>");

-- Constrains

INSERT INTO  Dual (Dummy) VALUES ('X');

-- Procedures

CREATE Procedure Person_SelectByKey(
	[@id] Long)
AS
SELECT * FROM Person WHERE PersonID = [@id];

-- GO

CREATE Procedure Person_SelectAll
AS
SELECT * FROM Person;

-- GO

CREATE Procedure Person_SelectByName(
	[@firstName] Text(50),
	[@lastName]  Text(50))
AS
SELECT
	*
FROM
	Person
WHERE
	FirstName = [@firstName] AND LastName = [@lastName];

-- GO

CREATE Procedure Person_SelectListByName(
	[@firstName] Text(50),
	[@lastName]  Text(50))
AS
SELECT
	*
FROM
	Person
WHERE
	FirstName like [@firstName] AND LastName like [@lastName];


-- GO

CREATE Procedure Person_Insert(
    [@FirstName]  Text(50),
    [@MiddleName] Text(50),
    [@LastName]   Text(50),
    [@Gender]     Text(1))
AS
INSERT INTO Person
    (FirstName, MiddleName, LastName, Gender)
VALUES
    ([@FirstName], [@MiddleName], [@LastName], [@Gender]);
	
-- GO

CREATE Procedure Person_Update(
	[@id]         Long,
	[@PersonID]   Long,
	[@FirstName]  Text(50),
	[@MiddleName] Text(50),
	[@LastName]   Text(50),
	[@Gender]     Text(1))
AS
UPDATE
	Person
SET
	LastName   = [@LastName],
	FirstName  = [@FirstName],
	MiddleName = [@MiddleName],
	Gender     = [@Gender]
WHERE
	PersonID = [@id];
	
-- GO

CREATE Procedure Person_Delete(
	[@PersonID] Long)
AS
DELETE FROM Person WHERE PersonID = [@PersonID];

-- GO

CREATE Procedure Patient_SelectAll
AS
SELECT
	Person.*, Patient.Diagnosis
FROM
	Patient, Person
WHERE
	Patient.PersonID = Person.PersonID;
	
-- GO

CREATE Procedure Patient_SelectByName(
	[@firstName] Text(50),
	[@lastName]  Text(50))
AS
SELECT
	Person.*, Patient.Diagnosis
FROM
	Patient, Person
WHERE
	Patient.PersonID = Person.PersonID
	AND FirstName = [@firstName] AND LastName = [@lastName];
	
-- GO

CREATE Procedure Scalar_DataReader
AS
SELECT 12345 AS intField, "54321" AS stringField;

-- End
