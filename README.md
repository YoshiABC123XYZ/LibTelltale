## LibTelltale
A library which supports reading, editing and writing of Telltale Games' file formats.

The library will support nearly all games from Telltale (see the telltale_keys.h for supported games) [Currently in development!]

As the library gets updated Ill update the following docs too. 
This lib will be available as a dynamic link library and static library for windows only! Download the source with the latest version for the header files.

This project is under the Creative Commons Attribution-NoDerivs 3.0 Unported license, in which allows you to copy and redistribute the material in any medium or format for any purpose, even commercially. If you remix, transform, or build upon the material, you may not distribute the modified material. If you use this library credit must be given visibly, and to Telltale Games\LCG Entertainments.

NOTE: Please do contact me if you believe there is a memory leak/bug somewhere! 

#### Streams used in this library

This library has the base class 'bytestream' which is an (almost) abstract class to any input stream of bytes. By default this stream opens from a buffer of memory previously allocated. The other stream at the moment is filestream, which as you guess just reads bytes from a file as the byte source. There is also a class called chunkedstream which is useful for reading bytes in chunks and when they are read manipulating them by example decrypting then or decompressing them. However you won't use this one. Endianess can be switched although it defaults to little endian!

##### For the documentation on how to use this library, in the docs directory just find the doc you are looking for. Suggested to start on the ttarchives! (ttarch/ttarch2)

#### All encryption keys are from Ttarchext by Aluigi which is under the GNU General Public License version 2.
