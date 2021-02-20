### The following are not supported or do not work (until further notice) in this library:

When creating a .ttarch (not .ttarch2) archive, creating without previously opening the archive is likely to cause errors when the game runs using it. This is because the archive contains sequences of encryption / unknown values which are saved in memory when the archive is loaded and then used when writing it. So always open the archive, then edit etc.. followed by finally saving it again and it will work without error :D.

