Imports Data.Linq.Model

Public Module VisualBasicCommon

	Public Function ParamenterName(ByVal db As TestDbManager) As IEnumerable(Of Parent)
		Dim id As Integer
		id = 1
		Return From p In db.Parent Where p.ParentID = id Select p
	End Function

End Module
