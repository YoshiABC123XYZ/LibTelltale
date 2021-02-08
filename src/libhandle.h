#pragma once

//Use for setting the custom dll name mappings for the various libraries this library requires. Use MapLibraryDll to map a dll reference ID (see below) to a 
//DLLs name. 

#include <map>
#include <string>
#include <Windows.h>
#include "LibTelltale.h"

using namespace std;

#define DLL_REFERENCE_IDS

constexpr auto OODLE_REF = "oodle";

#undef DLL_REFERENCE_IDS


#define DLL_DEFAULTS

constexpr auto OODLE_DEFAULT = "oodle 2.7.3.dll";

#undef DLL_DEFAULTS


extern "C" {

	_LIBTT_EXPORT void MapLibraryDll(char* lib, char* path);
	_LIBTT_EXPORT void ClearMappedLibraryDlls();

}

HMODULE load_lib(const char* lib1, const char* def1);
