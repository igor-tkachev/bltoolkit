DROP TABLE Person
GO
DROP SEQUENCE Seq
GO

CREATE SEQUENCE Seq INCREMENT 1 START 1
GO

CREATE TABLE Person
( 
	PersonID   INTEGER PRIMARY KEY DEFAULT NEXTVAL('Seq'),
	FirstName  VARCHAR(50) NOT NULL,
	LastName   VARCHAR(50) NOT NULL,
	MiddleName VARCHAR(50),
	Gender     CHAR(1)     NOT NULL
)
GO

INSERT INTO Person (FirstName, LastName, Gender) VALUES ('John',   'Pupkin',    'M')
GO
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Tester', 'Testerson', 'M')
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



DROP TABLE Parent
GO
DROP TABLE Child
GO
DROP TABLE GrandChild
GO

CREATE TABLE Parent      (ParentID int)
GO
CREATE TABLE Child       (ParentID int, ChildID int)
GO
CREATE TABLE GrandChild  (ParentID int, ChildID int, GrandChildID int)
GO

INSERT INTO Parent     VALUES (1);
INSERT INTO Child      VALUES (1,11);
INSERT INTO GrandChild VALUES (1,11,111);

INSERT INTO Parent     VALUES (2);
INSERT INTO Child      VALUES (2,21);
INSERT INTO GrandChild VALUES (2,21,211);
INSERT INTO GrandChild VALUES (2,21,212);
INSERT INTO Child      VALUES (2,22);
INSERT INTO GrandChild VALUES (2,22,221);
INSERT INTO GrandChild VALUES (2,22,222);

INSERT INTO Parent     VALUES (3);
INSERT INTO Child      VALUES (3,31);
INSERT INTO GrandChild VALUES (3,31,311);
INSERT INTO GrandChild VALUES (3,31,312);
INSERT INTO GrandChild VALUES (3,31,313);
INSERT INTO Child      VALUES (3,32);
INSERT INTO GrandChild VALUES (3,32,321);
INSERT INTO GrandChild VALUES (3,32,322);
INSERT INTO GrandChild VALUES (3,32,323);
INSERT INTO Child      VALUES (3,33);
INSERT INTO GrandChild VALUES (3,33,331);
INSERT INTO GrandChild VALUES (3,33,332);
INSERT INTO GrandChild VALUES (3,33,333);

INSERT INTO Parent     VALUES (4);
INSERT INTO Child      VALUES (4,41);
INSERT INTO GrandChild VALUES (4,41,411);
INSERT INTO GrandChild VALUES (4,41,412);
INSERT INTO GrandChild VALUES (4,41,413);
INSERT INTO GrandChild VALUES (4,41,414);
INSERT INTO Child      VALUES (4,42);
INSERT INTO GrandChild VALUES (4,42,421);
INSERT INTO GrandChild VALUES (4,42,422);
INSERT INTO GrandChild VALUES (4,42,423);
INSERT INTO GrandChild VALUES (4,42,424);
INSERT INTO Child      VALUES (4,43);
INSERT INTO Child      VALUES (4,44);

INSERT INTO Parent     VALUES (5);
