#ifndef FILESTREAM
#define FILESTREAM

#include <stdio.h>
#include "bytestream.h"

extern "C" {

	class _LIBTT_EXPORT filestream : public bytestream {
	protected:
		FILE* in;
	public:
		uint8* read(uint32 size) override;
		~filestream();
		filestream(FILE* f);
		filestream(const char* filepath);
		bool valid() override;
	};

}

#endif