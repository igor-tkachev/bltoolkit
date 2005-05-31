using System.Reflection;
using System.Runtime.CompilerServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//

[assembly: System.CLSCompliantAttribute(true)] 

// Set neutral resources language for assembly.
[assembly: System.Resources.NeutralResourcesLanguageAttribute("en")]

[assembly: AssemblyTitle("NullableTypes for .NET")] // Description
[assembly: AssemblyDescription("Nullable version of .NET built-in value-types")] // Comments
//[assembly: AssemblyConfiguration("beta")]
[assembly: AssemblyCompany("Luca Minudel")]
[assembly: AssemblyProduct("NullableTypes")] // Product Name
[assembly: AssemblyCopyright("Copyleft © 2003 Luca Minudel, MIT license")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]        

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("1.1.*")]
[assembly: AssemblyInformationalVersion("1.1.1")] // odd beta version 1.1.* - stable will be 1.2
//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile 
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: AssemblyDelaySign(false)]
#if DEBUG
[assembly: AssemblyKeyFile("")]
#else
[assembly: AssemblyKeyFile("..\\..\\snk\\NullableTypes.snk")]
#endif
[assembly: AssemblyKeyName("")]

// COM visible value types should not expose non-public instance fields because they are
// visible by COM clients. NullableTypes value-types make use of non-public instance fields
// so they are not suited for COM clients.
[assembly: System.Runtime.InteropServices.ComVisible(false)] 
