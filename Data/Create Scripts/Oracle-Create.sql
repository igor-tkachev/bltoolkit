-- Cleanup schema

DECLARE
	REFCURSOR SYS_REFCURSOR;
	SEQUENCE_NAME VARCHAR2(30);
	TABLE_NAME VARCHAR2(30);
	CONSTRAINT_NAME VARCHAR2(30);
BEGIN
OPEN REFCURSOR FOR
	SELECT
		SEQUENCE_NAME
	FROM
		USER_SEQUENCES;
LOOP
	FETCH
		REFCURSOR
	INTO
		SEQUENCE_NAME;
	EXIT
		WHEN REFCURSOR%NOTFOUND;
	EXECUTE IMMEDIATE
		'DROP SEQUENCE ' || SEQUENCE_NAME;
END LOOP;

OPEN REFCURSOR FOR
	SELECT
		CONSTRAINT_NAME, TABLE_NAME
	FROM
		USER_CONSTRAINTS
	WHERE
		CONSTRAINT_TYPE = 'R';
LOOP
	FETCH
		REFCURSOR
	INTO
		CONSTRAINT_NAME, TABLE_NAME;
	EXIT
		WHEN REFCURSOR%NOTFOUND;
	EXECUTE IMMEDIATE
		'ALTER TABLE ' || TABLE_NAME || ' DROP CONSTRAINT ' || CONSTRAINT_NAME;
END LOOP;

OPEN REFCURSOR FOR
	SELECT
		TABLE_NAME
	FROM
		USER_TABLES;
LOOP
	FETCH
		REFCURSOR
	INTO
		TABLE_NAME;
	EXIT
		WHEN REFCURSOR%NOTFOUND;
	EXECUTE IMMEDIATE
		'DROP TABLE ' || TABLE_NAME;
END LOOP;
END;
/

-- Person Table

CREATE SEQUENCE PersonSeq
/

CREATE TABLE Person
	( PersonID                     NUMBER NOT NULL PRIMARY KEY
	, Firstname                    NVARCHAR2(50) NOT NULL
	, Lastname                     NVARCHAR2(50) NOT NULL
	, Middlename                   NVARCHAR2(50)
	, Gender                       CHAR(1) NOT NULL
	
	, CONSTRAINT Ck_Person_Gender  CHECK (Gender IN ('M', 'F', 'U', 'O'))
	)
/

-- Insert Trigger for Person

CREATE OR REPLACE TRIGGER Person_Add
BEFORE INSERT
ON Person
FOR EACH ROW
BEGIN
SELECT
	PersonSeq.NEXTVAL
INTO
	:NEW.PersonID
FROM
	dual;
END;
/

-- Doctor Table Extension

CREATE TABLE Doctor
	( PersonID                       NUMBER NOT NULL PRIMARY KEY
	, Taxonomy                       NVARCHAR2(50) NOT NULL
	
	, CONSTRAINT Fk_Doctor_Person FOREIGN KEY (PersonID)
		REFERENCES Person (PersonID) ON DELETE CASCADE
	)
/

-- Patient Table Extension

CREATE TABLE Patient
	( PersonID                       NUMBER NOT NULL PRIMARY KEY
	, Diagnosis                      NVARCHAR2(256) NOT NULL
	
	, CONSTRAINT Fk_Patient_Person FOREIGN KEY (PersonID)
		REFERENCES Person (PersonID) ON DELETE CASCADE
	)
/

-- Sample data for Person/Doctor/Patient

INSERT INTO
	Person	(FirstName, LastName, Gender)
VALUES
			(   'John', 'Pupkin',    'M');
INSERT INTO
	Doctor	(         PersonID,     Taxonomy)
VALUES 
			(PersonSeq.CURRVAL, 'Psychiatry');
INSERT INTO
	Person	(FirstName,    LastName, Gender)
VALUES
			( 'Tester', 'Testerson',    'M');
INSERT INTO
	Patient	(         PersonID,                                                    Diagnosis)
VALUES
			(PersonSeq.CURRVAL, 'Hallucination with Paranoid Bugs'' Delirium of Persecution');

-- Person_Delete

CREATE OR REPLACE 
PROCEDURE Person_Delete(pPersonID IN NUMBER) IS
BEGIN
DELETE FROM
	Person
WHERE
	PersonID = pPersonID;
END;
/

-- Person_Insert

CREATE OR REPLACE 
PROCEDURE Person_Insert
	( pFirstName  IN NVARCHAR2
	, pLastName   IN NVARCHAR2
	, pMiddleName IN NVARCHAR2
	, pGender     IN CHAR
	, pPersonID   OUT NUMBER
	) IS
BEGIN
INSERT INTO Person
	( LastName,  FirstName,  MiddleName,  Gender)
VALUES
	(pLastName, pFirstName, pMiddleName, pGender)
RETURNING
	PersonID
INTO
	pPersonID;
END;
/

-- Person_SelectAll

CREATE OR REPLACE 
FUNCTION Person_SelectAll
RETURN SYS_REFCURSOR IS
	retCursor SYS_REFCURSOR;
BEGIN
OPEN retCursor FOR
	SELECT
		*
	FROM
		Person;
RETURN
	retCursor;
END;
/

-- Person_SelectAllByGender

CREATE OR REPLACE 
FUNCTION Person_SelectAllByGender(pGender IN CHAR)
RETURN SYS_REFCURSOR IS
	retCursor SYS_REFCURSOR;
BEGIN
OPEN retCursor FOR
	SELECT
		*
	FROM
		Person
	WHERE
		Gender = pGender;
