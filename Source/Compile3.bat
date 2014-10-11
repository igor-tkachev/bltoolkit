..\Tools\SvnRevision\SvnRevision.exe .. Templates\BLToolkitConstants.Revision.cs.template Properties\Revision.generated.cs

%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Clean BLToolkit.3.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Clean BLToolkit.3.csproj /property:Configuration=Release
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe BLToolkit.3.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe BLToolkit.3.csproj /property:Configuration=Release
pause