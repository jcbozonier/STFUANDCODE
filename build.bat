mkdir out
set SRC=src\bin\Release\
%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe src\STFUANDCODE.sln /p:Configuration=Release
copy %SRC%STFUANDCODE.exe out
copy %SRC%ICSharpCode.AvalonEdit.dll out
copy %SRC%ICSharpCode.NRefactory.dll out
pause