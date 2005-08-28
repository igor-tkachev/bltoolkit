IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Employee_SelectAll')
BEGIN
	DROP  Procedure  Employee_SelectAll
END
GO

CREATE Procedure Employee_SelectAll
AS

SELECT * FROM Employees

GO

GRANT EXEC ON Employee_SelectAll TO PUBLIC
GO

