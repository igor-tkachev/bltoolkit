SELECT
	[MiddleName],
	[PersonID],
	[LastName],
	[FirstName]
FROM
	[Person]
WHERE
	[FirstName] = @FirstName AND
	[LastName] = @LastName
