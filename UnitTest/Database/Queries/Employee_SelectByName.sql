IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Employee_SelectByName')
BEGIN
	DROP  Procedure  Employee_SelectByName
END
GO

CREATE Procedure Employee_SelectByName
	@firstName varchar(10),
	@lastName  varchar(20)
AS

SELECT
	*
FROM
	Employees
WHERE
	FirstName = @firstName AND LastName = @lastName

GO

GRANT EXEC ON Employee_SelectByName TO PUBLIC
GO

