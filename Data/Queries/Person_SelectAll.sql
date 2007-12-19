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

