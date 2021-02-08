#pragma once

#define _LIBTT_EXPORT __declspec(dllexport)
#define ENDSWITH(str,ending) !(std::string(str).compare(strlen(str) - strlen(ending), strlen(ending), ending))

#include <inttypes.h>
#include <zconf.h>
#include <cstring>
#include <zlib.h>

typedef int8_t int8;
typedef int16_t int16;
typedef int32_t int32;
typedef int64_t int64;
typedef uint8_t uint8;
typedef uint16_t uint16;
typedef uint32_t uint32;
typedef uint64_t uint64;

typedef uint8 endian;

constexpr endian LITTLE_ENDIAN = 0x01;
constexpr endian BIG_ENDIAN =  0x02;

/*Instead of encryption for some old games, telltale bit flipped the data in some wierd chunk sizes and encrypted the meta header bytes. These are its headers*/
constexpr uint32 HEADER_FORMATTED_A = 0xFB4A1764;
constexpr uint32 HEADER_FORMATTED_B = 0xEB794091;
constexpr uint32 HEADER_FORMATTED_C = 0x64AFDEFB;
constexpr uint32 HEADER_FORMATTED_D = 0x64AFDEAA;

constexpr uint32 HEADER_MSV5 = 0x4D535635; /*MSV5 : Meta Stream Version 5*/
constexpr uint32 HEADER_MSV6 = 0x4D535636; /*MSV6 : Meta Stream Version 6*/
constexpr uint32 HEADER_MTRE = 0x4D545245; /*MTRE : Meta ? Reference Encrypted? */
constexpr uint32 HEADER_MBIN = 0x4D42494E; /*MBIN : Meta Binary*/
constexpr uint32 HEADER_MBES = 0x4D424553; /*MBES : Meta Binary Encrypted Stream*/

constexpr uint32 TTARCHIVE_NT_SIZE = 0x10000;

constexpr uint32 HEADER_TTARCHIVE_V2 = 0x54544132; /*TTA2 : Telltale Archive v2 - unreleased*/
constexpr uint32 HEADER_TTARCHIVE_V3 = 0x54544133; /*TTA3 : Telltale Archive v3*/
constexpr uint32 HEADER_TTARCHIVE_V4 = 0x54544134; /*TTA4 : Telltale Archive v4*/

constexpr uint32 HEADER_TTARCHIVE_DEF = 0x5454434E; /*TTNC : Telltale Not Compressed*/
constexpr uint32 HEADER_TTARCHIVE_C_E = 0x54544345; /*TTCE : Telltale Compressed & Encrypted*/
constexpr uint32 HEADER_TTARCHIVE_C_Z = 0x5454435A; /*TTCZ : Telltale Compressed*/
constexpr uint32 HEADER_TTARCHIVE_C_e = 0x54544365; /*TTCe : Telltale Compressed & Encrypted : Specific algo (oodle)*/
constexpr uint32 HEADER_TTARCHIVE_C_z = 0x5454437A; /*TTCz : Telltale Compressed : Specific algo (oodle)*/

bool valid_str(uint8* buf,int len);

int z_decompress(uint8 * dest, uint32 * destLen, uint8 * source, uint32 * sourceLen, int window);

//Functions for myself to debug 

#define DUMPDEBUGAT(buf, size,fname) \
	char file[250];\
	strcpy(file,"c:\\users\\lucas\\desktop\\debug\\");\
	strcat(file,fname);\
	FILE * tmpfile = fopen(file,"wb");\
	fwrite(buf,size, 1, tmpfile);\
	fclose(tmpfile);

#define DUMPDEBUG(buf, size) \
	FILE * tmpfile1 = fopen("c:\\users\\lucas\\desktop\\debug.ttarch2","wb");\
	fwrite(buf,size, 1, tmpfile1);\
	fclose(tmpfile1);
