#pragma once
#include <vector>
#include "ByteStream.h"
#include "ByteOutStream.h"

/*
* Telltale Games' Dynamic Array - My Implementation of it
*/


//Old version of DCArray
#define DArray DCArray

typedef void* (*TDeserializer)(bytestream* stream);
typedef void (*TSerializer)(byteoutstream* stream, void* itemp);

template<typename T>
class DCArray {
public:
	DCArray(bytestream* serialized,TDeserializer deserializer);
	DCArray();
	int Size();
	void Add(T& v);
	~DCArray();
	void Remove(T& v);
	void AddAll(const DCArray<T>& other);
	void Clear();
	const T& operator[](std::size_t idx);
	void SerializeAll(byteoutstream* outstream, TSerializer serializer);
protected:
	std::vector<T> backend;
};
