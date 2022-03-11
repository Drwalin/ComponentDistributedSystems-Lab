
#include "MyModule.hpp"

#include <cstdio>
#include <iostream>

bool CHECK(HRESULT res) {
	if(res == S_OK)
		return true;
	printf(" hresult: %llX\n", res);
	return false;
}

int main() {
	if(CHECK(CoInitializeEx(NULL, COINIT_APARTMENTTHREADED))) {
		Interface* object;
		if(CHECK(CoCreateInstance(myclassguid, NULL, CLSCTX_LOCAL_SERVER,
						interfaceguid, (void**)&object))) {
			int val = 311;
			object->Push(val);
			int a = 0;
			object->Pop(&a);
			printf(" Expected: %i, received: %i\n", val, a);
			a = 0;
			object->Pop(&a);
			printf(" Expected: ???, received: %i\n", a);
			object->Release();
		} else {
			printf(" err 1\n");
		}
		CoUninitialize();
	} else {
		printf(" err 2\n");
	}
	
	printf("\n");
	return 0;
}

#include "MyModule.cpp"
