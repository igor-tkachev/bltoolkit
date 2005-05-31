Imports System
Imports System.Data

Imports NUnit.Framework

Imports Rsdn.Framework.Data

Namespace DbManagerTest

    Public MustInherit Class Test

        Public MustOverride ReadOnly Property ConfigurationString() As String

        Public Overridable Function ParamText(ByVal param As String) As String
            Return "@" + param
        End Function

#Region "ExecuteNonQuery"

        <Test()> _
        Public Overridable Sub SetCommand_CommandType_Text_ExecuteNonQuery()
            Dim db As DbManager
            db = New DbManager(ConfigurationString)

            Try

                db.SetCommand(CommandType.Text, "SELECT 1").ExecuteNonQuery()

            Finally
                If Not (db Is Nothing) Then
                    db.Dispose()
                End If
            End Try
        End Sub

        <Test()> _
        Public Overridable Sub SetCommand_CommandType_TableDirect_ExecuteNonQuery()
            Dim db As DbManager
            db = New DbManager(ConfigurationString)

            Try

                db.SetCommand(CommandType.TableDirect, "Customers").ExecuteNonQuery()

            Finally
                If Not (db Is Nothing) Then
                    db.Dispose()
                End If
            End Try
        End Sub

        <Test()> _
        Public Overridable Sub SetCommand_CommandType_StoredProcedure_ExecuteNonQuery()
            Dim db As DbManager
            db = New DbManager(ConfigurationString)

            Try

                db.SetCommand(CommandType.StoredProcedure, "[Ten Most Expensive Products]").ExecuteNonQuery()

            Finally
                If Not (db Is Nothing) Then
                    db.Dispose()
                End If
            End Try
        End Sub

#End Region

#Region "ExecuteScalar"

        <Test()> _
        Public Overridable Sub ExecuteScalar()
            Dim db As DbManager
            db = New DbManager(ConfigurationString)

            Try

                Dim count As Int32
                count = db.SetCommand( _
                    String.Format( _
                        "SELECT Count(*) FROM Customers WHERE Country = {0}", _
                        ParamText("country")), _
                    db.Parameter("@country", "USA")).ExecuteScalar()

                Assert.IsFalse(count = 0)

            Finally
                If Not (db Is Nothing) Then
                    db.Dispose()
                End If
            End Try
        End Sub

#End Region

    End Class

End Namespace

