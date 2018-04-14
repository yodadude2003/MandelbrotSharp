// Generated by Hybridizer version 1.0.0.0
 #include "cuda_runtime.h"                                                                                 
 #include "device_launch_parameters.h"                                                                     
                                                                                                             
 #if defined(__CUDACC__)                                                                                     
 	#ifndef hyb_device                                                                                       
 		#define hyb_inline __forceinline__                                                                   
 		                                                                                                     
 		#define hyb_constant __constant__                                                                    
 		#if defined(HYBRIDIZER_NO_HOST)                                                                      
 			#define hyb_host                                                                                 
 			#define	hyb_device  __device__                                                                   
 		#else                                                                                                
 			#define hyb_host __host__                                                                        
 			#define	hyb_device  __device__                                                                   
 		#endif                                                                                               
 	#endif                                                                                                   
 #else                                                                                                        
 	#ifndef hyb_device                                                                                       
 		#define hyb_inline inline                                                                            
 		#define hyb_device                                                                                   
 		#define hyb_constant                                                                                 
 	#endif                                                                                                   
 #endif                                                                                                       
                                                                                                              
 #pragma once                                                                                                 
 #if defined _WIN32 || defined _WIN64 || defined __CYGWIN__                                                   
   #define BUILDING_DLL                                                                                       
   #ifdef BUILDING_DLL                                                                                        
     #ifdef __GNUC__                                                                                          
       #define DLL_PUBLIC __attribute__ ((dllexport))                                                         
     #else                                                                                                    
       #define DLL_PUBLIC __declspec(dllexport) // Note: actually gcc seems to also supports this syntax.     
     #endif                                                                                                   
   #else                                                                                                      
     #ifdef __GNUC__                                                                                          
       #define DLL_PUBLIC __attribute__ ((dllimport))                                                         
     #else                                                                                                    
       #define DLL_PUBLIC __declspec(dllimport) // Note: actually gcc seems to also supports this syntax.     
     #endif                                                                                                   
   #endif                                                                                                     
   #define DLL_LOCAL                                                                                          
 #else                                                                                                        
   #if __GNUC__ >= 4                                                                                          
     #define DLL_PUBLIC __attribute__ ((visibility ("default")))                                            
     #define DLL_LOCAL  __attribute__ ((visibility ("hidden")))                                             
   #else                                                                                                      
     #define DLL_PUBLIC                                                                                       
     #define DLL_LOCAL                                                                                        
   #endif                                                                                                     
 #endif                                                                                                       


// hybridizer core types
#include <cstdint>
namespace hybridizer { struct hybridobject ; }
namespace hybridizer { struct runtime ; }

#pragma region defined enums and types
#if defined(__cplusplus) || defined(__CUDACC__)
namespace MandelBrot { 
struct GPUFractal ;
} // Leaving namespace
namespace System { 
struct Math ;
} // Leaving namespace
#endif // TOTO
#pragma endregion

extern "C" void* __hybridizer_init_basic_runtime();
#include <cstdio>
// generating GetTypeID function
#include <cstring> // for strcmp
extern "C" DLL_PUBLIC int HybridizerGetTypeID( const char* fullTypeName)
{
	if (strcmp (fullTypeName, "Hybridizer.Runtime.CUDAImports.blockDim") == 0) return 1000000 ; 
	if (strcmp (fullTypeName, "Hybridizer.Runtime.CUDAImports.blockIdx") == 0) return 1000001 ; 
	if (strcmp (fullTypeName, "Hybridizer.Runtime.CUDAImports.gridDim") == 0) return 1000002 ; 
	if (strcmp (fullTypeName, "Hybridizer.Runtime.CUDAImports.threadIdx") == 0) return 1000003 ; 
	if (strcmp (fullTypeName, "MandelBrot.GPUFractal") == 0) return 1000004 ; 
	if (strcmp (fullTypeName, "System.Math") == 0) return 1000005 ; 
	return 0 ;
}
extern "C" DLL_PUBLIC const char* HybridizerGetTypeFromID( const int typeId)
{
	if (typeId == 1000000) return "Hybridizer.Runtime.CUDAImports.blockDim" ; 
	if (typeId == 1000001) return "Hybridizer.Runtime.CUDAImports.blockIdx" ; 
	if (typeId == 1000002) return "Hybridizer.Runtime.CUDAImports.gridDim" ; 
	if (typeId == 1000003) return "Hybridizer.Runtime.CUDAImports.threadIdx" ; 
	if (typeId == 1000004) return "MandelBrot.GPUFractal" ; 
	if (typeId == 1000005) return "System.Math" ; 
	return "" ;
}
extern "C" DLL_PUBLIC int HybridizerGetShallowSize (const char* fullTypeName) 
{
	#ifdef __TYPE_DECL__MandelBrot_GPUFractal___
	if (strcmp (fullTypeName, "MandelBrot.GPUFractal") == 0) return 8 ; 
	#endif
	return 0 ;
}

