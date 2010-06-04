''@ example:
''@ emit Emit
Imports System
Imports BLToolkit.Aspects
Imports NUnit.Framework

Imports BLToolkit.Reflection

Namespace Examples

	<TestFixture()> _
	Public Class OverloadWithDefValue

		Public MustInherit Class TestObject

			Public Sub TestMethod( _
			 ByVal passthrough As Integer, _
			 Optional ByVal strVal As String = "str", _
			 Optional ByVal intVal As Integer = 123)
				Assert.AreEqual(321, passthrough)
				Assert.AreEqual("str", strVal)
				Assert.AreEqual(123, intVal)
			End Sub

			<Overload()> _
			Public MustOverride Sub TestMethod( _
			 ByVal passthrough As Integer, _
			 ByVal dateVal As Date)

		End Class

		<Test()> _
		Sub Test()
			Dim o As TestObject = TypeAccessor(Of TestObject).CreateInstance
			o.TestMethod(321, Today)
		End Sub

	End Class

End Namespace
