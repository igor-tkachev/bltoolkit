DROP TABLE Person
GO

CREATE TABLE Person
( 
	PersonID   INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL,
	FirstName  VARCHAR(50) NOT NULL,
	LastName   VARCHAR(50) NOT NULL,
	MiddleName VARCHAR(50),
	Gender     CHAR(1)     NOT NULL
)
GO

INSERT INTO Person ("FirstName", "LastName", "Gender") VALUES ('John',   'Pupkin',    'M')
GO
INSERT INTO Person ("FirstName", "LastName", "Gender") VALUES ('Tester', 'Testerson', 'M')
GO
