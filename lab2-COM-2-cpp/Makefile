

LIBS = -lole32 -luuid -loleaut32
CFLAGS = -m32 -ggdb3
FLAGS = $(LIBS) $(CFLAGS)
CXX = g++

# reg:
# [HKEY_CURRENT_USER\Wow6432Node\CLSID\{AF4D385C-05D6-49AC-A6D2-EC5CF12673BB}]

all:
	client server

client:
	$(CXX) -o client.exe client.cpp $(FLAGS)

server:
	$(CXX) -o server.exe Server.cpp $(FLAGS)

proxysource:
	"C:\Program Files (x86)\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
	(cd proxy_dir && midl MyClass.idl)

proxy:
	"C:\Program Files (x86)\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
	(cd proxy_dir && cl /OUT:proxy.dll /LD MyClass_i.c MyClass_p.c dlldata.c MyClass.def rpcns4.lib rpcrt4.lib uuid.lib /D"PROXY_CLSID_IS={0x00000002,0x3141,0x5926,{ 0x53, 0x58, 0x97, 0x93, 0x23, 0x84, 0x62, 0x64 }}" /DREGISTER_PROXY_DLL)
	cp proxy_dir\MyClass_i.dll proxy_stub.dll


