## TTArchive2 (.ttarch2) handling

An archive is held in a TTArchive2 struct, which contains the following fields
- game_key : This is the encryption key for the game returned by LibTelltale_GetKey. Must be set otherwise a lot of archives wont load.
- entries : All entries are found in this vector
- stream : The stream to read the archive from when using open.
- flushstream : The stream to write the archive to when using flush.
- options : ORed options which can be tested using the TTARCH_FLUSH_x options, and changed. You can specify encrypt/compressed/raw/uncompressed/oodle etc..

Each entry in the entries vector is a pointer to a ttarchive2entry, which has the following fields 
- offset : The file offset in the archive
- size : The size of the entry in bytes
- name : The name of the entry
- name_crc : The CRC64 of the file name, used mostly internally by the library. Do not edit this, use entrysetname. 
- flags : Internal use
- override_stream : The stream of the entry, but this is only used when editing the entry or creating a new one. Using streamopen opens the stream from the archive that was used in open and hence wont return the stream from the archive. This field should not be directly set, use streamset. 
<br />
TTArchive2_Open(archive*). This opens the archive from the stream field in the archive. Returns an int which is any of the the TTARCH_OPEN_x constants.<br />
TTArchive2_Free(archive*). Call this to free the archive and remove all entries etc.. Don't do this manually, use this! WARNING: This frees the streams too! It does NOT delete the archive pointer, thats up to you to do after.<br />
TTArchive2_StreamOpen(archive*,entry*). Returns a bytestream* of the given entry, reading from the file as you request data.<br />
TTArchive2_Flush(archive*, flushfunc*). Writes the archive and all its entries to the flushstream stream in the archive. This can take a while depending on bigger archives, so the flushfunc is a (nullable) function which gets called when a file is written to the archive. It should be void and take an entry* as a parameter. The function definition is void (*TTArchive2_OnFlush)(TTArchive2Entry* entry); WARNING: If the parameter the library passes is NULL then its compressing the archive, useful as this can take long.<br />
TTArchive2_EntryCreate(const char name[], stream*). Creates and returns a TTArchive2Entry, instead of using new. This sets the fields and you can add this to the vector in the archive. If you want to add entries to an archive, you MUST use this otherwise you will get undefined behaviour when writing the archive.<br />
TTArchive2_EntryFind(archive*, const char name[]). Factory method to linearly search the archive entries for an archive with the given name and returns it.<br />
TTArchive2_EntrySetName(entry*, const char name[]). Sets the name of the archive. Use this instead of directly addressing the name field.<br /><br />

## TTArchive (.ttarch) handling

An archive is held in a TTArchive struct, which contains the following fields
- game_key : This is the encryption key for the game returned by LibTelltale_GetKey. Must be set otherwise a lot of archives wont load.
- entries : All entries are found in this vector
- stream : The stream to read the archive from when using open.
- flushstream : The stream to write the archive to when using flush.
- reserved : A reserved struct for internal handling
- options : ORed options which can be tested using the TTARCH_FLUSH_x options, and changed. In TTArchives (not ttarchive2s) its suggested to not touch this as older archives are very specific. Suggested to leave this for only getting info about the archive.

Each entry in the entries vector is a pointer to a ttarchiveentry, which has the following fields 
- offset : The file offset in the archive
- size : The size of the entry in bytes
- name : The name of the entry
- flags : Internal use, used to hold formatted options about the file when rewriting after open
- accessed : Not really useful, just tested if you have opened the stream for it yet. Do not tamper with this!
- reserved : Reserved for lua scripts, do not tamper with this either!
- override_stream : The stream of the entry, but this is only used when editing the entry or creating a new one. Using streamopen opens the stream from the archive that was used in open and hence wont return the stream from the archive. This field should not be directly set, use streamset. 

The fields in the structures above are not meant to be edited by you. Let the library take care of that to save errors.<br />

TTArchive_Open(archive*). This opens the archive from the stream field in the archive. Returns an int which is any of the the TTARCH_OPEN_x constants.<br />
TTArchive_Free(archive*). Call this to free the archive and remove all entries etc.. Don't do this manually, use this! WARNING: This frees the streams too! It does NOT delete the archive pointer, thats up to you to do after.<br />
TTArchive_StreamOpen(archive*,entry*). Returns a bytestream* of the given entry, reading from the file as you request data.<br />
TTArchive_StreamSet(entry*, stream*). Use this method to set the stream of the given entry. Don't set override_stream, this is what you need to call for that!<br />
TTArchive_Flush(archive*, flushfunc*). Writes the archive and all its entries to the flushstream stream in the archive. This can take a while depending on bigger archives, so the flushfunc is a (nullable) function which gets called when a file is written to the archive. It should be void and take an entry* as a parameter. The function definition is void (*TTArchive_OnFlush)(TTArchiveEntry* entry); WARNING: If the parameter the library passes is NULL then its compressing the archive, useful as this can take long.<br />
TTArchive_EntryCreate(const char name[], stream*). Creates and returns a TTArchiveEntry, instead of using new. This sets the fields and you can add this to the vector in the archive. If you want to add entries to an archive, you MUST use this otherwise you will get undefined behaviour when writing the archive.<br />
TTArchive_EntryFind(archive*, const char name[]). Factory method to linearly search the archive entries for an archive with the given name and returns it.<br />
