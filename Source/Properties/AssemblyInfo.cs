using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Resources;

using BLToolkit;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle        (BLToolkitConstants.ProductName)]
[assembly: AssemblyDescription  (BLToolkitConstants.ProductDescription)]
[assembly: AssemblyProduct      (BLToolkitConstants.ProductName)]
[assembly: AssemblyCopyright    (BLToolkitConstants.Copyright)]
[assembly: AssemblyCulture      ("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany      (BLToolkitConstants.ProductName)]

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
[assembly: AssemblyVersion    (BLToolkitConstants.FullVersionString)]
[assembly: AssemblyFileVersion(BLToolkitConstants.FullVersionString)]

// The AllowPartiallyTrustedCallersAttribute requires the assembly to be signed with a strong name key.
// This attribute is necessary since the control is called by either an intranet or Internet
// Web page that should be running under restricted permissions.
#if !SILVERLIGHT
[assembly: AllowPartiallyTrustedCallers]
//[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
#endif

[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguageAttribute("en-US")]
