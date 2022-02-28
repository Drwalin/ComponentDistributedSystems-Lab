
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
	printf("\n running\n");
	fflush(stdout);
	
	if(CHECK(CoInitializeEx(NULL, COINIT_APARTMENTTHREADED))) {
		Interface* object;
		if(CHECK(CoCreateInstance(myclassguid, NULL, CLSCTX_INPROC_SERVER,
						interfaceguid, (void**)&object))) {
				object->SetA(2);
				object->SetB(-2);
				object->SetC(-100);
				float y = object->CalculateFunction(5);
				printf(" Expected: %f, received: %f\n", 2*5*5-2*5-100, y);
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

