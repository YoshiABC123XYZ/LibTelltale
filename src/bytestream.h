#ifndef BYTESTREAM
#define BYTESTREAM

#include "LibTelltale.h"

#include <iostream>

extern "C" {

	//uint* convert_func(inbuf, chunksize, inbufsize,outsize*). outsize is NULLPTR ABLE
	_LIBTT_EXPORT typedef uint8* (*convert_func)(uint8*, uint32, uint32, uint32*);

	class _LIBTT_EXPORT bytestream {
	protected:
		endian order;
		uint64 size;
		uint64 pos;
		uint64 mark;
		uint8* buf = nullptr;
	public:
		bytestream();
		bytestream(uint8* buf, uint64 size);
		~bytestream();
		virtual uint8 read();
		virtual uint8* read(uint32 size);
		unsigned char* read_string(uint32 len);
		unsigned char* read_string();
		uint64 read_int(uint8 width);
		void read_to(uint8* buf, uint32 size);
		void read_to_filter(uint8* buf, uint32 size, uint32 chunk_size, convert_func func, uint32* outsize);
		void mark_pos(uint64 pos);
		uint64 get_position();
		uint64 get_stream_size();
		uint64 get_mark();
		uint8* get_buffer();
		void rewind();
		endian get_endian();
		void set_endian(endian e);
		virtual bool seek_beg(uint64 pos);
		bool seek_cur(uint64 pos);
		bool seek_end(uint64 pos);
		virtual bool valid();
	};
}

#endif