// Get various Hybridizer properties at runtime
struct __hybridizer_properties {
    int32_t UseHybridArrays;
    int32_t Flavor;
    int32_t CompatibilityMode;
    int32_t _dummy;
};
extern "C" DLL_PUBLIC __hybridizer_properties __HybridizerGetProperties () {
    __hybridizer_properties res;
    res.UseHybridArrays = 0;
    res.Flavor = 1;
    res.CompatibilityMode = 0;
    return res ;
}
#include <cuda.h>                                     
 struct HybridModule                                  
 {                                                    
     void* module_data ;                              
     CUmodule module ;                                
 } ;                                                  
                                                      
 extern char __hybridizer_cubin_module_data [] ;      
 static HybridModule __hybridizer__gs_module = { 0 }; 

#pragma region Wrappers definitions


extern "C" DLL_PUBLIC int run_ExternCWrapper_CUDA( int gridDim_x,  int gridDim_y,  int blockDim_x,  int blockDim_y,  int blockDim_z,  int shared,  double* const data_in,  int lineFrom,  int lineTo,  int N,  int M,  int frameNum,  int maxiter)
{
	CUresult cures ;                                                                                 
	if (__hybridizer__gs_module.module_data == 0)                                                    
	{                                                                                              
		cures = cuModuleLoadData (&(__hybridizer__gs_module.module), __hybridizer_cubin_module_data) ; 
		if (cures != CUDA_SUCCESS) return (int)cures ;                                                 
	}                                                                                              
	                                                                                                 
	CUfunction __hybridizer__cufunc ;                                                                
	                                                                                                 
	cures = cuModuleGetFunction (&__hybridizer__cufunc, __hybridizer__gs_module.module, "run") ;   
	if (cures != CUDA_SUCCESS) return (int)cures ;                                                   
	                                                                                                 
	hybridizer::runtime* __hybridizer_runtime = (hybridizer::runtime*) __hybridizer_init_basic_runtime(); 



	void* __hybridizer_launch_config[9] = 
		{
			(void*)&__hybridizer_runtime,
			(void*)&data_in,
			(void*)&lineFrom,
			(void*)&lineTo,
			(void*)&N,
			(void*)&M,
			(void*)&frameNum,
			(void*)&maxiter,
			(void*)0
		} ;

	shared += 16 ; if (shared > 48*1024) shared = 48*1024 ;                                                                                                
	                                                                                                                                                       
	cures = cuLaunchKernel (__hybridizer__cufunc, gridDim_x, gridDim_y, 1, blockDim_x, blockDim_y, blockDim_z, shared, 0, __hybridizer_launch_config, 0) ; 
	if (cures != CUDA_SUCCESS) return (int)cures ; 
	int cudaLaunchRes = (int)::cudaPeekAtLastError ();                                                                                                     
	if (cudaLaunchRes != 0) return cudaLaunchRes;                                                                                                          
	int __synchronizeRes = (int)::cudaDeviceSynchronize () ;                                                                                               
	return __synchronizeRes ;                                                                                                                              

}

#pragma endregion
