
#include <windows.h>

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0500 */
/* at Fri Mar 11 10:23:22 2022
 */
/* Compiler settings for MyClass.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __MyClass_h__
#define __MyClass_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IInterface_FWD_DEFINED__
#define __IInterface_FWD_DEFINED__
typedef interface IInterface IInterface;
#endif 	/* __IInterface_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IInterface_INTERFACE_DEFINED__
#define __IInterface_INTERFACE_DEFINED__

/* interface IInterface */
/* [uuid][object] */ 


EXTERN_C const IID IID_IInterface;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("00000002-3141-5926-5358-979323846264")
    IInterface : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE Push( 
            int value) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Pop( 
            /* [out] */ int *value) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Top( 
            /* [out] */ int *value) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IInterfaceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IInterface * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IInterface * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IInterface * This);
        
        HRESULT ( STDMETHODCALLTYPE *Push )( 
            IInterface * This,
            int value);
        
        HRESULT ( STDMETHODCALLTYPE *Pop )( 
            IInterface * This,
            /* [out] */ int *value);
        
        HRESULT ( STDMETHODCALLTYPE *Top )( 
            IInterface * This,
            /* [out] */ int *value);
        
        END_INTERFACE
    } IInterfaceVtbl;

    interface IInterface
    {
        CONST_VTBL struct IInterfaceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IInterface_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IInterface_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IInterface_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IInterface_Push(This,value)	\
    ( (This)->lpVtbl -> Push(This,value) ) 

#define IInterface_Pop(This,value)	\
    ( (This)->lpVtbl -> Pop(This,value) ) 

#define IInterface_Top(This,value)	\
    ( (This)->lpVtbl -> Top(This,value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IInterface_INTERFACE_DEFINED__ */


/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


