
#ifndef MY_CLASS_HPP
#define MY_CLASS_HPP

#include "MyModule.hpp"

#include <atomic>

class MyClass : public Interface {
public:
	
	MyClass();
	~MyClass();
	
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID id, void **ptr);
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	
    float CalculateFunction(float x);
	float SetA(float value);
	float SetB(float value);
	float SetC(float value);
	
private:
	
	float a, b, c;
	std::atomic<uint64_t> counter;
};

class MyClassFactory : public IClassFactory {
public:
	MyClassFactory();
	~MyClassFactory();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID id, void **ptr);
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE LockServer(BOOL v);
	virtual HRESULT STDMETHODCALLTYPE CreateInstance(IUnknown *outer, REFIID id, void **ptr);
private:
	std::atomic<uint64_t> counter;
};




#endif

