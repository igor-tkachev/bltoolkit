cd ..\Source
call Compile3.bat
call Compile4.bat

cd ..\DataProviders
call Compile3.bat
call Compile4.bat

cd ..\NuGet

del *.nupkg

NuGet Pack BLToolkit.nuspec
rename BLToolkit.4.*.nupkg BLToolkit.nupkg

NuGet Pack BLToolkit.symbols.nuspec -Symbols
rename BLToolkit.4.*.nupkg BLToolkit.symbol.nupkg

NuGet Pack BLToolkit.DB2.nuspec
rename BLToolkit.DB2.*.nupkg BLToolkit.DB2.nupkg

NuGet Pack BLToolkit.Firebird.nuspec
rename BLToolkit.Firebird.*.nupkg BLToolkit.Firebird.nupkg

NuGet Pack BLToolkit.Informix.nuspec
rename BLToolkit.Informix.*.nupkg BLToolkit.Informix.nupkg

NuGet Pack BLToolkit.MySql.nuspec
rename BLToolkit.MySql.*.nupkg BLToolkit.MySql.nupkg

NuGet Pack BLToolkit.Oracle.nuspec
rename BLToolkit.Oracle.*.nupkg BLToolkit.Oracle.nupkg

NuGet Pack BLToolkit.PostgreSql.nuspec 
rename BLToolkit.PostgreSql.*.nupkg BLToolkit.PostgreSql.nupkg

NuGet Pack BLToolkit.SqlCe.nuspec 
rename BLToolkit.SqlCe.*.nupkg BLToolkit.SqlCe.nupkg

NuGet Pack BLToolkit.SQLite.nuspec 
rename BLToolkit.SQLite.*.nupkg BLToolkit.SQLite.nupkg

NuGet Pack BLToolkit.Sybase.nuspec 
rename BLToolkit.Sybase.*.nupkg BLToolkit.Sybase.nupkg
