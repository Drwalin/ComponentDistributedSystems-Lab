
LIBS = -lole32 -luuid

CFLAGS = -ggdb3

GUID1={00000000-3141-5926-5358-979323846264}
GUID2={00000001-3141-5926-5358-979323846264}
#GUID={31415926-5358-9793-2384-626433832795}

REGADDR1=HKEY_CLASSES_ROOT\WOW6432Node\CLSID\$(GUID1)\InProcServer32
REGADDR2=HKEY_CLASSES_ROOT\WOW6432Node\CLSID\$(GUID2)\InProcServer32

all: dll exe

dll:# MyClass.cpp MyModule.cpp MyClass.hpp MyModule.hpp Interface.hpp
	g++ -shared MyClass.cpp MyModule.cpp -o myclass.dll $(LIBS) $(CFLAGS)

exe:# Main.cpp Interface.hpp MyModule.hpp MyModule.cpp
	g++ Main.cpp MyModule.cpp -o main.exe $(LIBS) $(CFLAGS)

#installreg:
#	echo REGEDIT4 > reg.reg
#	echo [$(REGADDR1)] >> reg.reg
##	copy reg_base.reg reg.reg
#	echo @="%cd%" >> reg.reg
#	echo "ThreadingModel"="Both" >> reg.reg
#	echo [$(REGADDR2)] >> reg.reg
#	echo @="%cd%" >> reg.reg
#	echo "ThreadingModel"="Both" >> reg.reg
#
#uninstallreg:
#	echo REGEDIT4 > reg.reg
#	echo [-$(REGADDR1)] >> reg.reg
#	echo [-$(REGADDR2)] >> reg.reg


