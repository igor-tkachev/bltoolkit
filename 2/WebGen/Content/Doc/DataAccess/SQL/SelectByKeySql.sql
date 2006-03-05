SELECT
	[MiddleName],
	[PersonID],
	[LastName],
	[FirstName]
FROM
	[Person]
WHERE
	[PersonID] = @PersonID
