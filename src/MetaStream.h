#pragma once

#include "LibTelltale.h"
#include "ByteStream.h"
#include "DCArray.h"
#include "ByteOutStream.h"

//Once your TTContext is writing a file, the meta stream CANNOT be edited or the file will come out corrupt. The way to do it is:
/*
* - NextWrite (ttcontext)
* - <Write to stream> eg Vers_Flush
* - FinishCurrentWrite (ttcontext) - This removes the out stream ptr (so you need to call next write to write again) and the meta start offset.

* All versions of meta streams are:
* MBES, MBIN, MCOM, MSV4, MSV5, MSV6. MBES is an encrypted stream handled and converted to MBIN and automatically converted back during ttarchive read and write. mcom and mvs4 havent been released
* and i havent seen them so support for them isnt available D:
*
* 
*/

typedef struct {
	char* mTypeName;
	uint32 mVersion;
	uint64 mTypeNameCrc;
	uint32 mVersionCrc;
} MetaClassDescription;

class MetaStream {
protected:
	DCArray<MetaClassDescription*>* classes;
public:
	void Flush(byteoutstream* stream);
	uint32  mMetaVersion;
	uint32 mFlags;
	uint32 mSize;
	uint32 mTextureSize;
	MetaStream();
	~MetaStream();
	void Close();
	bool Open(bytestream* stream);
	DCArray<MetaClassDescription*>* GetClasses();
	uint32 GetClassVersion(const char typeName[], uint32 def);

};
