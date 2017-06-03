Oracle.ManagedDataAccess NuGet Package 12.2.1100 README
===========================================================

Release Notes: Oracle Data Provider for .NET, Managed Driver

May 2017

Copyright (c) 2017, Oracle and/or its affiliates. All rights reserved.

This document provides information that supplements the Oracle Data Provider for .NET (ODP.NET) documentation. 
You have downloaded Oracle Data Provider for .NET from Oracle, the license agreement to which is available at 
http://www.oracle.com/technetwork/licenses/distribution-license-152002.html

TABLE OF CONTENTS
*New Features
*Bug Fixes
*Installation and Configuration Steps
*Installation Changes
*Documentation Corrections and Additions
*ODP.NET, Managed Driver Tips, Limitations, and Known Issues

Note: The 32-bit "Oracle Developer Tools for Visual Studio" download from http://otn.oracle.com/dotnet is 
required for Entity Framework design-time features and for other Visual Studio designers such as the 
TableAdapter Wizard. This NuGet download does not enable design-time tools; it only provides run-time support. 
This version of ODP.NET supports Oracle Database version 10.2 and higher.


New Features since Oracle.ManagedDataAccess NuGet Package 12.1.24160719
=======================================================================
1. Database Resident Connection Pooling
2. Multitenant and Pluggable Databases Connection Pooling
3. Edition-Based Redefinition Connection Pooling
4. Connection Configuration Upon Open
5. .NET Framework 4.6.2 and 4.7 certification
6. Longer Schema Identifiers
7. PL/SQL Boolean Data Type

For more details on these features, visit the new features section of the ODP.NET documentation:
http://docs.oracle.com/cd/E85694_01/ODPNT/release_changes.htm#GUID-23EE609C-064C-484E-9D3A-C9CA4E1A970F


Bug Fixes since Oracle.ManagedDataAccess NuGet Package 12.1.24160719
====================================================================
24700485 VALUES ARE PARTIALLY FILLED IN DATATABLE USING MANAGED ODP.NET                 
22765798 ODPM: INDEX OUTSIDE ARRAY BOUNDS EXCEPTION WHEN FETCHSIZE HAS LARGE VALUE      
24810947 KERBEROS INITIAL HANDSHAKE FAILS IF "AUTHENTICATOR" IS GREATER THAN SDU SIZE   
25490365 LATEST DST PATCH NEEDS TO BE ADDED FOR ODP.NET MANAGED DRIVER                  
22385038 ORA-31061: XDB ERROR WHILE INSERTING DATA INTO XMLTYPE DATATYPE USING ODPM     
24299880 ORA-00303 WHEN PROGRAM PATH CONTAINS # CHARACTER AND POOLING ENABLED           
21393655 ODPM THROWS "INDEX WAS OUT OF RANGE" EXCEPTION  WHILE FETCHING NULL AND XMLTYPE
21847644 SYSTEM.INDEXOUTOFRANGEEXCEPTION OR SYSTEM.ARGUMENTEXCEPTION WITH MANAGED ODP.NET
22308527 UNEXPECTED PACKET ERROR WITH NEWLINE WITHIN SQL QUERY  


Installation and Configuration Steps
====================================
The downloads are NuGet packages that can be installed with the NuGet Package Manager. These instructions apply 
to install ODP.NET, Managed Driver.

1. Un-GAC and un-configure any existing assembly (i.e. Oracle.ManagedDataAccess.dll) and policy DLL 
(i.e. Policy.4.122.Oracle.ManagedDataAccess.dll) for the ODP.NET, Managed Driver, version 12.2.0.1
that exist in the GAC. Remove all references of Oracle.ManagedDataAccess from machine.config file, if any exists.

2. In Visual Studio, open NuGet Package Manager from an existing Visual Studio project. 

