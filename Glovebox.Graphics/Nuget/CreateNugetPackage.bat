rem automate nuget process

mkdir lib\uap10.0
copy ..\bin\release\Glovebox.Graphics.dll lib\uap10.0 /y
del *.nupkg
\data\software\nuget\nuget pack Glovebox.Graphics.dll.nuspec
del lib\uap10.0\*.* /q
rmdir lib /s /q

rem \data\software\nuget\nuget setApiKey your key
rem \data\software\nuget\nuget push Glovebox.Graphics.1.0.7.nupkg

