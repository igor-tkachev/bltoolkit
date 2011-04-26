DROP TABLE "Person"
GO
--DROP SEQUENCE Seq
--GO

--CREATE SEQUENCE Seq INCREMENT 1 START 1
--GO

CREATE TABLE "Person"
( 
	--PersonID   INTEGER PRIMARY KEY DEFAULT NEXTVAL('Seq'),
	"PersonID"   SERIAL PRIMARY KEY,
	"FirstName"  VARCHAR(50) NOT NULL,
	"LastName"   VARCHAR(50) NOT NULL,
	"MiddleName" VARCHAR(50),
	"Gender"     CHAR(1)     NOT NULL
)
GO

INSERT INTO "Person" ("FirstName", "LastName", "Gender") VALUES ('John',   'Pupkin',    'M')
GO
INSERT INTO "Person" ("FirstName", "LastName", "Gender") VALUES ('Tester', 'Testerson', 'M')
GO

DROP FUNCTION reverse(text)
GO

CREATE FUNCTION reverse(text) RETURNS text
	AS $_$
DECLARE
original alias for $1;
	reverse_str text;
	i int4;
BEGIN
	reverse_str := '';
	FOR i IN REVERSE LENGTH(original)..1 LOOP
		reverse_str := reverse_str || substr(original,i,1);
	END LOOP;
RETURN reverse_str;
END;$_$
	LANGUAGE plpgsql IMMUTABLE;
GO



DROP TABLE "Parent"
GO
DROP TABLE "Child"
GO
DROP TABLE "GrandChild"
GO

CREATE TABLE "Parent"      ("ParentID" int, "Value1" int)
GO
CREATE TABLE "Child"       ("ParentID" int, "ChildID" int)
GO
CREATE TABLE "GrandChild"  ("ParentID" int, "ChildID" int, "GrandChildID" int)
GO


DROP TABLE "LinqDataTypes"
GO

CREATE TABLE "LinqDataTypes"
(
	"ID"            int,
	"MoneyValue"    decimal(10,4),
	"DateTimeValue" timestamp,
	"BoolValue"     boolean,
	"GuidValue"     uuid,
	"BinaryValue"   bytea,
	"SmallIntValue" smallint,
	"IntValue"      int NULL
)
GO
