### The following are not supported or do not work (until further notice) in this library:

- When creating a .ttarch (not .ttarch2) archive, creating without previously opening the archive is likely (will work with some older games however) to cause errors when the game runs using it. This is because the archive contains sequences of encryption / unknown values which are saved in memory when the archive is loaded and then used when writing it. So always open the archive, then edit etc.. followed by finally saving it again and it will work without error :D.

- When creating a .ttarch (not .ttarch2) archive if you choose to compress it and not encrypt it, it will automatically be encrypted too due to problems with unencrypted and compressed archives in some games such as Back To The Future - Episode 105 (OUTATIME). The archive options flags will be updated after you call flush.

- Similar to above, when you toggle and enable a .ttarch (not .ttarch2) archive's compression and it was not compressed on load then you will get a warning returned on flush. This is because the type of compression varies between game archives. So be careful! This option is better used for .ttarch2 archives.

- Adding entries to a .ttarch (not .ttarch2) will also likely cause errors. Its suggested to just edit files, or view data in them. By edit, you can also replace etc...

#### The 'errors' above are WARNINGS which can be ignored and may or may not work.

- You cannot write version 9 archives (games such as jurassic park, law and order, puzzle agent 2) due to a problem with a new unknown value which I cant figure out. Hopefully in the future ill add support for these versions. However, you can open a version 9 archive and edit it and save files from the archive (eg extracting or just converting the files using the library etc..).
