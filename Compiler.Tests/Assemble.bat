call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" x86_amd64

ml64 %1 /link /subsystem:console /defaultlib:"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Lib\x64\Kernel32.Lib" /entry:main
