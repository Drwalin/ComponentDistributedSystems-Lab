
#include "MyClass.hpp"

#include <fstream>


template<typename T>
void Debug(const char*func, const char*fname, int line, T args, const char*n="\n") {
	std::ofstream file("C:\\Studies\\sem6\\ksr\\lab\\lab1-COM-1-cpp\\log.log",
			std::ios::out | std::ios::app);
	file << func << "   \t: " << line;
	// file << "  in  " << fname << " : " << line;
	file << "   " << args;
	file << n;
}

std::string F(const char* s) {
	std::string r = s, a, b;
	r[r.find('(')] = 0;
	b = r.c_str();
	a = b.c_str() + b.rfind(' ');
	return a.c_str() + a.find_first_of("_qwertyuiopasdfghjklmnbvcxzQWERTYUIOPLKJHGFDSAZXCVBNM");
}

// #define DEBUG(X)
#define DEBUG(X) Debug(F(__PRETTY_FUNCTION__).c_str(), __FILE__, __LINE__, X)


std::atomic<uint64_t> global_counter;

MyClass::MyClass() {
	DEBUG(this);
	size = 0;
}

MyClass::~MyClass() {
	DEBUG(this);
}

HRESULT STDMETHODCALLTYPE MyClass::Push(int val) {
	if(size>=0 && size<capacity) {
		arr[size] = val;
		++size;
	}
	return S_OK;
}

HRESULT STDMETHODCALLTYPE MyClass::Pop(int* val) {
	if(size>0 && size<=capacity) {
		size--;
		*val = arr[size];
	}
	return S_OK;
	
}

HRESULT STDMETHODCALLTYPE MyClass::Top(int* val) {
	if(size>0 && size<=capacity) {
		*val = arr[size-1];
	}
	return S_OK;
}


HRESULT STDMETHODCALLTYPE MyClass::QueryInterface(REFIID id, void **ptr) {
	DEBUG("");
	if(ptr == NULL)
		return E_POINTER;
	*ptr = NULL;
	
	if(id == IID_IUnknown)
		*ptr = this;
	else if(id == interfaceguid)
		*ptr = this;
	
	if(*ptr != NULL) {
		AddRef();
		return S_OK;
	}
	return E_NOINTERFACE;
	
}

ULONG STDMETHODCALLTYPE   MyClass::AddRef() {
	DEBUG("");
	return ++counter;
}

ULONG STDMETHODCALLTYPE   MyClass::Release() {
	DEBUG("");
	uint64_t ret = --counter;
	if(ret == 0)
		delete this;
	return ret;
}



 
MyClassFactory::MyClassFactory() {
	DEBUG("");
	counter = 0;
}

MyClassFactory::~MyClassFactory() {
	DEBUG("");
}

HRESULT STDMETHODCALLTYPE MyClassFactory::QueryInterface(REFIID id, void **ptr) {
	DEBUG("");
	if(ptr == NULL)
		return E_POINTER;
	*ptr = NULL;
	
	if(id == IID_IUnknown)
		*ptr = this;
	else if(id == IID_IClassFactory)
		*ptr = this;
	
	if(*ptr != NULL) {
		AddRef();
		return S_OK;
	}
	return E_NOINTERFACE;
}

uint64_t MyClassFactory::GetGlobalCounter() {
	return global_counter;
}

ULONG   STDMETHODCALLTYPE MyClassFactory::AddRef() {
	DEBUG("");
	return ++counter;
}

ULONG   STDMETHODCALLTYPE MyClassFactory::Release() {
	DEBUG("");
	uint64_t ret = --counter;
	if(ret == 0)
		delete this;
	return ret;
}

HRESULT STDMETHODCALLTYPE MyClassFactory::LockServer(BOOL v) {
	DEBUG("");
	global_counter += (v ? 1 : -1);
	return S_OK;
}

HRESULT STDMETHODCALLTYPE MyClassFactory::CreateInstance(IUnknown *outer, REFIID id, void **ptr) {
	DEBUG("");
	if(ptr == NULL)
		return E_POINTER;
	*ptr = NULL;
	if(id != IID_IUnknown && id != interfaceguid)
		return E_NOINTERFACE;

	MyClass* object = new MyClass();
	if(object == NULL)
		return E_OUTOFMEMORY;

	HRESULT res = object->QueryInterface(id, ptr);
	if(FAILED(res)) {
		DEBUG("");
		delete object;
		*ptr = NULL;
	}
	return res;	
}










extern "C" HRESULT __stdcall DllGetClassObject(REFCLSID cls, REFIID iid, void **ptr) {
	DEBUG("");
	if(ptr == NULL) {
		DEBUG("");
		return E_INVALIDARG;
	}
	*ptr = NULL;
	DEBUG("");

	DEBUG("");
	if(cls != myclassguid) {
		DEBUG("");
		return CLASS_E_CLASSNOTAVAILABLE;
	}

	DEBUG("");
	if(iid != IID_IUnknown && iid != IID_IClassFactory) {
		DEBUG("");
		return E_NOINTERFACE;
	}

	DEBUG("");
	MyClassFactory *factory = new MyClassFactory();
	DEBUG("");
	if(factory == NULL) {
		DEBUG("");
		return E_OUTOFMEMORY;
	}
	DEBUG("");

	HRESULT res = factory->QueryInterface(iid, ptr);
	DEBUG("");
	if(FAILED(res)) {
		DEBUG("");
		delete factory;
		*ptr = NULL;
	};
	DEBUG("");
	return res;
};


extern "C" HRESULT __stdcall DllCanUnloadNow() {
	DEBUG("");
	return global_counter > 0 ? S_FALSE : S_OK;
};

extern "C" BOOL WINAPI DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
	DEBUG("");
	switch (ul_reason_for_call) {
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	};
	return TRUE;
};
