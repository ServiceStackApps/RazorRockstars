CD RazorRockstars.SelfHost

DEL /F /S /q img\*
DEL /F /S /q js\*
DEL /F /S /q stars\*
DEL /F /S /q Views\*

MD img
MD js
MD stars
MD Views

XCOPY  /S /E ..\RazorRockstars.WebHost\img img
XCOPY  /S /E ..\RazorRockstars.WebHost\js js
XCOPY  /S /E ..\RazorRockstars.WebHost\stars stars
XCOPY  /S /E ..\RazorRockstars.WebHost\Views Views

COPY ..\RazorRockstars.WebHost\*.css .
COPY ..\RazorRockstars.WebHost\*.cshtml .
COPY ..\RazorRockstars.WebHost\Web.config .
COPY ..\RazorRockstars.WebHost\AppHost.cs .
COPY ..\RazorRockstars.WebHost\RockstarsService.cs .


CD ..\RazorRockstars.WinService

DEL /F /S /q img\*
DEL /F /S /q js\*
DEL /F /S /q stars\*
DEL /F /S /q Views\*

MD img
MD js
MD stars
MD Views

XCOPY  /S /E ..\RazorRockstars.WebHost\img img
XCOPY  /S /E ..\RazorRockstars.WebHost\js js
XCOPY  /S /E ..\RazorRockstars.WebHost\stars stars
XCOPY  /S /E ..\RazorRockstars.WebHost\Views Views

COPY ..\RazorRockstars.WebHost\*.css .
COPY ..\RazorRockstars.WebHost\*.cshtml .
COPY ..\RazorRockstars.WebHost\Web.config .
COPY ..\RazorRockstars.WebHost\AppHost.cs .
COPY ..\RazorRockstars.WebHost\RockstarsService.cs .
