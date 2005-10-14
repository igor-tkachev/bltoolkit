''@ example:
''@ emit Emit
Imports System
Imports NUnit.Framework
Imports BLToolkit.Reflection.Emit

Namespace Examples.Reflection.Emit

    <TestFixture()> _
    Public Class HelloWorld

        Public Interface IHello
            Sub SayHello()
        End Interface

        <Test()> _
        Sub Test()
            Dim assemblyHelper As AssemblyBuilderHelper = New AssemblyBuilderHelper("HelloWorld.dll")
            Dim typeHelper As TypeBuilderHelper = assemblyHelper.DefineType("Hello", GetType(Object), GetType(IHello))
            Dim methodHelper As MethodBuilderHelper = typeHelper.DefineMethod(GetType(IHello).GetMethod("SayHello"))
            Dim emit As EmitHelper = methodHelper.Emitter

            Dim params() As Type = Array.CreateInstance(GetType(Type), 1)
            params(0) = GetType(String)

            emit.ldstr("Hello, World!")
            emit.call(GetType(Console).GetMethod("WriteLine", params))
            emit.ret()

            Dim type As Type = typeHelper.Create()

            Dim hello As IHello = Activator.CreateInstance(type)

            hello.SayHello()
        End Sub

    End Class

End Namespace
