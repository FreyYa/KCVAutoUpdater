mkdir dist
cd dist

copy ..\..\AutoUpdater\bin\Release\AutoUpdater.exe . /Y
copy ..\..\AutoUpdater\bin\Release\*.dll . /Y	
..\libz.exe inject-dll --assembly AutoUpdater.exe --include *.dll --move