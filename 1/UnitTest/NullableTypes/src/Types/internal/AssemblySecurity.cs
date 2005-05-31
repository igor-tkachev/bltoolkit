//
// NullableTypes.AssemblySecurity
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 03-Jul-2003  Luca    Create
//

using System;
using System.Security.Permissions;

// RequestMinimum
[assembly:SecurityPermission(SecurityAction.RequestMinimum, Execution=true)]

// RequestRefuse
[assembly:SecurityPermission(SecurityAction.RequestRefuse, UnmanagedCode=true)]
[assembly:EnvironmentPermission(SecurityAction.RequestRefuse, Unrestricted=true)]
[assembly:FileDialogPermission(SecurityAction.RequestRefuse, Unrestricted=true)]
[assembly:FileIOPermission(SecurityAction.RequestRefuse, Unrestricted=true)]
[assembly:IsolatedStorageFilePermission(SecurityAction.RequestRefuse, Unrestricted=true)]
[assembly:ReflectionPermission(SecurityAction.RequestRefuse, Unrestricted=true)]
[assembly:RegistryPermission(SecurityAction.RequestRefuse, Unrestricted=true)]
[assembly:UIPermission(SecurityAction.RequestRefuse, Unrestricted=true)]