3. Install the NuGet package from an OTN-downloaded local package source or from nuget.org.


   From Local Package Source
   -------------------------
   A. Click on the Settings button in the lower left of the dialog box.

   B. Click the "+" button to add a package source. In the Source field, enter in the directory location where the 
   NuGet package(s) were downloaded to. Click the Update button, then the Ok button.

   C. On the left side, under the Online root node, select the package source you just created. The ODP.NET NuGet 
   packages will appear.


   From Nuget.org
   --------------
   A. In the Search box in the upper right, search for the package with id, "Oracle.ManagedDataAccess". Verify 
   that the package uses this unique ID to ensure it is the official Oracle Data Provider for .NET, Managed Driver 
   download.

   B. Select the package you wish to install.


4. Click on the Install button to select the desired NuGet package(s) to include with the project. Accept the 
license agreement and Visual Studio will continue the setup.

5. Open the app/web.config file to configure the ODP.NET connection string and local naming parameters 
(i.e. tnsnames.ora). Below is an example of configuring the local naming parameters:

  <oracle.manageddataaccess.client>
    <version number="*">
      <dataSources>
        <!-- Customize these connection alias settings to connect to Oracle DB -->
        <dataSource alias="MyDataSource" descriptor="(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL))) " />
      </dataSources>
    </version>
  </oracle.manageddataaccess.client>

After following these instructions, ODP.NET is now configured and ready to use.

NOTE: ODP.NET, Managed Driver comes with two platform specific assemblies:

        i.  Oracle.ManagedDataAccessDTC.dll (for Distributed Transaction Support)
        ii. Oracle.ManagedDataAccessIOP.dll (for Kerberos Support)

The Oracle.ManagedDataAccessDTC.dll assembly is ONLY needed if you are using Distributed Trasactions and the 
.NET Framework being used is 4.5.1 or lower. If you are using .NET Framework 4.5.2 or higher, this assembly does 
not need to be referenced by your application.

The Oracle.ManagedDataAccessIOP.dll assembly is ONLY needed if you are using Kerberos5 based external 
authentication. Kerberos5 users will need to download MIT Kerberos for Windows version 4.0.1 from 
	http://web.mit.edu/kerberos/dist/
to utilize ODP.NET, Managed Driver's support of Kerberos5.

These asssemblies are located under
      packages\Oracle.ManagedDataAccess.<version>\bin\x64
and
      packages\Oracle.ManagedDataAccess.<version>\bin\x86
depending on the platform.

If these assemblies are required by your application, your Visual Studio project requires additional changes.

Use the following steps for your application to use the 64-bit version of Oracle.ManagedDataAccessDTC.dll:

1. Right click on the Visual Studio project.
2. Select Add -> New Folder
3. Name the folder x64.
4. Right click on the newly created x64 folder
5. Select Add -> Existing Item
6. Browse to packages\Oracle.ManagedDataAccess.<version>\bin\x64 under your project solution directory.
7. Choose Oracle.ManagedDataAccessDTC.dll
8. Click the 'Add' button
9. Left click the newly added Oracle.ManagedDataAccessDTC.dll in the x64 folder
10. In the properties window, set 'Copy To Output Directory' to 'Copy Always'.

For x86 targeted applications, name the folder x86 and add assemblies from the 
packages\Oracle.ManagedDataAccess.<version>\bin\x86 folder.

Use the same steps for adding Oracle.ManagedDataAccessIOP.dll.

To make your application platform independent even if it depends on Oracle.ManagedDataAccessDTC.dll and/or 
Oracle.ManagedDataAccessIOP.dll, create both x64 and x86 folders with the necessary assemblies added to them.


Installation Changes
====================
The following app/web.config entries are added by including the "Official Oracle ODP.NET, Managed Driver" NuGet package 
to your application:

1) Configuration Section Handler

The following entry is added to the app/web.config to enable applications to add an <oracle.manageddataaccess.client> 
section for ODP.NET, Managed Driver-specific configuration:

