
#ifndef MY_INTERFACE_HPP
#define MY_INTERFACE_HPP

#include <windows.h>
#include <winerror.h>

class Interface : public IUnknown {
public:
    virtual float CalculateFunction(float x) = 0;
	virtual float SetA(float value) = 0;
	virtual float SetB(float value) = 0;
	virtual float SetC(float value) = 0;
	virtual unsigned Test(wchar_t* test) = 0;
};

#endif

