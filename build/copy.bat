DEL /F /S /q ..\src\RazorRockstars.SelfHost\img\*
@rmdir /S /Q ..\src\RazorRockstars.SelfHost\img
MD ..\src\RazorRockstars.SelfHost\img

DEL /F /S /q ..\src\RazorRockstars.SelfHost\js\*
@rmdir /S /Q ..\src\RazorRockstars.SelfHost\js
MD ..\src\RazorRockstars.SelfHost\js

DEL /F /S /q ..\src\RazorRockstars.SelfHost\stars\*
@rmdir /S /Q ..\src\RazorRockstars.SelfHost\stars
MD ..\src\RazorRockstars.SelfHost\stars

DEL /F /S /q ..\src\RazorRockstars.SelfHost\Views\*
@rmdir /S /Q ..\src\RazorRockstars.SelfHost\Views
MD ..\src\RazorRockstars.SelfHost\Views


COPY ..\src\RazorRockstars.WebHost\*.css ..\src\RazorRockstars.SelfHost\
COPY ..\src\RazorRockstars.WebHost\*.cshtml ..\src\RazorRockstars.SelfHost\
COPY ..\src\RazorRockstars.WebHost\RockstarsService.cs ..\src\RazorRockstars.SelfHost\
XCOPY  /S /E ..\src\RazorRockstars.WebHost\img ..\src\RazorRockstars.SelfHost\img
XCOPY  /S /E ..\src\RazorRockstars.WebHost\js ..\src\RazorRockstars.SelfHost\js
XCOPY  /S /E ..\src\RazorRockstars.WebHost\stars ..\src\RazorRockstars.SelfHost\stars
XCOPY  /S /E ..\src\RazorRockstars.WebHost\Views ..\src\RazorRockstars.SelfHost\Views


DEL /F /S /q ..\src\RazorRockstars.WinService\img\*
@rmdir /S /Q ..\src\RazorRockstars.WinService\img
MD ..\src\RazorRockstars.WinService\img

DEL /F /S /q ..\src\RazorRockstars.WinService\js\*
@rmdir /S /Q ..\src\RazorRockstars.WinService\js
MD ..\src\RazorRockstars.WinService\js

DEL /F /S /q ..\src\RazorRockstars.WinService\stars\*
@rmdir /S /Q ..\src\RazorRockstars.WinService\stars
MD ..\src\RazorRockstars.WinService\stars

DEL /F /S /q ..\src\RazorRockstars.WinService\Views\*
@rmdir /S /Q ..\src\RazorRockstars.WinService\Views
MD ..\src\RazorRockstars.WinService\Views


COPY ..\src\RazorRockstars.WebHost\*.css ..\src\RazorRockstars.WinService\
COPY ..\src\RazorRockstars.WebHost\*.cshtml ..\src\RazorRockstars.WinService\
COPY ..\src\RazorRockstars.WebHost\RockstarsService.cs ..\src\RazorRockstars.WinService\
XCOPY  /S /E ..\src\RazorRockstars.WebHost\img ..\src\RazorRockstars.WinService\img
XCOPY  /S /E ..\src\RazorRockstars.WebHost\js ..\src\RazorRockstars.WinService\js
XCOPY  /S /E ..\src\RazorRockstars.WebHost\stars ..\src\RazorRockstars.WinService\stars
XCOPY  /S /E ..\src\RazorRockstars.WebHost\Views ..\src\RazorRockstars.WinService\Views



DEL /F /S /q ..\src\RazorRockstars.CompiledViews\img\*
@rmdir /S /Q ..\src\RazorRockstars.CompiledViews\img
MD ..\src\RazorRockstars.CompiledViews\img

DEL /F /S /q ..\src\RazorRockstars.CompiledViews\js\*
@rmdir /S /Q ..\src\RazorRockstars.CompiledViews\js
MD ..\src\RazorRockstars.CompiledViews\js

DEL /F /S /q ..\src\RazorRockstars.CompiledViews\stars\*
@rmdir /S /Q ..\src\RazorRockstars.CompiledViews\stars
MD ..\src\RazorRockstars.CompiledViews\stars

DEL /F /S /q ..\src\RazorRockstars.CompiledViews\Views\*
@rmdir /S /Q ..\src\RazorRockstars.CompiledViews\Views
MD ..\src\RazorRockstars.CompiledViews\Views


REM COPY ..\src\RazorRockstars.WebHost\Web.config ..\src\RazorRockstars.CompiledViews\
COPY ..\src\RazorRockstars.WebHost\*.css ..\src\RazorRockstars.CompiledViews\
COPY ..\src\RazorRockstars.WebHost\*.cshtml ..\src\RazorRockstars.CompiledViews\
COPY ..\src\RazorRockstars.WebHost\RockstarsService.cs ..\src\RazorRockstars.CompiledViews\
XCOPY  /S /E ..\src\RazorRockstars.WebHost\img ..\src\RazorRockstars.CompiledViews\img
XCOPY  /S /E ..\src\RazorRockstars.WebHost\js ..\src\RazorRockstars.CompiledViews\js
XCOPY  /S /E ..\src\RazorRockstars.WebHost\stars ..\src\RazorRockstars.CompiledViews\stars
XCOPY  /S /E ..\src\RazorRockstars.WebHost\Views ..\src\RazorRockstars.CompiledViews\Views
