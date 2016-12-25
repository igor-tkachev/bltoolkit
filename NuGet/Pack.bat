cd ..
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe BLToolkit.2012.sln /property:Configuration=Release 

cd Source
call Compile3.bat
call Compile4.bat

cd ..\NuGet

del *.nupkg

..\Redist\NuGet Pack BLToolkit.nuspec
rename BLToolkit.4.*.nupkg BLToolkit.nupkg

..\Redist\NuGet Pack BLToolkit.symbols.nuspec -Symbols
rename BLToolkit.4.*.nupkg BLToolkit.symbol.nupkg

..\Redist\NuGet Pack BLToolkit.DB2.nuspec
rename BLToolkit.DB2.*.nupkg BLToolkit.DB2.nupkg

..\Redist\NuGet Pack BLToolkit.Firebird.nuspec
rename BLToolkit.Firebird.*.nupkg BLToolkit.Firebird.nupkg

..\Redist\NuGet Pack BLToolkit.Informix.nuspec
rename BLToolkit.Informix.*.nupkg BLToolkit.Informix.nupkg

..\Redist\NuGet Pack BLToolkit.MySql.nuspec
rename BLToolkit.MySql.*.nupkg BLToolkit.MySql.nupkg

..\Redist\NuGet Pack BLToolkit.Oracle.nuspec
rename BLToolkit.Oracle.*.nupkg BLToolkit.Oracle.nupkg

..\Redist\NuGet Pack BLToolkit.Oracle.Managed.nuspec
rename BLToolkit.Oracle.Managed.*.nupkg BLToolkit.Oracle.Managed.nupkg

..\Redist\NuGet Pack BLToolkit.PostgreSql.nuspec 
rename BLToolkit.PostgreSql.*.nupkg BLToolkit.PostgreSql.nupkg

..\Redist\NuGet Pack BLToolkit.SqlCe.nuspec 
rename BLToolkit.SqlCe.*.nupkg BLToolkit.SqlCe.nupkg

..\Redist\NuGet Pack BLToolkit.SQLite.nuspec 
rename BLToolkit.SQLite.*.nupkg BLToolkit.SQLite.nupkg

..\Redist\NuGet Pack BLToolkit.Sybase.nuspec 
rename BLToolkit.Sybase.*.nupkg BLToolkit.Sybase.nupkg
