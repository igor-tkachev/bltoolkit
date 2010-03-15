%windir%\Microsoft.NET\Framework\v4.0.30128\MSBuild.exe /target:Clean BLToolkit.3.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v4.0.30128\MSBuild.exe /target:Clean BLToolkit.3.csproj /property:Configuration=Release
%windir%\Microsoft.NET\Framework\v4.0.30128\MSBuild.exe BLToolkit.4.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v4.0.30128\MSBuild.exe BLToolkit.4.csproj /property:Configuration=Release
pause