RETURN
	retCursor;
END;
/

-- Person_SelectByKey

CREATE OR REPLACE 
FUNCTION Person_SelectByKey(pPersonID IN NUMBER)
RETURN SYS_REFCURSOR IS
	retCursor SYS_REFCURSOR;
BEGIN
OPEN retCursor FOR
	SELECT
		*
	FROM
		Person
	WHERE
		PersonID = pPersonID;
RETURN
	retCursor;
END;
/

-- Person_SelectByName

CREATE OR REPLACE 
FUNCTION Person_SelectByName
	( pFirstName IN NVARCHAR2
	, pLastName  IN NVARCHAR2
	)
RETURN SYS_REFCURSOR IS
	retCursor SYS_REFCURSOR;
BEGIN
OPEN retCursor FOR
	SELECT
		*
	FROM
		Person
	WHERE
		FirstName = pFirstName AND LastName = pLastName;
RETURN
	retCursor;
END;
/

-- Person_SelectListByName

CREATE OR REPLACE 
FUNCTION Person_SelectListByName
	( pFirstName IN NVARCHAR2
	, pLastName  IN NVARCHAR2
	)
RETURN SYS_REFCURSOR IS
	retCursor SYS_REFCURSOR;
BEGIN
OPEN retCursor FOR
	SELECT
		*
	FROM
		Person
	WHERE
		FirstName LIKE pFirstName AND LastName LIKE pLastName;
RETURN
	retCursor;
END;
/

CREATE OR REPLACE 
PROCEDURE Person_Update
	( pPersonID   IN NUMBER
	, pFirstName  IN NVARCHAR2
	, pLastName   IN NVARCHAR2
	, pMiddleName IN NVARCHAR2
	, pGender     IN CHAR
	) IS
BEGIN
UPDATE
	Person
SET
	LastName   = pLastName,
	FirstName  = pFirstName,
	MiddleName = pMiddleName,
	Gender     = pGender
WHERE
	PersonID   = pPersonID;
END;
/

-- BinaryData Table

CREATE SEQUENCE BinaryDataSeq
/

CREATE TABLE BinaryData
	( BinaryDataID                 NUMBER NOT NULL PRIMARY KEY
	, Stamp                        TIMESTAMP DEFAULT SYSDATE NOT NULL
	, Data                         BLOB NOT NULL
	)
/

-- Insert Trigger for Binarydata

CREATE OR REPLACE TRIGGER BinaryData_Add
BEFORE INSERT
ON BinaryData
FOR EACH ROW
BEGIN
SELECT
	BinaryDataSeq.NEXTVAL
INTO
	:NEW.BinaryDataID
FROM
	dual;
END;
/

-- OutRefTest

CREATE OR REPLACE 
PROCEDURE OutRefTest
	( pID             IN     NUMBER
	, pOutputID       OUT    NUMBER
	, pInputOutputID  IN OUT NUMBER
	, pStr            IN     NVARCHAR2
	, pOutputStr      OUT    NVARCHAR2
	, pInputOutputStr IN OUT NVARCHAR2
	) IS
BEGIN
	pOutputID       := pID;
	pInputOutputID  := pID + pInputOutputID;
	pOutputStr      := pStr;
	pInputOutputStr := pStr || pInputOutputStr;
END;
/

-- ArrayTest

CREATE OR REPLACE 
PROCEDURE ArrayTest
	( pIntArray            IN     DBMS_UTILITY.NUMBER_ARRAY
	, pOutputIntArray      OUT    DBMS_UTILITY.NUMBER_ARRAY
	, pInputOutputIntArray IN OUT DBMS_UTILITY.NUMBER_ARRAY
	, pStrArray            IN     DBMS_UTILITY.NAME_ARRAY
	, pOutputStrArray      OUT    DBMS_UTILITY.NAME_ARRAY
	, pInputOutputStrArray IN OUT DBMS_UTILITY.NAME_ARRAY
	) IS
BEGIN
pOutputIntArray := pIntArray;

FOR i IN pIntArray.FIRST..pIntArray.LAST LOOP
	pInputOutputIntArray(i) := pInputOutputIntArray(i) + pIntArray(i);
END LOOP;

pOutputStrArray := pStrArray;

FOR i IN pStrArray.FIRST..pStrArray.LAST LOOP
	pInputOutputStrArray(i) := pInputOutputStrArray(i) || pStrArray(i);
END LOOP;
END;
/

CREATE OR REPLACE 
PROCEDURE ScalarArray
	( pOutputIntArray      OUT    DBMS_UTILITY.NUMBER_ARRAY
	) IS
BEGIN
FOR i IN 1..5 LOOP
	pOutputIntArray(i) := i;
END LOOP;
END;

-- ExecuteScalarTest

CREATE OR REPLACE 
FUNCTION Scalar_DataReader
RETURN SYS_REFCURSOR
IS
	retCursor SYS_REFCURSOR;
BEGIN
OPEN retCursor FOR
	SELECT
		12345 intField, '54321' stringField 
	FROM
		DUAL;
RETURN
	retCursor;
END;
/

CREATE OR REPLACE 
PROCEDURE Scalar_OutputParameter
	( pOutputInt    OUT BINARY_INTEGER
	, pOutputString OUT NVARCHAR2
	) IS
BEGIN
	pOutputInt := 12345;
	pOutputString := '54321';
END;
/

CREATE OR REPLACE 
FUNCTION Scalar_ReturnParameter
RETURN BINARY_INTEGER IS
BEGIN
RETURN
	12345;
END;
/
