## LibTelltale
A library which supports reading, editing and writing of Telltale Games' file formats.

The library will support nearly all games from Telltale (see the telltale_keys.h for supported games) [Currently in development!]

As the library gets updated Ill update the following docs too. 
This lib will be available as a dynamic link library and static library for windows only! Download the source with the latest version for the header files.

This project is under the Creative Commons Attribution-NoDerivs 3.0 Unported license, in which allows you to copy and redistribute the material in any medium or format for any purpose, even commercially. If you remix, transform, or build upon the material, you may not distribute the modified material. If you use this library credit must be given visibly, and to Telltale Games\LCG Entertainments.

NOTE: Please do contact me if you believe there is a memory leak/bug somewhere! 

#### Streams used in this library

This library has the base class 'bytestream' which is an (almost) abstract class to any input stream of bytes. By default this stream opens from a buffer of memory previously allocated. The other stream at the moment is filestream, which as you guess just reads bytes from a file as the byte source. There is also a class called chunkedstream which is useful for reading bytes in chunks and when they are read manipulating them by example decrypting then or decompressing them. However you won't use this one. Endianess can be switched although it defaults to little endian!

### Opening an archive for reading

The first major part of the library is the handing of opening Ttarchives (both .ttarch and .ttarch2), with the correct game name.
The game names are found in telltale_keys.h!

The archives are opened by TTArchive2_Open and TTArchive_Open (TTArchive2 for .ttarch2, TTArchive for .ttarch)

```
#include "ttarchive.h"
#include "filestream"
#include "telltale_keys.h"
#include "libhandle.h"
// Opening a .ttarch2

//The first important thing to mention is that some archives require the oodle compression library (games such as Batman TEW),
//and you need to make sure the DLL is correct. Use the libhandle.h's MapLibraryDll("oodle","oo2core_7_win64.dll or whatever the dll is called")
//to map where LibTelltale should look for the oodle DLL. Defaults to oo2core_8_win64.dll. Most games dont use this, so you should be fine. It will
//return TTARCH_OPEN_LIB_ERR on TTArchive2_Open if the library is required and it could not be found/loaded.
using namespace ttarchive2;
TTArchive2 * archive = new TTArchive2();
archive.stream = new filestream("c:\\my\\path\\to\\my\\ttarch.ttarch2"); // Note that this stream gets closed (hence in this using fclose for the file) on destruct!
archive.game_key = get_key("mc2"); // Not required, but a lot of archives are encrypted and you should specify the key! (See telltale_keys.h for a list)
if(TTArchive2_Open(&archive)) {/*ERR!*/} // Returns 0 (TTARCH_OPEN_OK) if it opened without error, else D:
// Note, all files are not loaded into memory! A special instance of a bytestream is returned to open a stream which reads from the file in chunks.

printf("Archive contains %d file entries\n", archive.entry_count);
printf("The files start at offset %llx and the nametable has a size of %X bytes\n", archive.files_start, archive.nametable_size);
printf("Header Info: No encryption/compression? %d, encrypted? %d, compressed? %d\n", archive.headerinfo->def, archive.headerinfo->enc, archive.headerinfo->cz);

printf("-- Entries --\n");
for(int i = 0; i < archive.entry_count; i++){
  TTArchive2Entry* entry = archive.entries[i];
  
  printf("File name: %s, at offset %llx, with size %d\n", TTArchive2_GetName(&archive, entry), entry->offset, entry->size);
  // Getting the file name is a bit different. The namestable is loaded into memory instead of keeping the names in the entry struct. Use
  // TTArchive2_GetName(<archive>,<entry>) to get its name as a char*. To get an entry by name you would need to implement a search algorithm (unordered!)
  // The TTArchive2Entry also has the crc64 of the file from the archive : entry->crc64
  
  bytestream * myfilestream = TTArchive2_StreamOpen(&archive, entry);
  printf("The file has the header (little endian by default) %X\n", myfilestream->read_int(32));
  delete myfilestream;
}
TTArchive2_Free(&archive); // Frees all memory associated with the archive. This frees the stream too so be careful!

//Opening a .ttarch is the exact same, just with TTArchive structs and TTArchiveEntry. Its in the ttarchive namespace. Basically the same without the 2.
//The only major different is the names are in the entry structs, not in the nametable. So there is no TTArchive_GetName(...), its just entry->name (uchar*)!
```

### Flushing (writing) an archive to a stream (.ttarch support in next release!)

The code below shows how you would flush a loaded or created archive back to a .ttarch2 file. 

```

void myflush(TTArchive2Flushable* entry);//Come to this in a second

void myfunction() {

  using namespace ttarchive2;

  //The way of flushing an archive is to create a new instance of TTArchive2Flushable. This archive can be written and edited. To convert from a TTArchive2, use
  //TTArchive2_MakeFlushable(TTArchive2,bool). The second parameter specifies if the previous archive should be freed. The entries get deleted as they are converted,
  //making it slightly more efficient. DO NOT set it to true if you are going to keep the entries or use then to be flushed again as they need their streams!
  //Use only when you are deleting the old archive (Shortcut to TTArchive2_Free(...))

  TTArchive2Flushable * flush = TTArchive2_MakeFlushable(loadedArchive,false);
  flush.entries.add(TTArchive2_CreateFlushableEntry(new filestream("d:\\myfiles\\includeme.lua"), "includeme.lua");//Add an entry from a file on disk
  flush.entries.add(TTArchive2_ConvertToFlushable(old_entry_which_i_want_to_import_into_the_archive));//Convert a TTArchive2Entry to a flushable one, useful if you 
  //only want specific files from a previous archive.

  flush.game_key = get_key("wdc"); // The key is automatically set if converted from the old archive!
  flush.stream = new fileoutstream("c:\\users\\me\\desktop\\MyTtarchive.ttarch2"); // An outstream must be set when flushing it!

  flush.options |= TTARCH_FLUSH_COMPRESS_OODLE | TTARCH_FLUSH_ENCRYPT; //OR options for how to flush it. Set to 0 for default with no compression/encryption.
  //The options get automatically set when converting, and its suggested that you know which version of the archive to OR (TTARCH_FLUSH_V3 or TTARCH_FLUSH_V4) if 
  //you haven't converted. The version is the same for each archive in a  game, but to check just open an archive from the games archives folder. 
  //Check in a music and sounds archive (_ms.ttarch2) and see if the header is TTA3 or TTA4. Ignore the first 12 bytes.

  if(TTArchive2_Flush(flush)){/*ERR!*/}//Flushes the archive, without touching any part of the given archive apart from the options which will be changed if they are invalid.

  TTArchive2_FreeFlushable(flush,&myflush);//Works the same as TTArchive2_Free.
  //The second parameter is the (nullable) on flush function. Will get called after an entry has been written to the archive. Useful for debugging.
  delete flush;
}

void onflush(ttarchive2::TTArchive2EntryFlushable* entry) {
	if (!entry) {//If the entry is null, this means the function is compressing the archive! Added this so you know to wait as this can take time.
		printf("Compressing archive! Please wait...\n");
		return;
	}
	printf("Saved file to archive: %s\n", entry->name);//This may be called a lot earlier than expected, because the compression happens last. This is called
	//AFTER a file is saved to the archive!
}

```

#### All encryption keys are from Ttarchext by Aluigi which is under the GNU General Public License version 2.
