
#include <windows.h>
#include "MyClass.hpp"
#include "MyModule.hpp"

#include <cstdio>
#include <iostream>
#include <cstring>

bool CHECK(HRESULT res) {
	if(res == S_OK)
		return true;
	printf(" hresult: %llX\n", res);
	return false;
}

int main(int argc, const char **argv) {
	if(CHECK(CoInitializeEx(NULL, COINIT_MULTITHREADED))) {
		if(argc < 2)
			return 0;
		for(int i=0; i<argc; ++i) {
			if(strcmp(argv[i], "/Embedding") == 0
					|| strcmp(argv[i], "-Embedding") == 0) {
				MyClassFactory* f = new MyClassFactory();
				f->AddRef();
				DWORD id;
				HRESULT rv = CoRegisterClassObject(
						myclassguid, 
						(IUnknown*)f,
						CLSCTX_LOCAL_SERVER, REGCLS_MULTIPLEUSE, &id
						);

				if(FAILED(rv)) {
					CoUninitialize();
					return 0;
				}
				Sleep(5000);

				do {
					Sleep(1000);
					printf(" global counter: %llu\n", MyClassFactory::GetGlobalCounter());
				} while(MyClassFactory::GetGlobalCounter() > 1);
				
				CoRevokeClassObject(id);
				f->Release();
			}
		}
		CoUninitialize();
	}
}

#include "MyClass.cpp"
#include "MyModule.cpp"
