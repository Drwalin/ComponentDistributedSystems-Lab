
LIBS = -lole32 -luuid

CFLAGS = -m32 -ggdb3

GUID1={00000000-3141-5926-5358-979323846264}
GUID2={00000001-3141-5926-5358-979323846264}
#GUID={31415926-5358-9793-2384-626433832795}

REGADDR1=HKEY_CLASSES_ROOT\WOW6432Node\CLSID\$(GUID1)\InProcServer32
REGADDR2=HKEY_CLASSES_ROOT\WOW6432Node\CLSID\$(GUID2)\InProcServer32

CC=g++

all: dll exe

dll:# MyClass.cpp MyModule.cpp MyClass.hpp MyModule.hpp Interface.hpp
	$(CC) -shared MyClass.cpp MyModule.cpp -o myclass.dll $(LIBS) $(CFLAGS) -Wl,--kill-at

exe:# Main.cpp Interface.hpp MyModule.hpp MyModule.cpp
	$(CC) Main.cpp MyModule.cpp -o main.exe $(LIBS) $(CFLAGS)

