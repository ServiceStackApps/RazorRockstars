SET ILMERGE=C:\tools\ILMerge\ILMerge.exe
SET RELEASE=..\src\RazorRockstars.CompiledViews.WpfHost\bin\Release
SET INPUT=%RELEASE%\RazorRockstars.CompiledViews.WpfHost.exe
SET INPUT=%INPUT% %RELEASE%\RazorRockstars.CompiledViews.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.Text.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.Client.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.Common.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.Interfaces.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.OrmLite.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.OrmLite.Sqlite.dll
SET INPUT=%INPUT% %RELEASE%\Mono.Data.Sqlite.dll
SET INPUT=%INPUT% %RELEASE%\ServiceStack.Razor.dll
SET INPUT=%INPUT% %RELEASE%\System.Web.Razor.dll

%ILMERGE% /target:exe /targetplatform:v4,"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /out:WpfRazorRockstars.exe /ndebug %INPUT% 
