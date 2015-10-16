copy ..\bin\release\Glovebox.Graphics.dll lib\uap10.0 /y
del *.nupkg
\data\software\nuget\nuget pack Glovebox.Graphics.dll.nuspec