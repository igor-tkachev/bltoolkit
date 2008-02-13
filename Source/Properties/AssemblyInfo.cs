using System;
using System.Reflection;
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
[assembly: AssemblyCopyright("\xA9 2002-2008 www.bltoolkit.net")]
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
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.462")]

// FxCop

[assembly: CLSCompliant(true)]

[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: SuppressMessage("Microsoft.Usage",  "CA2209:AssembliesShouldDeclareMinimumSecurity")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "BLToolkit.TypeBuilder.TypeFactory..cctor()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "BLToolkit.TypeBuilder.Builders.FakeParameterInfo..ctor(System.Reflection.MethodInfo)")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", Scope = "member", Target = "BLToolkit.Mapping.ObjectMapper.Item[System.String]", MessageId = "name")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "BLToolkit.Aspects")]
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "BLToolkit.Mapping.MemberMapper.MapTo(System.Object,BLToolkit.Mapping.MapMemberInfo):System.Object")]
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "BLToolkit.Mapping.MemberMapper.MapFrom(System.Object,BLToolkit.Mapping.MapMemberInfo):System.Object")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Scope = "member", Target = "BLToolkit.Mapping.MappingSchema.MapInternal(BLToolkit.Reflection.InitContext):System.Object")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Scope = "member", Target = "BLToolkit.Mapping.MappingSchema.MapInternal(BLToolkit.Mapping.IMapDataSource,System.Object,BLToolkit.Mapping.IMapDataDestination,System.Object,System.Int32[]):System.Void")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Scope = "member", Target = "BLToolkit.Mapping.MappingSchema.GetIndex(BLToolkit.Mapping.IMapDataSource,BLToolkit.Mapping.IMapDataDestination):System.Int32[]")]
[assembly: SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Scope = "member", Target = "BLToolkit.Mapping.DataRowMapper..ctor(System.Data.DataRowView)")]

