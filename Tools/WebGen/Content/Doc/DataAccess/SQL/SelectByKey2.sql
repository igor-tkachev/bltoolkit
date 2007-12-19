-- SelectByKeySql
SELECT
	[MiddleName],
	[PersonID],
	[LastName],
	[FirstName]
FROM
	[Person]
WHERE
	[PersonID] = @PersonID

-- SelectByKey
exec Person_SelectByKey @id=1
