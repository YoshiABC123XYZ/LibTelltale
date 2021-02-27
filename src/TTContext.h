#pragma once

#include "LibTelltale.h"
#include "TTArchive.h"
#include "ByteStream.h"
#include "MetaStream.h"
#include "Vers.h"
#include "DCArray.h"
#include "TTArchive.h"

typedef struct Block {
	uint32 size;
	bytestream* blockstream;
} Block;

struct Vers;

class TTContext {
	protected:
		void* archive;
		bytestream* current_stream;
		byteoutstream* current_ostream;
		uint32 start_offset;
		MetaStream* current_meta;
		char* current_file_name;
	public:
		TTContext(void* ttarch_or_ttarch2);
		void NextArchive(void* ttarch_or_ttarch2, bool del);
		~TTContext();
		bool NextStream(bytestream* next, bool del);
		bool NextWrite(const char file[],byteoutstream* out, bool del);
		void OverrideNewMeta(MetaStream* stream, bool del);
		void FinishCurrentWrite(bool del);
		char* GetCurrentFileName();
		Block* ReadBlock();
		void SkipBlock();
		void SkipString();
		char* ReadString();
		void DeleteBlock(Block* blk);
		char* FindArchiveEntry(uint64 filename_crc);
		uint32 GetStartOffset();
		MetaStream* GetCurrentMeta();
		bytestream* GetCurrentStream();
		byteoutstream* GetCurrentOutStream();
		bytestream* OpenArchiveStream(const char fileName[]);
		DCArray<Vers*>* versions;

};