<configuration>
  <configSections>
    <section name="oracle.manageddataaccess.client" type="OracleInternal.Common.ODPMSectionHandler, Oracle.ManagedDataAccess, Version=4.122.1.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />
  </configSections>
</configuration>

Note: If your application is a web application and the above entry was added to a web.config and the same config 
section handler for "oracle.manageddataaccess.client" also exists in machine.config but the "Version" attribute values 
are different, an error message of "There is a duplicate 'oracle.manageddataaccess.client' section defined." may be 
observed at runtime.  If so, the config section handler entry in the machine.config for 
"oracle.manageddataaccess.client" has to be removed from the machine.config for the web application to not encounter 
this error.  But given that there may be other applications on the machine that depended on this entry in the 
machine.config, this config section handler entry may need to be moved to all of the application's .NET config file on 
that machine that depend on it.

2) DbProviderFactories

The following entry is added for applications that use DbProviderFactories and DbProviderFactory classes. Also, any 
DbProviderFactories entry for "Oracle.ManagedDataAccess.Client" in the machine.config will be ignored with the following 
entry:

<configuration>
  <system.data>
    <DbProviderFactories>
      <remove invariant="Oracle.ManagedDataAccess.Client" />
      <add name="ODP.NET, Managed Driver" invariant="Oracle.ManagedDataAccess.Client" description="Oracle Data Provider for .NET, Managed Driver" type="Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess, Version=4.122.1.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />
    </DbProviderFactories>
  </system.data>
</configuration>

3) Dependent Assembly

The following entry is created to ignore policy DLLs for Oracle.ManagedDataAccess.dll and always use the 
Oracle.ManagedDataAccess.dll version that is specified by the newVersion attribute in the <bindingRedirect> element.  
The newVersion attribute corresponds to the Oracle.ManagedDataAccess.dll version which came with the NuGet package 
associated with the application.

<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <publisherPolicy apply="no" />
        <assemblyIdentity name="Oracle.ManagedDataAccess" publicKeyToken="89b483f429c47342" culture="neutral" />
        <bindingRedirect oldVersion="4.122.0.0 - 4.65535.65535.65535" newVersion="4.122.1.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>

4) Data Sources

The following entry is added to provide a template on how a data source can be configured in the app/web.config. 
Simply rename "MyDataSource" to an alias of your liking and modify the PROTOCOL, HOST, PORT, SERVICE_NAME as required 
and un-comment the <dataSource> element. Once that is done, the alias can be used as the "data source" attribute in 
your connection string when connecting to an Oracle Database through ODP.NET, Managed Driver.

<configuration>
  <oracle.manageddataaccess.client>
    <version number="*">
      <dataSources>
        <dataSource alias="SampleDataSource" descriptor="(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL))) " />
      </dataSources>
    </version>
  </oracle.manageddataaccess.client>
</configuration>


Documentation Corrections and Additions
=======================================
This section contains information that corrects or adds to existing ODP.NET documentation, which can be found here:
http://www.oracle.com/technetwork/topics/dotnet/tech-info/index.html


ODP.NET, Managed Driver Tips, Limitations, and Known Issues
===========================================================
This section contains information that is specific to ODP.NET, Managed Driver. 

1. OracleConnection object's OpenWithNewPassword() method invocation will result in an ORA-1017 error with 11.2.0.3.0 
and earlier versions of the database. [Bug 12876992]

2. ODP.NET does not support usage of the "ALTER SESSION" statement to modify the Edition in Edition-Based Redefinition during the lifetime of a process.

3. ODP.NET, Managed Driver and Distributed Transactions - Using managed ODP.NET distributed transactions with Oracle.ManagedDataAccessDTC.dll is deprecated as it is primarily used with .NET Framework 4 releases earlier than .NET 4.5.2. Microsoft has desupported these earlier .NET Framework 4 versions. Managed ODP.NET distributed transactions will continue to be supported and enhanced with .NET Framework's native fully managed distributed transaction implementation.