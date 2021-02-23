## LibTelltale
A library which supports reading, editing and writing of Telltale Games' file formats such as: meshes, textures, scenes, properties, styles, input mappers and archives (+ more!).

The library will support all games from Telltale (see below for supported games), however few games will have features which are not available. See the unsupported.md for these games and whats supported and not as well as below too.

Please do contact me if you believe there is a memory leak/bug somewhere! 

This project is under the Creative Commons Attribution-NoDerivs 3.0 Unported license, in which allows you to copy and redistribute the material in any medium or format for any purpose, even commercially. If you remix, transform, or build upon the material, you may not distribute the modified material. If you use this library credit must be given visibly, and to Telltale Games\LCG Entertainments.

##### If you want to use C#/Java/Python with this library (more languages supported in the future if needed), the sources will be in src/<lang> 

#### Streams used in this library (All default to LITTLE endian!)

This library has the base class 'bytestream' which is an (almost) abstract class to any input stream of bytes. By default this stream opens from a buffer of memory previously allocated. The other public stream is the filestream, which as you guess just reads bytes from a file as the byte source.

Then there is byteoutstream and fileoutstream. These are identical to byte and file streams except they are for writing data, where a byteoutstreams buffer takes an initial size and will grow if you add more data to the buffer than the current size. The fileoutstreams grow writes zeros to the file.

##### For the documentation on how to use this library, in the docs directory just find the doc you are looking for. Suggested to start on the ttarchives! (ttarch/ttarch2)

##### IMPORTANT: There are some archives and files which won't be supported in the library. See the unsupported.md for more info.

#### All encryption keys are from Ttarchext by Aluigi which is under the GNU General Public License version 2.

The following are all the games which this library supports (which at the moment is all their released ones). The game ID is used to reference the game when getting the encryption key for the game (LibTelltale_GetKey("some game id")).

FORMAT: Game Name | Game IDs (For each episode if there are more than one) | Support Notes <br /> 
The support notes are only given if there is a known problem with it or a note.

Sam and Max: Save The World Remastered | sammaxremaster | N/A<br /> 
The Walking Dead: A New Day | twd1 | This is NOT the game key to use for TWD the definitive series! <br /> 
The Walking Dead: Michonne | michonne | N/A<br /> 
The Walking Dead: Season Two | wd2 | This is NOT the game key to use for TWD the definitive series! <br /> 
Game Of Thrones | thrones | N/A<br /> 
Hector: Badge Of Carnage | hector101, hector102, hector103 | N/A<br /> 
Marvels Gaurdians Of The Galaxy | marvel | N/A<br /> 
Batman: The Telltale Series | batman | N/A<br /> 
Batman: The Enemy Within | batman2 | N/A<br /> 
Wallace And Gromits Grand Adventures | wag101, wag102, wag103, wag103 | N/A<br /> 
Telltale Texas Hold'em | texasholdem | N/A<br /> 
Bone: Out from Boneville | boneville | N/A<br /> 
Bone: The Great Cow Race | cowrace | N/A<br /> 
CSI: 3 Dimensions of Murder | csi3dimensions | N/A<br /> 
Sam and Max: Season 1 (2006) | sammax101, sammax102, sammax103, sammax104, sammax105, sammax106 | This is not the remastered version!<br /> 
Sam and Max: Season 2 | sammax201, sammax202, sammax203, sammax204, sammax205 | N/A<br /> 
Sam and Max: Season 3 | sammax301, sammax302, sammax303, sammax304, sammax305 | N/A<br /> 
Tales from The Borderlands | borderlands | N/A<br /> 
Back To The Future | bttf101, bttf102, bttf103, bttf104, bttf105 | N/A<br /> 
CSI: Deadly Intent | csideadly | N/A<br /> 
CSI: Fatal Conspiracy | csifatal | N/A<br /> 
CSI: Hard Evidence | csihard | N/A<br /> 
Jurassic Park: The Game | jurrassicpark | Writing archives for this game is not supported; however you can still open them, edit the files and save the files for conversion etc<br /> 
Law and Order: Legacies | lawandorder | Writing archives for this game is not supported; however you can still open them, edit the files and save the files for conversion etc<br /> 
Minecraft: Story Mode | mcsm | N/A<br /> 
Minecraft Story Mode: Season Two | mc2 | N/A<br /> 
Poker Night at The Inventory | celebritypoker | N/A<br /> 
Poker Night 2 | celebritypoker2 | Writing archives for this game is not supported; however you can still open them, edit the files and save the files <br /> 
Puzzle Agent: The Mystery of Scoggins | grickle101 | N/A<br /> 
Puzzle Agent 2 | grickle102 |  Writing archives for this game is not supported; however you can still open them, edit the files and save the files <br /> 
Strong Bad: Cool Game for Attractive People | sbcg4ap101, sbcg4ap102, sbcg4ap103, sbcg4ap104, sbcg4ap105 | N/A<br /> 
The Wolf Among Us | fables | N/A<br /> 
Tales of Monkey Island | monkeyisland101, monkeyisland102, monkeyisland103, monkeyisland104, monkeyisland105 | Writing archives is likely to error!<br /> 
Walking Dead: The Definitive Series | wdc | N/A<br /> 
The Walking Dead: A New Frontier | wd3 | N/A






























