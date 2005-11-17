using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Business Logic Toolkit")]
[assembly: AssemblyDescription("Business Logic Toolkit")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Business Logic Toolkit")]
[assembly: AssemblyCopyright("© 2002-2005 www.bltoolkit.net")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM componenets.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9a7e41f3-ca15-4dc5-b724-65b7cdbbdcd1")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]

// FxCop

[assembly: CLSCompliant(true)]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: SuppressMessage("Microsoft.Usage",  "CA2209:AssembliesShouldDeclareMinimumSecurity")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", Scope = "member", Target = "BLToolkit.Reflection.ObjectFactoryAttribute..ctor(System.Type)", MessageId = "System.String.Format(System.String,System.Object[])")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.TypeHelper.op_Implicit(BLToolkit.Reflection.TypeHelper):System.Type")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.TypeHelper.op_Implicit(System.Type):BLToolkit.Reflection.TypeHelper")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.Emit.AssemblyBuilderHelper.op_Implicit(BLToolkit.Reflection.Emit.AssemblyBuilderHelper):System.Reflection.Emit.AssemblyBuilder")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.Emit.AssemblyBuilderHelper.op_Implicit(BLToolkit.Reflection.Emit.AssemblyBuilderHelper):System.Reflection.Emit.ModuleBuilder")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.Emit.ConstructorBuilderHelper.op_Implicit(BLToolkit.Reflection.Emit.ConstructorBuilderHelper):System.Reflection.Emit.ConstructorBuilder")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.Emit.EmitHelper.op_Implicit(BLToolkit.Reflection.Emit.EmitHelper):System.Reflection.Emit.ILGenerator")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.Emit.MethodBuilderHelper.op_Implicit(BLToolkit.Reflection.Emit.MethodBuilderHelper):System.Reflection.Emit.MethodBuilder")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Scope = "member", Target = "BLToolkit.Reflection.Emit.TypeBuilderHelper.op_Implicit(BLToolkit.Reflection.Emit.TypeBuilderHelper):System.Reflection.Emit.TypeBuilder")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Scope = "member", Target = "BLToolkit.TypeBuilder.TypeBuilderException..ctor()", MessageId = "System.Exception.#ctor(System.String)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "BLToolkit.TypeBuilder.TypeFactory..cctor()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "BLToolkit.TypeBuilder.Builders.FakeParameterInfo..ctor(System.Reflection.MethodInfo)")]
