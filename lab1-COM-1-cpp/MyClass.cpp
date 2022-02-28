
#include "MyClass.hpp"


std::atomic<uint64_t> global_counter;

MyClass::MyClass() {
	a = 1;
	b = 0;
	c = 0;
}

MyClass::~MyClass() {
}

float MyClass::CalculateFunction(float x) {
	return a*x*x + b*x + c;
}

float MyClass::SetA(float value) {
	return a = value;
}

float MyClass::SetB(float value) {
	return b = value;
}

float MyClass::SetC(float value) {
	return c = value;
}


HRESULT STDMETHODCALLTYPE MyClass::QueryInterface(REFIID id, void **ptr) {
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
	return ++counter;
}

ULONG STDMETHODCALLTYPE   MyClass::Release() {
	uint64_t ret = --counter;
	if(ret == 0)
		delete this;
	return ret;
}



 
MyClassFactory::MyClassFactory() {
	counter = 0;
}

MyClassFactory::~MyClassFactory() {
}

HRESULT STDMETHODCALLTYPE MyClassFactory::QueryInterface(REFIID id, void **ptr) {
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

ULONG   STDMETHODCALLTYPE MyClassFactory::AddRef() {
	return ++counter;
}

ULONG   STDMETHODCALLTYPE MyClassFactory::Release() {
	uint64_t ret = --counter;
	if(ret == 0)
		delete this;
	return ret;
}

HRESULT STDMETHODCALLTYPE MyClassFactory::LockServer(BOOL v) {
	global_counter += (v ? 1 : -1);
	return S_OK;
}

HRESULT STDMETHODCALLTYPE MyClassFactory::CreateInstance(IUnknown *outer, REFIID id, void **ptr) {
	if(ptr == NULL)
		return E_POINTER;
	*ptr = NULL;
	if(id != IID_IUnknown && id != interfaceguid)
		return E_NOINTERFACE;

	MyClass* object = new MyClass();
	if(object == NULL) return E_OUTOFMEMORY;

	HRESULT res = object->QueryInterface(id, ptr);
	if(FAILED(res)) {
		delete object;
		*ptr = NULL;
	}
	return res;	
}











extern "C" HRESULT __stdcall DllGetClassObject(REFCLSID cls, REFIID iid, void **ptr) {
	if(ptr == NULL)
		return E_INVALIDARG;
	*ptr = NULL;

	if(cls != myclassguid)
		return CLASS_E_CLASSNOTAVAILABLE;
	if(iid != IID_IUnknown && iid != IID_IClassFactory)
		return E_NOINTERFACE;

	MyClassFactory *factory = new MyClassFactory();
	if(factory == NULL)
		return E_OUTOFMEMORY;

	HRESULT res = factory->QueryInterface(iid, ptr);
	if(FAILED(res)) {
		delete factory;
		*ptr = NULL;
	};
	return res;
};


extern "C" HRESULT __stdcall DllCanUnloadNow() {
	return global_counter > 0 ? S_FALSE : S_OK;
};
