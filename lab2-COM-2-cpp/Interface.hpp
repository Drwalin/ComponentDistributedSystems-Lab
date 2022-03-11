
#ifndef MY_INTERFACE_HPP
#define MY_INTERFACE_HPP

#include <windows.h>
#include <winerror.h>

class Interface : public IUnknown {
public:
    virtual HRESULT STDMETHODCALLTYPE Push(int val) = 0;
    virtual HRESULT STDMETHODCALLTYPE Pop(int* val) = 0;
    virtual HRESULT STDMETHODCALLTYPE Top(int* val) = 0;
};

#endif

