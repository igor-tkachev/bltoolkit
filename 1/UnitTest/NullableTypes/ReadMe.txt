==========================================================
NullableTypes ver. 1.1.1 (Beta)
==========================================================

 0. THIS BETA
 1. INTRO
 2. NULLABLETYPES LICENSING
 3. NULLABLETYPES ASSEMBLY 
 4. .NET AND VS.NET VERSIONS
 5. NULLABLETYPES FAQ
 6. OVERWHELMING TYPING
 7. THANKS TO
 8. SUPPORT

==========================================================


0. THIS BETA
==============
This beta follows the beta v1.1.0, it enable NullableTypes to work smoothly 
with Web Services as built-in types and correct some bugs of beta v1.1.0. As usual this beta has been 
released to find out bugs and expose implementation defects.

To check that NullableTypes run correctly on your system, first of all do run tests (from the 'tests'
folder just execute 'RunTests.BAT' and click 'RUN' button).
To check that NullableTypes work well with Web Services try out NullableTypes with your Web Services
(use one of the NullableTypes as a parameter and as a return type, also use it as a member of a parameter 
type or as a member of the return type). 

If you get a red bar running tests or if you discover a bug don't hesitate to drop an email to 
lukadotnet@users.sourceforge.net, we love to get feedback.


1. INTRO
===========
NullableTypes for .NET are a very reliable and efficient version of built-in value-types that can be Null. 
NullableTypes pass more than 800 different test cases and have close-to-optimal efficiency as built-in 
value-types. They may be used every time you need to store a Null value in a .NET built-in value-type. 

Types implemented by NullableTypes are: NullableBoolean, NullableByte, NullableInt16, NullableInt32, 
NullableInt64, NullableSingle, NullableDouble, NullableDecimal, NullableString and NullableDateTime. 
Helper functions provide seamless integration with Windows and ASP.NET user controls and with ADO.NET.  

NullableTypes are based on solid foundation: they are based on .NET built-in value-types specifications
and design (Value Object pattern [1][2]), on SqlTypes design, on Null Object pattern [1][2] and on 
NULL semantic [3].

To check that NullableTypes run correctly on your system, first of all do run tests: from the 'tests'
folder just execute 'RunTests.BAT' and click 'RUN' button.
If you get a red bar after running tests, if you discover a bug in NullableTypes software or if you 
see a mistake in the documentation don't hesitate to drop a bug report: we love to get feedback.

[1] Test-Driven Development - Kent Beck - Three Rivers Institute - 
    http://groups.yahoo.com/group/testdrivendevelopment/files/TDD17Jul2002.pdf
[2] Refactoring: Improving the Design of Existing Code -  Martin Fowler, Kent Beck et al. - 
    Addison Wesley Professional - 1999 - http://www.refactoring.com/catalog
[3] SQL92 (ISO/IEC 9075:1992, Database Language SQL- July 30, 1992) - 
    http://www.contrib.andrew.cmu.edu/%7Eshadow/sql/sql1992.txt


2. NULLABLETYPES LICENSING
==============================
NullableTypes license is very permissive.
NullableTypes can be used by and deployed with commercial closed-source products without posing
limitations or license requirements on the commercial product.
Look the License.txt for details.


3. NULLABLETYPES ASSEMBLY
============================
NullableTypes.dll is a strong-named assembly, so you can choose to use it as a private assembly (place it 
under your application's base directory or sub-directories) or as a shared assembly (place it in the GAC
or use CODEBASE hint and place it in a shared folder or in a shared site).

We'll do the best effort to keep source-code and binary compatibility in new NullableTypes versions.
A Publisher Policy Assembly will be released to state versions compatibility.


4. .NET AND VS.NET VERSIONS
==============================
NullableTypes source code has been written and compiled against .NET Framework v1.1.
Due to  high degree of support for .NET Framework v1.0 forward compatibility (applications created 
using v1.1 may run on v1.0) NullableTypes source code still compile with .NET Framework v1.0 and
NullableTypes assembly still run and pass tests on .NET Framework v1.0.
To run NullableTypes on .NET Framework v1.0 use the configuration settings in file 
bin\NullableTypes.Tests.DLL.config to make your myAppllication.exe.config file or put them on your
Web.config file.

NullableTypes projects and source code are created using VS.NET 2002 and have been converted to VS.NET 2003.


5. NULLABLETYPES FAQ
=======================

What about NullableTypes and SqlTypes (or other nullable types provided by the .NET Data Provider, as the 
structures in System.Data.OracleClient or types in Oracle.DataAccess.Types)?
What about  NullableTypes and Nullable<T> (Whidbey)?

