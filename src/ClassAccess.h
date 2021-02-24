#pragma once

/*
	Helper functions for programs in languages such as C#, Java and Python to access the classes and their functions.
	The prefix 'h' stands for helper function :D
	Looks like a C source lol
*/

#include "LibTelltale.h"
#include "ByteStream.h"
#include "ByteOutStream.h"
#include "FileStream.h"
#include "FileOutStream.h"
#include "TTArchive.h"

#define E _LIBTT_EXPORT

// TTARCHIVES

E void hTTArchive_EntrySetName(ttarchive::TTArchiveEntry* entry, const char name[]);

E void hTTArchive_EntryAdd(ttarchive::TTArchive* arc, ttarchive::TTArchiveEntry* entry);

E void hTTArchive2_EntryAdd(ttarchive2::TTArchive2* arc, ttarchive2::TTArchive2Entry* entry);

E uint32 hTTArchive_GetEntryCount(ttarchive::TTArchive* archive);

E ttarchive::TTArchiveEntry* hTTArchive_GetEntryAt(ttarchive::TTArchive* archive, uint32 index);

E void hTTArchive_ClearEntries(ttarchive::TTArchive* archive);

E uint32 hTTArchive2_GetEntryCount(ttarchive2::TTArchive2* archive);

E ttarchive2::TTArchive2Entry* hTTArchive_GetEntryAt(ttarchive2::TTArchive2* archive, uint32 index);

E void hTTArchive2_ClearEntries(ttarchive2::TTArchive2* archive);

// BYTE OUT STREAMS

E void hByteOutStream_Position(byteoutstream* stream, uint64 off);

E bool hByteOutStream_IsLittleEndian(byteoutstream* stream);

E void hByteOutStream_SetEndian(byteoutstream* stream, bool little);

E void hByteOutStream_WriteInt(byteoutstream* stream, uint32 width, uint64 i);

E uint64 hByteOutStream_GetPosition(byteoutstream* stream);

E uint64 hByteOutStream_GetSize(byteoutstream* stream);

E void hByteOutStream_WriteBytes(byteoutstream* stream, uint8* buf, uint32 size);

E bytestream* hByteOutStream_Create(uint32 size);

E fileoutstream* hFileOutStream_Create(const char filepath[]);

E bool hByteOutStream_Valid(byteoutstream* stream);

//BYTE STREAMS

E void hByteStream_Position(bytestream* stream, uint64 off);

E bool hByteStream_IsLittleEndian(bytestream* stream);

E void hByteStream_SetEndian(bytestream* stream, bool little);

E uint64 hByteStream_ReadInt(bytestream* stream, uint32 width);

E uint64 hByteStream_GetPosition(bytestream* stream);

E uint64 hByteStream_GetSize(bytestream* stream);

E unsigned char* hByteStream_ReadString(bytestream* stream, uint32 len);

E unsigned char* hByteStream_ReadString0(bytestream* stream);//Reads a null terminated string

E uint8* hByteStream_ReadBytes(bytestream* stream, uint32 size);

E uint8 hByteStream_ReadByte(bytestream* stream);

E bytestream* hByteStream_Create(uint32 size);

E bytestream* hByteStream_CreateFromBuffer(uint8* buf, uint32 size);

E filestream* hFileStream_Create(const char filepath[]);

E bool hByteStream_Valid(bytestream* stream);
