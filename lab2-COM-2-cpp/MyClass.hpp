
#ifndef MY_CLASS_HPP
#define MY_CLASS_HPP

#include "MyModule.hpp"

#include <atomic>

class MyClass : public Interface {
public:
	
	MyClass();
	~MyClass();
	
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID id, void **ptr) override;
	virtual ULONG STDMETHODCALLTYPE AddRef() override;
	virtual ULONG STDMETHODCALLTYPE Release() override;
	
    virtual HRESULT STDMETHODCALLTYPE Push(int val) override;
    virtual HRESULT STDMETHODCALLTYPE Pop(int* val) override;
    virtual HRESULT STDMETHODCALLTYPE Top(int* val) override;
	
private:
	
	inline const static int capacity = 1024;
	int arr[capacity];
	int size;
	std::atomic<uint64_t> counter;
};

class MyClassFactory : public IClassFactory {
public:
	MyClassFactory();
	~MyClassFactory();
	static uint64_t GetGlobalCounter();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID id, void **ptr);
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE LockServer(BOOL v);
	virtual HRESULT STDMETHODCALLTYPE CreateInstance(IUnknown *outer, REFIID id, void **ptr);
private:
	std::atomic<uint64_t> counter;
};




#endif

