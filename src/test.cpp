#include "ttarchive.h"
#include "filestream.h"
#include "telltale_keys.h"

void dump_test(bytestream* stream);

int main() {
	bytestream* stream = new filestream("C:\\Users\\lucas\\Desktop\\in.ttarch");
	if (!stream->valid()) {
		printf("could not locate the input file"); 
		return 0;
	}
	dump_test(stream);
}

void dump_test(bytestream* stream) {
	using namespace ttarchive2;
	TTArchive2 archive;
	archive.game_key = get_key("batman");
	if (!archive.game_key) {
		printf("key not found");
		return;
	}
	archive.stream = stream;
	int res;
	printf("\nopening...\n");
	if (res = TTArchive2_Open(&archive)) {
		printf("Error opening archive: %d", res);
		return;
	}
	printf("Entries : %d\n", archive.entry_count);
	for (int i = 0; i < archive.entry_count; i++) {
		ttarch2_entry* entry = archive.entries[i];
		printf("File: %s at offset %llx\n", TTArchive2_GetName(&archive,entry),entry->offset);
		bytestream* fstream = TTArchive2_StreamOpen(&archive, entry);
		DUMPDEBUGAT(fstream->read(fstream->get_stream_size()), fstream->get_stream_size(), TTArchive2_GetName(&archive, entry))
	}
}
