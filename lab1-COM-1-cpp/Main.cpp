
#include "MyModule.hpp"

#include <cstdio>
#include <iostream>

int main() {
	printf("\n running\n");
	fflush(stdout);
	
	if(!FAILED(CoInitializeEx(NULL, COINIT_APARTMENTTHREADED))) {
		Interface* object;
		if(!FAILED(CoCreateInstance(myclassguid, NULL, CLSCTX_INPROC_SERVER,
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

