#ifndef TTARCHIVE
#define TTARCHIVE

#include "LibTelltale.h"
#include "bytestream.h"

extern "C" {

	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_OK = 0x00;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_NULL_STREAM = 0x01;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_HEADER = 0x02;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_VERSION = 0x04;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_DATA = 0x05;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_KEY = 0x06;
	_LIBTT_EXPORT constexpr auto TTARCH_OPEN_LIB_ERR = 0x07;

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


		typedef struct TTArchive2 {
			ttarch2_h* headerinfo;
			uint32 nametable_size;
			uint32 entry_count;
			bytestream* name_table;
			unsigned char* game_key;
			uint64 files_start;
			ttarch2_entry** entries;
			bytestream* stream;
		} ttarch2;

		typedef struct TTArchive2C {
			ttarch2* archive;
			uint32 chunk_size;
			uint32 chunk_count;
		} ttarch2_ctx;

		_LIBTT_EXPORT char* TTArchive2_GetName(TTArchive2* archive, TTArchive2Entry* entry);
		_LIBTT_EXPORT bytestream* TTArchive2_StreamOpen(TTArchive2* archive, TTArchive2Entry* entry);
		_LIBTT_EXPORT int TTArchive2_Open(TTArchive2* archive);
		_LIBTT_EXPORT void TTArchive2_Free(TTArchive2* archive);


	}

}
#endif