Find the answer here:
  http://nullabletypes.sourceforge.net/#faq


6. OVERWHELMING TYPING
=========================
All NullableTypes have a name that start with "Nullable". These names are clear but they can be 
overwhelming to type even with IntelliSense.
To save some typing, aliases can be defined. 
Do use the following aliases, instead of defining your own, to keep coding with NullableTypes consistent 
among different programmers:

    C#
    --------------------------------------------------
    using NBoolean = NullableTypes.NullableBoolean;
    using NByte = NullableTypes.NullableByte; 
    using NInt16 = NullableTypes.NullableInt16;
    using NInt32 = NullableTypes.NullableInt32;
    using NInt64 = NullableTypes.NullableInt64;
    using NSingle = NullableTypes.NullableSingle; 
    using NDouble = NullableTypes.NullableDouble;
    using NDecimal = NullableTypes.NullableDecimal;
    using NString = NullableTypes.NullableString;
    using NDateTime = NullableTypes.NullableDateTime;


    VB.NET
    --------------------------------------------------
    Imports NBoolean = NullableTypes.NullableBoolean
    Imports NByte = NullableTypes.NullableByte
    Imports NInt16 = NullableTypes.NullableInt16
    Imports NInt32 = NullableTypes.NullableInt32
    Imports NInt64 = NullableTypes.NullableInt64
    Imports NSingle = NullableTypes.NullableSingle
    Imports NDouble = NullableTypes.NullableDouble
    Imports NDecimal = NullableTypes.NullableDecimal
    Imports NString = NullableTypes.NullableString
    Imports NDateTime = NullableTypes.NullableDateTime


7. THANKS TO
===============
Thanks goes to the NullableTypes project members:
- Abhijeet Dev (NullableDecimal, NullableInt16)
- Eric Lau (NullableDateTime)
- Rob Cowell (NullableString)
- Massimo Prota (porting to Linux/Mono, VB.NET sample)

Thanks also goes to Massimo Roccaforte, Roni Burd, Partha Choudhury, Shaun Bowe and 
Alberto Tronchin for their suggestions and to Damien Guard for his help implementing Web Services 
support.

At the end thanks also goes to the open-source community that developed these tools:
- NUnit
- Anakrino
- NDoc
and to those MS developers who are on the bright-side and developed the FxCop tool.
The mono project source code too was a good inspiration, especially mono's SqlTypes implementation.
Well, nmake and emacs with C# extensions by Brad Merrill are great but I have to admit that 
Visual Studio .NET did his job well.

Without all these peoples and tools NullableTypes software quality would not be the same.


8. SUPPORT
=============
You can submit an help request here:
http://sourceforge.net/forum/forum.php?forum_id=264057

You can submit a feature request here:
http://sourceforge.net/tracker/?atid=549983&group_id=77359

You can submit a bug report here:
http://sourceforge.net/tracker/?atid=549980&group_id=77359


Visit project home-page for more details: http://nullabletypes.sourceforge.net 


---
If you use CodeSmith (http://www.ericjsmith.com/codesmith) you may be interested in these templates:

* Use  NullableTypes in a C# stored procedure wrapper 
  http://www.ericjsmith.net/codesmith/forum/?f=9&m=1622 
  Author: Oskar Austegard

* Tip/Trick
  http://www.ericjsmith.net/codesmith/forum/?f=11&m=1270 
  Author: Damien Guard 

Let me know if you find more.



---
I colleghi italiani potranno trovarmi nel forum di UGIdotNET, lo User Group Italiano dot NET, all'indirizzo
http://www.ugidotnet.org/ e quindi comunicare direttamente in italiano.
Inoltre possono accedere alle seguenti risorse in italiano:

* Articolo: "Come convivere felicemente con i NULL: i NullableTypes"
  http://www.visualcsharp.it/articoli/NullableTypes/NullableTypes.asp

* Articolo: "Le lezioni imparate in un progetto impegnativo: NullableTypes internals"
  http://www.visualcsharp.it/articoli/NullableTypes/NullableTypesInternals.asp

* Workshop UGIdotNET patrocinato da Microsoft Italia
  "Creare nuovi data type con il C#: i NullableTypes"
  Diapositive: http://www.ugidotnet.org/downloads/workshop/2003-09-17/NullableTypes.zip
  Audio/Video: http://www.ugidotnet.org/downloads/workshop/2003-09-17/agile%203.wmv
---

Regards, 
(luKa) 
NullableTypes Project Manager

