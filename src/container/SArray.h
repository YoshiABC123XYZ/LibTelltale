#pragma once

#include <vector>
#include "ByteStream.h"
#include "ByteOutStream.h"
#include "DCArray.h"

/*
* Telltale Games' Static Array - My Implementation of it
*/

typedef void* (*TDeserializer)(bytestream* stream);
typedef void (*TSerializer)(byteoutstream* stream, void* itemp);

template<typename T>
class SArray {
public:
	SArray(bytestream* serialized, TDeserializer deserializer);
	int Size();
	const T& operator[](std::size_t idx);

	void SerializeAll(byteoutstream* outstream, TSerializer serializer);

protected:
	std::vector<T> backend;
};
