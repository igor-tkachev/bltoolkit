Imports System
Imports System.Data

Imports NUnit.Framework

Imports Rsdn.Framework.Data

Namespace DbManagerTest.OleDb

    <TestFixture()> _
    Public Class SqlServer
        Inherits VB.DbManagerTest.Test

        Public Overrides ReadOnly Property ConfigurationString() As String
            Get
                Return "SqlServer.OleDb"
            End Get
        End Property

        Public Overrides Function ParamText(ByVal param As String) As String
            Return "?"
        End Function

    End Class

End Namespace

