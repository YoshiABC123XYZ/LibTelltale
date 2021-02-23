#pragma once

/*
	Helper functions for programs in languages such as C#, Java and Python to access the classes and their functions.
*/

#include "LibTelltale.h"
#include "ByteStream.h"
#include "ByteOutStream.h"
#include "FileStream.h"
#include "FileOutStream.h"
#include "TTArchive.h"

#define E _LIBTT_EXPORT

// TTARCHIVES

E uint32 TTArchive_GetEntryCount(ttarchive::TTArchive* archive);

E ttarchive::TTArchiveEntry* TTArchive_GetEntryAt(ttarchive::TTArchive* archive, uint32 index);

E void TTArchive_ClearEntries(ttarchive::TTArchive* archive);

E uint32 TTArchive2_GetEntryCount(ttarchive2::TTArchive2* archive);

E ttarchive2::TTArchive2Entry* TTArchive_GetEntryAt(ttarchive2::TTArchive2* archive, uint32 index);

E void TTArchive2_ClearEntries(ttarchive2::TTArchive2* archive);

// BYTE OUT STREAMS

E void ByteOutStream_Position(byteoutstream* stream, uint64 off);

E bool ByteOutStream_IsLittleEndian(byteoutstream* stream);

E void ByteOutStream_SetEndian(byteoutstream* stream, bool little);

E void ByteOutStream_WriteInt(byteoutstream* stream, uint32 width, uint64 i);

E uint64 ByteOutStream_GetPosition(byteoutstream* stream);

E uint64 ByteOutStream_GetSize(byteoutstream* stream);

E void ByteOutStream_WriteBytes(byteoutstream* stream, uint8* buf, uint32 size);

E bytestream* ByteOutStream_Create(uint32 size);

E fileoutstream* FileOutStream_Create(const char filepath[]);

E bool ByteOutStream_Valid(byteoutstream* stream);

//BYTE STREAMS

E void ByteStream_Position(bytestream* stream, uint64 off);

E bool ByteStream_IsLittleEndian(bytestream* stream);

E void ByteStream_SetEndian(bytestream* stream, bool little);

E uint64 ByteStream_ReadInt(bytestream* stream, uint32 width);

E uint64 ByteStream_GetPosition(bytestream* stream);

E uint64 ByteStream_GetSize(bytestream* stream);

E unsigned char* ByteStream_ReadString(bytestream* stream, uint32 len);

E unsigned char* ByteStream_ReadString0(bytestream* stream);

E uint8* ByteStream_ReadBytes(bytestream* stream, uint32 size);

E uint8 ByteStream_ReadByte(bytestream* stream);

E bytestream* ByteStream_Create(uint32 size);

E bytestream* ByteStream_CreateFromBuffer(uint8* buf, uint32 size);

E filestream* FileStream_Create(const char filepath[]);

E bool ByteStream_Valid(bytestream* stream);
