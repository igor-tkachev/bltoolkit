c:
cd c:\temp\BLToolkitSnapshot\
del *.zip

rd /S /Q bl-toolkit
md bl-toolkit

"%ProgramW6432\%TortoiseProc.exe" /command:checkout /path:"c:\temp\BLToolkitSnapshot" /url:"http://bl-toolkit.googlecode.com/svn/trunk" /closeonend:1

cd bl-toolkit

"%ProgramFiles%\WinRAR\WinRar.exe" a -m5 -md1024 -s -r -rr -AFzip -x*\_svn\* c:\temp\BLToolkitSnapshot\bltoolkit_dev *.*
"%ProgramFiles%\WinRAR\WinRar.exe" a -m5 -md1024 -s -r -rr -AFzip -x*\_svn\* c:\temp\BLToolkitSnapshot\bltoolkit Source\*.*

cd Source
%windir%\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe BLToolkit.2.csproj /property:Configuration=Release 
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe BLToolkit.3.csproj /property:Configuration=Release 
copy bin\Release\*.dll .

cd ..\Tools\BLTgen
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe BLTgen.csproj /property:Configuration=Release 
copy bin\Release\*.exe ..\..\Source
cd ..\..\Source

"%ProgramFiles%\WinRAR\WinRar.exe" a -m5 -md1024 -s -r -rr -AFzip -x*\_svn\* -x*\bin\* -x*\obj\* c:\temp\BLToolkitSnapshot\bltoolkit_bin *.exe *.dll Data\DataProvider\

ftp -s:e:\documents\copybltsnapshot.txt ftp.bltoolkit.net