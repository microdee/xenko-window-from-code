example code for creating a xenko game on windows without the editor

you need to install xenko and its visual studio extension. after that restore nuget packages and manually copy files from [user]\.nuget\packages\xenko\3.[version]\Bin\Windows to build directory (bin\x64|x86). Copy SDL dll's into the build dir too