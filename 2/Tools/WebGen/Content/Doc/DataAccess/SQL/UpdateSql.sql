UPDATE
	[Person]
SET
	[MiddleName] = @MiddleName,
	[Gender] = @Gender,
	[LastName] = @LastName,
	[FirstName] = @FirstName
WHERE
	[PersonID] = @PersonID
