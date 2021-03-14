## LibTelltale
A library which supports reading, editing and writing of Telltale Games' file formats such as: meshes, textures, scenes, properties, styles, input mappers and archives (+ more!).

The library will support all games from Telltale (see below for supported games), however few games will have features which are not available. See the unsupported.md for these games and whats supported and not as well as below too.

Please do contact me if you believe there is a memory leak/bug somewhere! 

This project is under the Creative Commons Attribution-NoDerivs 3.0 Unported license, in which allows you to copy and redistribute the material in any medium or format for any purpose, even commercially. If you remix, transform, or build upon the material, you may not distribute the modified material. If you use this I library credit I would very much appreciate if you give credit visibly, and to Telltale Games\LCG Entertainments.

##### If you want to use C#/Java/Python with this library (more languages supported in the future if needed), the sources will be in src/[lang]

#### Streams used in this library (All default to LITTLE endian!)

This library has the base class 'bytestream' which is an (almost) abstract class to any input stream of bytes. By default this stream opens from a buffer of memory previously allocated. The other public stream is the filestream, which as you guess just reads bytes from a file as the byte source.

Then there is byteoutstream and fileoutstream. These are identical to byte and file streams except they are for writing data, where a byteoutstreams buffer takes an initial size and will grow if you add more data to the buffer than the current size. The fileoutstreams grow writes zeros to the file.

##### For the documentation on how to use this library, in the docs directory just find the doc you are looking for. Suggested to start on the ttarchives! (ttarch/ttarch2)

##### IMPORTANT: There are some archives and files which won't be supported in the library. See the unsupported.md for more info.

The encryption keys can all be also found in the executables, by searching '985887462' with a hex editor below it you should see a 55 byte long encryption key :D. The value 985887462 is a blowfish encryption constant in the ORIG_S.<br/><br/>

Recent work on the formats has meant that support for any old games in MTRE or MBIN meta versions (ie, games using .ttarch not .ttarch2 before the wolf among us) has been removed. This means you can't have access to a lot of those games' files. However, formats such as D3DTX and D3DMESH will be supported since they do not have the big changes which files such as .prop and .scene have in newer games.
