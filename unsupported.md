### The following are not supported or do not work (until further notice) in this library:

- When creating a .ttarch (not .ttarch2) archive, creating without previously opening the archive is likely (will work with some older games however) to cause errors when the game runs using it. This is because the archive contains sequences of encryption / unknown values which are saved in memory when the archive is loaded and then used when writing it. So always open the archive, then edit etc.. followed by finally saving it again and it will work without error :D.

- When creating a .ttarch (not .ttarch2) archive if you choose to compress it and not encrypt it, it will automatically be encrypted too due to problems with unencrypted and compressed archives in some games such as Back To The Future - Episode 105 (OUTATIME). The archive options flags will be updated after you call flush.

- Similar to above, when you toggle and enable a .ttarch (not .ttarch2) archive's compression and it was not compressed on load then you will get a warning returned on flush. This is because the type of compression varies between game archives. So be careful! This option is better used for .ttarch2 archives.

- Adding entries to a .ttarch (not .ttarch2) will also likely cause errors. Its suggested to just edit files, or view data in them. By edit, you can also replace etc...

#### The 'errors' above are WARNINGS which can be ignored and may or may not work.

- You cannot write or open version 9 archives (games such as jurassic park, law and order, puzzle agent 2, poker night 2, walking dead: a new day OG) due to a problem with a new unknown value which I cant figure out. Hopefully in the future ill add support for these versions. If you extract with ttarchext for example, then you can directly load the files from it in the library. The archives aren't supported, the files are.

- You cannot read or write formats such as .prop and .scene in games before The Wolf Among us. Due to the huge format differences. You could still try and open some it would work but its not guaranteed. 

- Opening a .amap from Sam and Max Remastered won't work (although the only amap is empty sooo :D)

- Chore files before Minecraft: Story Mode are not supported (format different, in the future I may add support for these files)
