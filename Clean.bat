if exist "%cd%\MiniProto\Release" (rd /s /q "%cd%\MiniProto\Release")
if exist "%cd%\MiniProto\Debug" (rd /s /q "%cd%\MiniProto\Debug")
if exist "%cd%\MiniProtoCppLib\Release" (rd /s /q "%cd%\MiniProtoCppLib\Release")
if exist "%cd%\MiniProtoCppLib\Debug" (rd /s /q "%cd%\MiniProtoCppLib\Debug")
if exist "%cd%\MiniProtoCppTest\Release" (rd /s /q "%cd%\MiniProtoCppTest\Release")
if exist "%cd%\MiniProtoCppTest\Debug" (rd /s /q "%cd%\MiniProtoCppTest\Debug")
if exist "%cd%\MiniProtoCSharpDll\bin" (rd /s /q "%cd%\MiniProtoCSharpDll\bin")
if exist "%cd%\MiniProtoCSharpDll\obj" (rd /s /q "%cd%\MiniProtoCSharpDll\obj")
if exist "%cd%\MiniProtoCSharpTest\bin" (rd /s /q "%cd%\MiniProtoCSharpTest\bin")
if exist "%cd%\MiniProtoCSharpTest\obj" (rd /s /q "%cd%\MiniProtoCSharpTest\obj")
if exist "%cd%\MiniProtoJavaJar\bin" (rd /s /q "%cd%\MiniProtoJavaJar\bin")
if exist "%cd%\MiniProtoJavaTest\bin" (rd /s /q "%cd%\MiniProtoJavaTest\bin")

del /s /q "%cd%\Release\*.pdb"
del /s /q "%cd%\Release\*.exe"
del /s /q "%cd%\Release\*.config"
del /s /q "%cd%\Release\*.manifest"
del /s /q "%cd%\Release\*.txt"
del /s /q "%cd%\Release\*.jar"
del /s /q "%cd%\Release\*.lib"
del /s /q "%cd%\Release\*.dll"
pause