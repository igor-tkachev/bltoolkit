Imports System
Imports System.Data

Imports NUnit.Framework

Imports Rsdn.Framework.Data

Namespace DbManagerTest

    <TestFixture()> _
    Public Class SqlServer
        Inherits VB.DbManagerTest.Test

        Public Overrides ReadOnly Property ConfigurationString() As String
            Get
                Return "SqlServer"
            End Get
        End Property

        Public Overrides Sub SetCommand_CommandType_TableDirect_ExecuteNonQuery()
        End Sub
    End Class

End Namespace

