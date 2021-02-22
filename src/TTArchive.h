#ifndef TTARCHIVE
#define TTARCHIVE

#include "LibTelltale.h"
#include "bytestream.h"
#include "byteoutstream.h"
#include <vector>

#define TTARCH_GET_VER(options) ((options >> 7) & 15)
#define TTARCH_SET_VER(v,ops) ops &= 63; ops |= ((v & 15) << 7)
#define TTARCH_IS_VER(ver,ops) (TTARCH_GET_VER(ops) == ver)
#define OPSET(op) BIT_ORED(archive->options, op)
#define COMPRESSED(options) (OPSET(TTARCH_FLUSH_ENCRYPT) || OPSET(TTARCH_FLUSH_COMPRESS_OODLE) || OPSET(TTARCH_FLUSH_COMPRESS_DEFAULT))

_LIBTT_EXPORT constexpr auto TTARCH_OPEN_OK = 0x00;
_LIBTT_EXPORT constexpr auto TTARCH_OPEN_NULL_STREAM = 0x01;
_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_HEADER = 0x02;
_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_VERSION = 0x04;
_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_DATA = 0x05;
_LIBTT_EXPORT constexpr auto TTARCH_OPEN_BAD_KEY = 0x06;
_LIBTT_EXPORT constexpr auto TTARCH_OPEN_LIB_ERR = 0x07;
_LIBTT_EXPORT constexpr auto TTARCH_OPEN_NULL_ARCHIVE = 0x08;

_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_OK = 0x00;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_NULL_STREAM = 0x01;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_NULL_ARCHIVE = 0x02;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_DATA_ERR = 0x03;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_LIB_ERR = 0x04;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_BAD_OPTIONS = 0x05;

_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_COMPRESS_DEFAULT = 0b1;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_COMPRESS_OODLE =	 0b10;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_ENCRYPT =			 0b100;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_SKIP_CRCS =		 0b1000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_RAW =				 0b10000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_NO_TMPFILE =		 0b100000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V0 =				 0b00000000000; /*.ttarch LEGACY*/
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V1 =				 0b00010000000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V2=				 0b00100000000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V3 =				 0b00110000000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V4 =				 0b01000000000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V7 =				 0b01110000000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V8 =				 0b10000000000;
_LIBTT_EXPORT constexpr auto TTARCH_FLUSH_V9 =				 0b10010000000;

namespace ttarchive {

	typedef struct TTArchiveEntry {
		bytestream* override_stream;
		uint64 offset;
		uint32 size;
		char* name;
		uint8 flags   : 6;
		bool accessed : 1;
		bool reserved : 1;
		TTArchiveEntry() : flags(0), override_stream(0), accessed(false), reserved(false) {}
	} ttarch_entry;

	typedef struct TTArchive {
		unsigned char* game_key;
		std::vector<ttarch_entry*> entries;
		bytestream* stream;
		byteoutstream* flushstream;
		void* reserved;
		int options;
		TTArchive() : options(0), game_key(NULL), reserved(NULL) {};
	} ttarch;

	typedef void (*TTArchive_OnFlush)(TTArchiveEntry* entry);

	_LIBTT_EXPORT bytestream* TTArchive_StreamOpen(TTArchive* archive, TTArchiveEntry* entry);
	_LIBTT_EXPORT int TTArchive_Open(TTArchive* archive);
	_LIBTT_EXPORT void TTArchive_Free(TTArchive* archive);
	_LIBTT_EXPORT TTArchiveEntry* TTArchive_EntryCreate(const char* name, bytestream* stream);
	_LIBTT_EXPORT int TTArchive_Flush(TTArchive* archive,TTArchive_OnFlush onflush);
	_LIBTT_EXPORT void TTArchive_StreamSet(TTArchiveEntry* entry, bytestream * stream);
	_LIBTT_EXPORT TTArchiveEntry* TTArchive_EntryFind(TTArchive* archive,const char* name);

}

namespace ttarchive2 {

	typedef struct TTArchive2Entry {
		uint64 offset;
		uint32 size;
		uint64 name_crc;
		uint32 names_offset;
		char* name;
		bytestream* override_stream;
		uint8 flags;
		TTArchive2Entry() : name(NULL), name_crc(0), override_stream(0), flags(0) {}
	} ttarch2_entry;

	typedef struct TTArchive2 {
		int options;
		uint32 nametable_size;
		bytestream* name_table;
		unsigned char* game_key;
		uint64 files_start;
		std::vector<ttarch2_entry*> entries;
		bytestream* stream;
		byteoutstream* flushstream;
		uint8 flags;
		TTArchive2() : game_key(NULL), options(0), name_table(NULL), entries(NULL) {}
	} ttarch2;

	typedef struct TTArchive2C {
		ttarch2* archive;
		uint32 chunk_size;
		uint32 chunk_count;
	} ttarch2_ctx;

	typedef void (*TTArchive2_OnFlush)(TTArchive2Entry* entry);

	_LIBTT_EXPORT bytestream* TTArchive2_StreamOpen(TTArchive2* archive, TTArchive2Entry* entry);
	_LIBTT_EXPORT int TTArchive2_Open(TTArchive2* archive);
	_LIBTT_EXPORT void TTArchive2_Free(TTArchive2* archive);
	_LIBTT_EXPORT int TTArchive2_Flush(TTArchive2* archive, TTArchive2_OnFlush filter);
	_LIBTT_EXPORT TTArchive2Entry* TTArchive2_EntryCreate(const char* name, bytestream* stream);
	_LIBTT_EXPORT TTArchive2Entry* TTArchive2_EntryFind(TTArchive2* archive, const char* name);

}

#define DUMPTTARCH				FILE* tm = fopen("c:\\users\\lucas\\desktop\\debug.ttarch2", "wb");\
				int cpos = stream->get_position();\
				for (int i = 0; i < chunks; i++) {\
					uint32 z = 0x10000;\
					uint8* buf = (uint8*)calloc(1, 0x10000);\
					uint32 src = handle->value.chunk_sizes[i];\
					stream->seek_beg(cpos);\
					int i1 = z_decompress(buf,&z,stream->read(src),&src,-15);\
					if (i1)exit(1);\
					fwrite(buf, 0x10000, 1, tm);\
					cpos += src;\
				}

#endif
