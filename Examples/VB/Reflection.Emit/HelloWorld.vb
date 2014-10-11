''@ example:
''@ emit Emit
Imports System
Imports NUnit.Framework

Imports BLToolkit.Reflection
Imports BLToolkit.Reflection.Emit

Namespace Examples.Reflection.Emit

    <TestFixture()> _
    Public Class HelloWorld

        Public Interface IHello
            Sub SayHello(ByVal toWhom As String)
        End Interface

        <Test()> _
        Sub Test()
            Dim assemblyHelper As AssemblyBuilderHelper = New AssemblyBuilderHelper("HelloWorld.dll")
            Dim typeHelper As TypeBuilderHelper = assemblyHelper.DefineType("Hello", GetType(Object), GetType(IHello))
            Dim methodHelper As MethodBuilderHelper = typeHelper.DefineMethod(GetType(IHello).GetMethod("SayHello"))
            Dim emit As EmitHelper = methodHelper.Emitter

            ' string.Format("Hello, {0} World!", toWhom)
            '
            emit _
            .ldstr("Hello, {0} World!") _
            .ldarg_1 _
            .call(GetType(String), "Format", GetType(String), GetType(Object))

            ' Console.WriteLine("Hello, World!");
            '
            emit _
            .call(GetType(Console), "WriteLine", GetType(String)) _
            .ret()

            Dim type As Type = typeHelper.Create()

            Dim hello As IHello = TypeAccessor.CreateInstance(type)

            hello.SayHello("VB")
        End Sub

    End Class

End Namespace
