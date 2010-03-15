Imports Data.Linq.Model

Public Module CompilerServices

	Public Function CompareString(ByVal db As TestDbManager) As IEnumerable(Of Person)
		Return From p In db.Person Where p.FirstName = "John" Select p
	End Function

End Module
