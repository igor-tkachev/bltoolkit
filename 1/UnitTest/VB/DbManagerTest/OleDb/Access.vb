Imports System
Imports System.Data

Imports NUnit.Framework

Imports Rsdn.Framework.Data

Namespace DbManagerTest.OleDb

    <TestFixture()> _
    Public Class Access
        Inherits VB.DbManagerTest.Test

        Public Overrides ReadOnly Property ConfigurationString() As String
            Get
                Return "Access.OleDb"
            End Get
        End Property

    End Class

End Namespace

