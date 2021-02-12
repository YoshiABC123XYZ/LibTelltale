#ifndef TTARCHIVE
#define TTARCHIVE

#include "LibTelltale.h"
#include "bytestream.h"

extern "C" {

	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_OK = 				 0x00;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_NULL_STREAM = 			 0x01;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_HEADER =			 0x02;
	//Redacted err 3
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_VERSION =			 0x04;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_DATA =			 0x05;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_KEY =			 0x06;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_LIB_ERR =			 0x07;
	
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_OK = 				 0x00;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_NULL_STREAM = 		 0x01;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_NULL_ARCHIVE = 		 0x02;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_DATA_ERR =			 0x03;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_LIB_ERR = 			 0x04;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_BAD_OPTIONS = 		 0x05;

	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_COMPRESS_DEFAULT = 		 0b1;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_COMPRESS_OODLE =		 0b10;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_ENCRYPT =			 0b100;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V0 =				 0b1000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V1 =				 0b10000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V2=				 0b100000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V3 =				 0b1000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V4 =				 0b10000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V5 =			    	 0b100000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V6 =				 0b1000000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V7 =				 0b10000000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V8 =				 0b100000000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V9 =				 0b1000000000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_SKIP_CRCS =			 0b10000000000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_RAW =				 0b100000000000000;
	_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_NO_TMPFILE =			 0b1000000000000000;

	namespace ttarchive {

		typedef struct TTArchiveHeader {
			bool enc_info : 1;
			bool cz : 1;
			bool legacy : 1;
			bool enc : 1;
		} ttarch_h;

		typedef struct TTArchiveEntry {
			uint64 offset;
			uint32 size;
			unsigned char* name;
		} ttarch_entry;

		typedef struct TTArchive {
			uint32 entry_count;
			uint8 version;
			unsigned char* game_key;
			ttarch_entry** entries;
			ttarch_h headerinfo;
			bytestream* stream;
			void* reserved;
		} ttarch;

		_LIBTT_EXPORT bytestream* TTArchive_StreamOpen(TTArchive* archive, TTArchiveEntry* entry);
		_LIBTT_EXPORT int TTArchive_Open(TTArchive* archive);
		_LIBTT_EXPORT void TTArchive_Free(TTArchive* archive);

	}

	namespace ttarchive2 {

		typedef struct TTArchive2_H {
			bool enc : 1;
			bool cz : 1;
			bool def : 1;
			bool oodle : 1;
		} ttarch2_h;

		typedef struct TTArchive2Entry {
			uint64 offset;
			uint32 size;
			uint64 crc64;
			uint32 names_offset;
		} ttarch2_entry;

		typedef struct TTArchive2EntryFlushable {
			char* name;
			uint64 crc;
			bytestream* stream;
			TTArchive2EntryFlushable() : crc(0) {}
		} ttarch2_entryf;

		typedef struct TTArchive2Flushable {
			int options;
			byteoutstream* stream;
			unsigned char* game_key; // Must be initialized with a get_key if TTARCH_FLUSH_ENCRYPT is an option when flushing! otherwise undefined behaviour
			std::vector<ttarch2_entryf*> entries;
			TTArchive2Flushable() : options(0), game_key(NULL) {}
		} ttarch2_f;

		typedef struct TTArchive2 {
			int version;
			ttarch2_h* headerinfo;
			uint32 nametable_size;
			uint32 entry_count;
			bytestream* name_table;
			unsigned char* game_key;
			uint64 files_start;
			ttarch2_entry** entries;
			bytestream* stream;
			TTArchive2() : game_key(NULL), version(0), headerinfo(NULL), name_table(NULL), entries(NULL) {}
		} ttarch2;

		typedef struct TTArchive2C {
			ttarch2* archive;
			uint32 chunk_size;
			uint32 chunk_count;
		} ttarch2_ctx;

		typedef void (*TTArchive2_OnFlush)(TTArchive2EntryFlushable* entry);

		_LIBTT_EXPORT char* TTArchive2_GetName(TTArchive2* archive, TTArchive2Entry* entry);
		_LIBTT_EXPORT bytestream* TTArchive2_StreamOpen(TTArchive2* archive, TTArchive2Entry* entry);
		_LIBTT_EXPORT int TTArchive2_Open(TTArchive2* archive);
		_LIBTT_EXPORT void TTArchive2_Free(TTArchive2* archive);
		_LIBTT_EXPORT int TTArchive2_Flush(TTArchive2Flushable* archive, TTArchive2_OnFlush filter);
		_LIBTT_EXPORT TTArchive2Flushable* TTArchive2_MakeFlushable(TTArchive2* archive, bool del_old);
		_LIBTT_EXPORT void TTArchive2_FreeFlushable(TTArchive2Flushable* archive);

	}

}
#endif
