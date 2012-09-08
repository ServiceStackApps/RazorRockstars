PUSHD "..\..\ServiceStack\release\"
CALL copy.bat
POPD
COPY ..\..\ServiceStack\release\latest\ServiceStack\*  .\
COPY ..\..\ServiceStack\release\latest\ServiceStack.OrmLite\Sqlite32.Mono\* .\