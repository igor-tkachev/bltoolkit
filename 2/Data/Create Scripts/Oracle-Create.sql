-- Cleanup schema

DECLARE
    REFCURSOR SYS_REFCURSOR;
    SEQUENCE_NAME VARCHAR2(30);
    TABLE_NAME VARCHAR2(30);
    CONSTRAINT_NAME VARCHAR2(30);
BEGIN
    OPEN REFCURSOR FOR
        SELECT SEQUENCE_NAME
        FROM USER_SEQUENCES;
	LOOP
        FETCH REFCURSOR INTO SEQUENCE_NAME;
        EXIT WHEN REFCURSOR%NOTFOUND;
  	    EXECUTE IMMEDIATE 'DROP SEQUENCE ' || SEQUENCE_NAME;
	END LOOP;

    OPEN REFCURSOR FOR
        SELECT CONSTRAINT_NAME, TABLE_NAME
        FROM USER_CONSTRAINTS
        WHERE CONSTRAINT_TYPE = 'R';
	LOOP
        FETCH REFCURSOR INTO CONSTRAINT_NAME, TABLE_NAME;
        EXIT WHEN REFCURSOR%NOTFOUND;
  	    EXECUTE IMMEDIATE 'ALTER TABLE ' || TABLE_NAME || ' DROP CONSTRAINT ' || CONSTRAINT_NAME;
	END LOOP;

    OPEN REFCURSOR FOR
        SELECT TABLE_NAME
        FROM USER_TABLES;
	LOOP
        FETCH REFCURSOR INTO TABLE_NAME;
        EXIT WHEN REFCURSOR%NOTFOUND;
  	    EXECUTE IMMEDIATE 'DROP TABLE ' || TABLE_NAME;
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
    
    , CONSTRAINT Ck_Person_Gender CHECK (Gender IN ('M', 'F', 'U', 'O'))
    )
/

-- Insert Trigger for Person

CREATE OR REPLACE TRIGGER Person_Add
BEFORE INSERT
ON Person
FOR EACH ROW
BEGIN
SELECT PersonSeq.NEXTVAL INTO :NEW.PersonID FROM dual;
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

INSERT INTO Person (FirstName, LastName, Gender) VALUES ('John',   'Pupkin',    'M');
INSERT INTO Doctor (PersonID, Taxonomy) VALUES (PersonSeq.CURRVAL, 'Psychiatry');
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Tester', 'Testerson', 'M');
INSERT INTO Patient (PersonID, Diagnosis) VALUES (PersonSeq.CURRVAL, 'Hallucination with Paranoid Bugs'' Delirium of Persecution');

-- Person_Delete

CREATE OR REPLACE 
PROCEDURE Person_Delete(PersonID IN NUMBER) IS
BEGIN
DELETE FROM Person WHERE Person.PersonID = Person_Delete.PersonID;
END;
/

-- Person_Insert

CREATE OR REPLACE 
PROCEDURE Person_Insert
	( FirstName IN NVARCHAR2
	, LastName IN NVARCHAR2
	, MiddleName IN NVARCHAR2
	, Gender IN CHAR
	, PersonID OUT NUMBER
	) IS
BEGIN
INSERT INTO Person
	( LastName,  FirstName,  MiddleName,  Gender)
VALUES
	(LastName, FirstName, MiddleName, Gender)
        RETURNING Person.PersonID INTO Person_Insert.PersonID;
END;
/

-- Person_SelectAll

CREATE OR REPLACE 
PROCEDURE Person_SelectAll(ret OUT SYS_REFCURSOR) IS
BEGIN
OPEN ret FOR
    SELECT * FROM Person;
END;
/

-- Person_SelectAllByGender

CREATE OR REPLACE 
PROCEDURE Person_SelectAllByGender(Gender IN CHAR, ret OUT SYS_REFCURSOR) IS
BEGIN
OPEN ret FOR
    SELECT * FROM Person WHERE Person.Gender = Person_SelectAllByGender.Gender;
END;
/

-- Person_SelectByKey

CREATE OR REPLACE 
PROCEDURE Person_SelectByKey(PersonID IN NUMBER, ret OUT SYS_REFCURSOR) IS
BEGIN
OPEN ret FOR
    SELECT * FROM Person WHERE Person.PersonID = Person_SelectByKey.PersonID;
END;
/

-- Person_SelectByName

CREATE OR REPLACE 
PROCEDURE Person_SelectByName
    ( FirstName IN NVARCHAR2
    , LastName IN NVARCHAR2
    , ret OUT SYS_REFCURSOR
    ) IS
BEGIN
OPEN ret FOR
    SELECT * FROM Person
        WHERE Person.FirstName = Person_SelectByName.FirstName
            AND Person.LastName = Person_SelectByName.LastName;
END;
/

-- Person_SelectListByName

CREATE OR REPLACE 
PROCEDURE Person_SelectListByName
    ( FirstName IN NVARCHAR2
    , LastName IN NVARCHAR2
    , ret OUT SYS_REFCURSOR
    ) IS
BEGIN
OPEN ret FOR
    SELECT * FROM Person
        WHERE Person.FirstName LIKE Person_SelectListByName.FirstName
            AND Person.LastName LIKE Person_SelectListByName.LastName;
END;
/

CREATE OR REPLACE 
PROCEDURE Person_Update
    ( PersonID IN NUMBER
    , FirstName IN NVARCHAR2
    , LastName IN NVARCHAR2
    , MiddleName IN NVARCHAR2
    , Gender IN CHAR
    ) IS
BEGIN
UPDATE
	Person
SET
	Person.LastName   = Person_Update.LastName,
	Person.FirstName  = Person_Update.FirstName,
	Person.MiddleName = Person_Update.MiddleName,
	Person.Gender     = Person_Update.Gender
WHERE
	Person.PersonID = Person_Update.PersonID;
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
SELECT BinaryDataSeq.NEXTVAL INTO :NEW.BinaryDataID FROM dual;
END;
/

-- OutRefTest

CREATE OR REPLACE 
PROCEDURE OutRefTest
	( ID IN NUMBER
	, outputID OUT NUMBER
	, inputOutputID IN OUT NUMBER
	, str IN NVARCHAR2
	, outputStr OUT NVARCHAR2
	, inputOutputStr IN OUT NVARCHAR2
	) IS
BEGIN
    outputID := ID;
    inputOutputID := ID + inputOutputID;
    outputStr := Str;
    inputOutputStr := Str || inputOutputStr;
END;
/

