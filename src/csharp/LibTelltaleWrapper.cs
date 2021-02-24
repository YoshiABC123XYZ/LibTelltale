using System;
using System.Runtime.InteropServices;

/** 
 * C# Implementation of LibTelltale. May look slightly like Java because I don't code much C# D:
 * 
 * Members are left all protected but the classes are left final because in the future I may make then inheritable.
 * 
 * Remember that the library HAS to be called 'LibTelltale.dll' in the Dll directory as the exe or the one defined by SetDllDirectory.
 * 
 * Documented lots because the author of TSE wanted it ._. lol
 * 
*/

namespace LibTelltale {

	public class Config {

		/// <summary>
		/// The Minimum version required of the LibTelltale DLL for this library to work.
		/// </summary>
		public static readonly Version MIN_VERSION = Version.Parse("2.0.5");

		static Config() {
			if (MIN_VERSION > Version.Parse (GetVersion ()))
				throw new LibTelltaleException (String.Format("Cannot use LibTelltale v{0}, the minimum version required is v{1}", GetVersion(), MIN_VERSION));
		}

		/// <summary>
		/// Clears the mapped libraries.
		/// </summary>
		public static void ClearMappedLibraries(){
			Config0.LibTelltale_ClearMappedLibs ();
		}

		/// <summary>
		/// Maps a library. The library id can be found in libtelltale.h on github, and the dll_name is the dll file name that libtelltale should search using.
		/// </summary>
		/// <param name="lib_id">Lib identifier.</param>
		/// <param name="dll_name">Dll name.</param>
		public static void MapLibrary(string lib_id, string dll_name){
			if (lib_id == null || dll_name == null)
				return;
			Config0.LibTelltale_MapLib (Marshal.StringToHGlobalAnsi (lib_id), Marshal.StringToHGlobalAnsi (dll_name));
		}

		/// <summary>
		/// Gets a game encryption key by its ID.
		/// </summary>
		/// <returns>The game encryption key.</returns>
		/// <param name="game_id">Game identifier.</param>
		public static string GetGameEncryptionKey(string game_id){
			return Marshal.PtrToStringAnsi (Config0.LibTelltale_GetKey (Marshal.StringToHGlobalAnsi (game_id)));
		}

		/// <summary>
		/// Gets the version of LibTelltale.
		/// </summary>
		/// <returns>The version.</returns>
		public static string GetVersion(){
			return Marshal.PtrToStringAnsi (Config0.LibTelltale_Version ());
		}

	}

	class Config0 {

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr LibTelltale_Version();

		[DllImport("LibTelltale.dll")]
		public static extern void LibTelltale_MapLib(IntPtr id, IntPtr name);

		[DllImport("LibTelltale.dll")]
		public static extern void LibTelltale_ClearMappedLibs ();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr LibTelltale_GetKey(IntPtr id);

	}

	/// <summary>
	/// Any exception which is because you may have forgotten to set a stream etc.
	/// </summary>
	public class LibTelltaleException : Exception {
		public LibTelltaleException(string message) : base(message) {}
	}

	/// <summary>
	/// This namespace is all to do with the loading/writing of TTArchive bundles, .ttarch and .ttarch2
	/// </summary>
	namespace TTArchives {

		/// <summary>
		/// Constants which get returned from flush and open, use this to set custom options too.
		/// </summary>
		public static class Constants {
			public static readonly uint TTARCH_OPEN_OK = 0;
			public static readonly uint TTARCH_OPEN_BAD_STREAM = 1;
			public static readonly uint TTARCH_OPEN_BAD_HEADER = 2;
			public static readonly uint TTARCH_OPEN_BAD_VERSION = 4;
			public static readonly uint TTARCH_OPEN_BAD_DATA = 5;
			public static readonly uint TTARCH_OPEN_BAD_KEY = 6;
			public static readonly uint TTARCH_OPEN_LIB_ERR = 7;
			public static readonly uint TTARCH_OPEN_BAD_ARCHIVE = 8;
			public static readonly uint TTARCH_FLUSH_OK = 0;
			public static readonly uint TTARCH_FLUSH_BAD_STREAM = 1;
			public static readonly uint TTARCH_FLUSH_BAD_ARCHIVE = 2;
			public static readonly uint TTARCH_FLUSH_DATA_ERR = 3;
			public static readonly uint TTARCH_FLUSH_LIB_ERR = 4;
			public static readonly uint TTARCH_FLUSH_BAD_OPTIONS = 5;
			public static readonly uint TTARCH_FLUSH_COMPRESS_DEFAULT = 1;
			public static readonly uint TTARCH_FLUSH_COMPRESS_OODLE = 2;
			public static readonly uint TTARCH_FLUSH_ENCRYPT = 4;
			public static readonly uint TTARCH_FLUSH_SKIP_CRCS =	8;
			public static readonly uint TTARCH_FLUSH_RAW = 16;
			public static readonly uint TTARCH_FLUSH_NO_TMPFILE = 32;
			public static readonly uint TTARCH_FLUSH_V0 = 0;
			public static readonly uint TTARCH_FLUSH_V1 = 128;
			public static readonly uint TTARCH_FLUSH_V2 = 256;
			public static readonly uint TTARCH_FLUSH_V3 = 384;
			public static readonly uint TTARCH_FLUSH_V4 = 512;
			public static readonly uint TTARCH_FLUSH_V7 = 896;
			public static readonly uint TTARCH_FLUSH_V8 = 1024;
			public static readonly uint TTARCH_FLUSH_V9 = 1152;
		}

		/// <summary>
		/// Represents an entry in a .ttarch archive. Do not touch the referencee pointer or any value in any of the fields! The only field useful is the backend.name.
		/// </summary>
		public struct TTArchiveEntry {
			public _TTArchiveEntry backend;
			public IntPtr reference;
		}

		/// <summary>
		/// The full struct of an entry in memory, important that you do not edit fields in here!
		/// </summary>
		public struct _TTArchiveEntry {
			public IntPtr override_stream; // To override it, use set stream!
			public ulong offset;
			public uint size;
			[MarshalAs(UnmanagedType.LPStr)]
			public string name;
			public byte flags;
		}

		/// <summary>
		/// Handles a .ttarch archive
		/// </summary>
		public sealed class TTArchive {

			/// <summary>
			/// Sets the name of the entry, do not use entry.name = ...
			/// </summary>
			public static void SetEntryName(TTArchiveEntry entry, string name){
				Native.hTTArchive_EntrySetName (entry.reference, name);
			}

			/// <summary>
			/// Creates the TTArchive entry, using the given name and input stream of bytes.
			/// </summary>
			/// <returns>The TTArchive entry.</returns>
			public static TTArchiveEntry CreateTTArchiveEntry(string name, ByteStream stream){
				TTArchiveEntry ret = new TTArchiveEntry();
				ret.reference = Native.TTArchive_EntryCreate (name, stream.reference);
				ret.backend = (_TTArchiveEntry)Marshal.PtrToStructure (ret.reference, typeof(_TTArchiveEntry));
				return ret;
			}

			[StructLayout(LayoutKind.Sequential)]
			protected struct ttarch {
				public IntPtr game_key;
				public IntPtr entries;
				public IntPtr stream;
				public IntPtr flushstream;
				public IntPtr reserved;
				public uint options;
			};

			protected ttarch handle;

			/// <summary>
			/// Reference pointer, do not touch.
			/// </summary>
			public readonly IntPtr reference;

			protected ByteStream instream;

			protected ByteOutStream outstream;

			/// <summary>
			/// Gets the archive options.
			/// </summary>
			/// <returns>The archive options.</returns>
			public uint GetArchiveOptions(){
				UpdateAndSync (true);
				return this.handle.options;
			}

			/// <summary>
			/// Sets an archive option, use the options from constants.
			/// </summary>
			/// <param name="op">Op.</param>
			public void SetArchiveOption(uint op){
				this.handle.options |= op;
				UpdateAndSync (false);
			}

			/// <summary>
			/// Convenience method to unset an option from constants.
			/// </summary>
			/// <param name="op">Op.</param>
			public void UnsetArchiveOption(uint op){
				this.handle.options &= ~op;
				UpdateAndSync (false);
			}

			/// <summary>
			/// Gets the archive version.
			/// </summary>
			/// <returns>The archive version.</returns>
			public uint GetArchiveVersion(){
				return (GetArchiveOptions () >> 7) & 15;
			}

			/// <summary>
			/// Sets the archive version. Must only use versions from constants.
			/// </summary>
			/// <param name="version">Version.</param>
			public void SetArchiveVersion(uint version){
				this.handle.options &= 63;
				this.handle.options |= (version & 15) << 7;
				UpdateAndSync (false);
			}

			/// <summary>
			/// Initializes a new  <see cref="LibTelltale.TTArchives.TTArchive"/>. The game ID should be the game ID from the github page, and will throw an exception in the case that the game for the ID
			/// could not be found.
			/// </summary>
			/// <param name="gameid">The Game ID</param>
			public TTArchive(string gameid){
				reference = Native.hTTArchive_Create ();
				if (this.reference.Equals (IntPtr.Zero))
					throw new LibTelltaleException ("Could not create backend archive");	
				this.UpdateAndSync (false);
				IntPtr key = Marshal.StringToHGlobalAnsi (gameid);
				this.handle.game_key = Config0.LibTelltale_GetKey (key);
				if (this.handle.game_key.Equals (IntPtr.Zero))
					throw new LibTelltaleException (String.Format("Could not find a key for the game ID {0}", gameid));
				Marshal.FreeHGlobal (key);
			}

			/// <summary>
			/// Opens a readable byte input stream for the given entry.
			/// </summary>
			public ByteStream StreamOpen(TTArchiveEntry entry){
				return new ByteStream (Native.TTArchive_StreamOpen (reference, entry.reference));
			}

			/// <summary>
			/// Removes the entry from the archive.
			/// </summary>
			/// <param name="e">The Entry to remove. Must not be null</param>
			/// <param name="delete">If set to <c>true</c> then the entry will be freed to save memory. Use this only if you are removing and deleting the entry because you don't need it</param>
			public void RemoveEntry(TTArchiveEntry e, bool delete){
				Native.hTTArchive_EntryRemove (this.reference, e.reference, delete);
			}

			/// <summary>
			/// Finds an entry by its full file name, for example FindEntry("Boot.lua"). Returns a nullable value, if not found.
			/// </summary>
			public TTArchiveEntry? FindEntry(string name){
				TTArchiveEntry r = new TTArchiveEntry ();	
				IntPtr entryp = Native.TTArchive_EntryFind(reference, name);
				if (entryp.Equals (IntPtr.Zero))
					return null;
				r.backend = (_TTArchiveEntry)Marshal.PtrToStructure (entryp, typeof(_TTArchiveEntry));
				r.reference = entryp;
				return r;
			}

			/// <summary>
			/// Sets the stream for entry, which will be the overriden stream to use when writing (flushing) the archive.
			/// </summary>
			public void SetStreamForEntry(TTArchiveEntry entry, ByteStream stream){
				Native.TTArchive_StreamSet (entry.reference, stream.reference);
				entry.backend = (_TTArchiveEntry)Marshal.PtrToStructure (entry.reference, typeof(_TTArchiveEntry));
			}

			/// <summary>
			/// Adds the entry to the backend vector list ready for when you write back the archive.
			/// </summary>
			/// <param name="entry">Entry.</param>
			public void AddEntry(TTArchiveEntry entry){
				Native.hTTArchive_EntryAdd (this.reference, entry.reference);
				UpdateAndSync (true);
			}

			/// <summary>
			/// Gets an entry at the given file index. This is used when you want to loop through all files, or if you already know the index in the entry list.
			/// </summary>
			/// <returns>The entry.</returns>
			public TTArchiveEntry? GetEntry(uint index){
				TTArchiveEntry r = new TTArchiveEntry ();
				IntPtr entryp = Native.hTTArchive_GetEntryAt (reference, index);
				if (entryp.Equals (IntPtr.Zero))
					return null;
				r.backend = (_TTArchiveEntry)Marshal.PtrToStructure (entryp, typeof(_TTArchiveEntry));
				r.reference = entryp;
				return r;
			}

			/// <summary>
			/// Clears the entries.
			/// </summary>
			public void ClearEntries(){
				Native.hTTArchive_ClearEntries (this.reference);
				UpdateAndSync (true);
			}

			/// <summary>
			/// Writes all entries with the given options to the outstream in this archive instance. Returns TTARCH_FLUSH_OK is all goes well (0), else see the constants value.
			/// </summary>
			public int Flush(){
				if (outstream == null)
					throw new LibTelltaleException ("No stream set");
				int ret = Native.TTArchive_Flush (this.reference,IntPtr.Zero);
				if (ret == Constants.TTARCH_FLUSH_OK) {
					UpdateAndSync (true);
				}
				return ret;
			}

			/// <summary>
			/// Releases all resources used by this <see cref="LibTelltale.TTArchives.TTArchive"/> object.
			/// This also frees all memory within the backend TTArchive and all its entries so make sure you call this after you need every entry otherwise you will get unmanaged memory errors.
			/// </summary>
			public void Dispose(){
				Native.TTArchive_Free (this.reference);
				Marshal.FreeHGlobal (this.reference);
				this.instream = null;
				this.outstream = null;
			}

			/// <summary>
			/// Opens and adds all entries from the given archive InStream. Do not use when you have previously loaded an archive, in that case use a new instance of this class.
			/// </summary>
			public int Open(){
				if (instream == null)
					throw new LibTelltaleException ("No stream set");
				int ret = Native.TTArchive_Open (this.reference);
				if (ret == Constants.TTARCH_OPEN_OK) {
					UpdateAndSync (true);
				}
				return ret;
			}

			/// <summary>
			/// The amount of entries in this archive.
			/// </summary>
			/// <returns>The entry count.</returns>
			public uint GetEntryCount(){
				return Native.hTTArchive_GetEntryCount (reference);
			}
				
			/// <summary>
			/// Gets or sets the input stream to read the archive from in Open
			/// </summary>
			/// <value>The in stream.</value>
			public ByteStream InStream {
				get{ return instream; }
				set { instream = value;  if(instream != null)this.handle.stream = instream.reference;  if(instream != null)UpdateAndSync (false); }
			}

			/// <summary>
			/// Gets or sets the output stream to write the archive to in Flush.
			/// </summary>
			/// <value>The out stream.</value>
			public ByteOutStream OutStream {
				get{ return outstream; }
				set { outstream = value; if(outstream != null)this.handle.flushstream = outstream.reference;  if(outstream != null)UpdateAndSync (false);}
			}

			protected void UpdateAndSync(bool retrieve){
				if (retrieve) {
					this.handle = (ttarch)Marshal.PtrToStructure (reference, typeof(ttarch));
				} else {
					Marshal.StructureToPtr (this.handle, reference, false);
				}
			}

		}

		/// <summary>
		/// Represents a file entry in a TTArchive2 (.ttarch2) archive.
		/// </summary>
		public struct TTArchive2Entry {
			public _TTArchive2Entry backend;
			public IntPtr reference;
		}

		/// <summary>
		/// Backend structure of an entry in memory. Do not edit fields, the only useful field in this struct is the name, which you can retrieve to your liking.
		/// </summary>
		public struct _TTArchive2Entry {
			public ulong offset;
			public uint size;
			public ulong name_crc;
			[MarshalAs(UnmanagedType.LPStr)]
			public string name;
			public IntPtr override_stream; // To override it, use set stream!
			public byte flags;
		}

		/// <summary>
		/// A .ttarch2 archive
		/// </summary>
		public sealed class TTArchive2 {

			/// <summary>
			/// Sets the name of the given entry. Do not use a direct set to the entry.backend.name!
			/// </summary>
			public static void SetEntryName(TTArchive2Entry entry, string name){
				Native.TTArchive2_EntrySetName (entry.reference, name);
			}

			/// <summary>
			/// Creates a TTArchive2 entry, with the given name and input stream of bytes.
			/// </summary>
			/// <returns>The TT archive2 entry.</returns>
			public static TTArchive2Entry CreateTTArchive2Entry(string name, ByteStream stream){
				TTArchive2Entry ret = new TTArchive2Entry();
				ret.reference = Native.TTArchive2_EntryCreate (name, stream.reference);
				ret.backend = (_TTArchive2Entry)Marshal.PtrToStructure (ret.reference, typeof(_TTArchive2Entry));
				return ret;
			}

			[StructLayout(LayoutKind.Sequential)]
			protected struct ttarch2 {
				public uint options;
				public IntPtr game_key;
				public IntPtr entries;
				public IntPtr stream;
				public IntPtr flushstream;
				public byte flags;
			};

			protected ttarch2 handle;

			public readonly IntPtr reference;

			protected ByteStream instream;

			protected ByteOutStream outstream;

			/// <summary>
			/// Gets the options for this archive.
			/// </summary>
			/// <returns>The archive options.</returns>
			public uint GetArchiveOptions(){
				UpdateAndSync (true);
				return this.handle.options;
			}

			/// <summary>
			/// Removes the entry from the archive.
			/// </summary>
			/// <param name="e">The Entry to remove. Must not be null</param>
			/// <param name="delete">If set to <c>true</c> then the entry will be freed to save memory. Use this only if you are removing and deleting the entry because you don't need it</param>
			public void RemoveEntry(TTArchive2Entry e, bool delete){
				Native.hTTArchive2_EntryRemove (this.reference, e.reference, delete);
			}

			/// <summary>
			/// Gets the archive version.
			/// </summary>
			/// <returns>The archive version.</returns>
			public uint GetArchiveVersion(){
				return (GetArchiveOptions () >> 7) & 15;
			}

			/// <summary>
			/// Sets the archive version.
			/// </summary>
			/// <param name="version">Version.</param>
			public void SetArchiveVersion(uint version){
				this.handle.options &= 63;
				this.handle.options |= (version & 15) << 7;
				UpdateAndSync (false);
			}

			/// <summary>
			/// Sets the archive an option from the constants class.
			/// </summary>
			/// <param name="op">Op.</param>
			public void SetArchiveOption(uint op){
				this.handle.options |= op;
				UpdateAndSync (false);
			}

			/// <summary>
			/// Unsets the archive option.
			/// </summary>
			/// <param name="op">Op.</param>
			public void UnsetArchiveOption(uint op){
				this.handle.options &= ~op;
				UpdateAndSync (false);
			}

			/// <summary>
			/// Initializes a new  <see cref="LibTelltale.TTArchives.TTArchive2"/>. The game ID should be the game ID from the github page, and will throw an exception in the case that the game for the ID
			/// could not be found.
			/// </summary>
			/// <param name="gameid">The Game ID</param>
			public TTArchive2(string gameid){
				reference = Native.hTTArchive2_Create ();
				if (this.reference.Equals (IntPtr.Zero))
					throw new LibTelltaleException ("Could not create backend archive");	
				this.UpdateAndSync (false);
				IntPtr key = Marshal.StringToHGlobalAnsi (gameid);
				this.handle.game_key = Config0.LibTelltale_GetKey (key);
				if (this.handle.game_key.Equals (IntPtr.Zero))
					throw new LibTelltaleException (String.Format("Could not find a key for the game ID {0}", gameid));
				Marshal.FreeHGlobal (key);
			}

			/// <summary>
			/// Streams the open.
			/// </summary>
			/// <returns>The open.</returns>
			/// <param name="entry">Entry.</param>
			public ByteStream StreamOpen(TTArchive2Entry entry){
				return new ByteStream (Native.TTArchive_StreamOpen (this.reference, entry.reference));
			}

			/// <summary>
			/// Finds an entry by its name
			/// </summary>
			/// <returns>The entry.</returns>
			/// <param name="name">Name.</param>
			public TTArchive2Entry? FindEntry(string name){
				TTArchive2Entry r = new TTArchive2Entry ();	
				IntPtr entryp = Native.TTArchive_EntryFind(reference, name);
				if (entryp.Equals (IntPtr.Zero))
					return null;
				r.backend = (_TTArchive2Entry)Marshal.PtrToStructure (entryp, typeof(_TTArchive2Entry));
				r.reference = entryp;
				return r;
			}

			/// <summary>
			/// Sets the input byte stream for the given entry.
			/// </summary>
			/// <param name="entry">Entry.</param>
			/// <param name="stream">Stream.</param>
			public void SetStreamForEntry(TTArchive2Entry entry, ByteStream stream){
				Native.TTArchive2_StreamSet (entry.reference, stream.reference);
				entry.backend = (_TTArchive2Entry)Marshal.PtrToStructure (entry.reference, typeof(_TTArchive2Entry));
			}

			/// <summary>
			/// Adds an entry to the archive
			/// </summary>
			/// <param name="entry">Entry.</param>
			public void AddEntry(TTArchive2Entry entry){
				Native.hTTArchive2_EntryAdd (this.reference, entry.reference);
				UpdateAndSync (true);
			}

			/// <summary>
			/// Gets an entry by its index in the archive entries backend list. Useful for iterating over all entries. It is nullable.
			/// </summary>
			/// <returns>The entry.</returns>
			/// <param name="index">Index.</param>
			public TTArchive2Entry? GetEntry(uint index){
				TTArchive2Entry r = new TTArchive2Entry ();
				IntPtr entryp = Native.hTTArchive2_GetEntryAt (reference, index);
				if (entryp.Equals (IntPtr.Zero))
					return null;
				r.backend = (_TTArchive2Entry)Marshal.PtrToStructure (entryp, typeof(_TTArchive2Entry));
				r.reference = entryp;
				return r;
			}

			/// <summary>
			/// Clears the entries in this archive.
			/// </summary>
			public void ClearEntries(){
				Native.hTTArchive2_ClearEntries (this.reference);
				UpdateAndSync (true);
			}

			/// <summary>
			/// Writes all entries with the options set in this archive to the OutStream as a valid .ttarch2.
			/// </summary>
			public int Flush(){
				if (outstream == null)
					throw new LibTelltaleException ("No stream set");
				int ret = Native.TTArchive2_Flush (this.reference,IntPtr.Zero);
				if (ret == Constants.TTARCH_FLUSH_OK) {
					UpdateAndSync (true);
				}
				return ret;
			}

			/// <summary>
			/// Releases all resources used by this <see cref="LibTelltale.TTArchives.TTArchive2"/> object.
			/// This also frees all memory within the backend TTArchive and all its entries so make sure you call this after you need every entry otherwise you will get unmanaged memory errors.
			/// </summary>
			public void Dispose(){
				Native.TTArchive_Free (this.reference);
				Marshal.FreeHGlobal (this.reference);
				this.instream = null;
				this.outstream = null;
			}

			/// <summary>
			/// Opens and adds all entries from the given .ttarch archive input stream. This also sets the options, and allows once the outstream is set for you to write back the archive to a stream.
			/// </summary>
			public int Open(){
				if (instream == null)
					throw new LibTelltaleException ("No stream set");
				int ret = Native.TTArchive2_Open (this.reference);
				if (ret == Constants.TTARCH_OPEN_OK) {
					UpdateAndSync (true);
				}
				return ret;
			}

			/// <summary>
			/// Gets the amount of entries in this archive.
			/// </summary>
			/// <returns>The entry count.</returns>
			public uint GetEntryCount(){
				return Native.hTTArchive2_GetEntryCount (reference);
			}

			/// <summary>
			/// Gets or sets the input stream to read the archive from in Open.
			/// </summary>
			/// <value>The in stream.</value>
			public ByteStream InStream {
				get{ return instream; }
				set { instream = value;  if(instream != null)this.handle.stream = instream.reference;  if(instream != null)UpdateAndSync (false); }
			}

			/// <summary>
			/// Gets or sets the output stream to write the archive to in Flush.
			/// </summary>
			/// <value>The out stream.</value>
			public ByteOutStream OutStream {
				get{ return outstream; }
				set { outstream = value; if(outstream != null)this.handle.flushstream = outstream.reference;  if(outstream != null)UpdateAndSync (false);}
			}

			protected void UpdateAndSync(bool retrieve){
				if (retrieve) {
					this.handle = (ttarch2)Marshal.PtrToStructure (reference, typeof(ttarch2));
				} else {
					Marshal.StructureToPtr (this.handle, reference, false);
				}
			}

		}

	}

	/// <summary>
	/// An input stream of bytes which can be read from and positioned.
	/// </summary>
	public sealed class ByteStream {

		/// <summary>
		/// The reference, do not touch!
		/// </summary>
		public readonly IntPtr reference = IntPtr.Zero;

		/// <summary>
		/// Used internally to create from the given stream pointer.
		/// </summary>
		/// <param name="r">The red component.</param>
		public ByteStream(IntPtr r){
			this.reference = r;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteStream"/> class. The filepath is the existing file location of the file to read from.
		/// </summary>
		/// <param name="filepath">Filepath.</param>
		public ByteStream(string filepath){
			this.reference = Native.hFileStream_Create (filepath);
			if (this.reference.Equals (IntPtr.Zero) || !IsValid())
				throw new LibTelltaleException ("Could not create backend filestream");	
		}

		/// <summary>
		/// Determines whether this instance is valid, and if it is not then the archive will return a stream error. This is if the backend byte buffer is non existent or the file does not exist.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
		public bool IsValid(){
			return Native.hByteStream_Valid (reference);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteStream"/> class, with the given size of zeros. Useful if you need an empty stream.
		/// </summary>
		/// <param name="initalSize">Inital size.</param>
		public ByteStream(uint initalSize){
			this.reference = Native.hByteStream_Create (initalSize);
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend stream");	
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteStream"/> class, which reads from the given input byte buffer. Do not set the byte array to null until you are done with this stream.
		/// </summary>
		/// <param name="buffer">Buffer.</param>
		public ByteStream(byte[] buffer){
			IntPtr ptr = Marshal.AllocHGlobal (buffer.Length);
			Marshal.Copy (buffer, 0, ptr, buffer.Length);
			this.reference = Native.hByteStream_CreateFromBuffer (ptr, (uint)buffer.Length);
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend stream");	
		}

		/// <summary>
		/// Reads a single byte.
		/// </summary>
		/// <returns>The single byte.</returns>
		public byte ReadSingleByte(){
			return Native.hByteStream_ReadByte (reference);
		}

		/// <summary>
		/// Gets the backend buffer this stream is reading from, or null if there is none (for example if its reading from a file).
		/// </summary>
		/// <returns>The buffer.</returns>
		public byte[] GetBuffer(){
			IntPtr ptr = Native.hByteStream_GetBuffer (reference);
			if (ptr.Equals (IntPtr.Zero))
				return null;
			byte[] ret = new byte[GetSize ()];
			Marshal.Copy (ptr,ret, 0, (int)GetSize ());
			//Dont want to free the buffer!!
			return ret;
		}
			
		/// <summary>
		/// Read the specified amount of bytes, and increases the position by length.
		/// </summary>
		/// <param name="length">Length.</param>
		public byte[] Read(int length){
			IntPtr read = Native.hByteStream_ReadBytes (reference, (uint)length);
			if (read.Equals (IntPtr.Zero))
				return null;
			byte[] ret = new byte[length];
			Marshal.Copy (read, ret, 0, length);
			return ret;
		}

		/// <summary>
		/// Reads an unsigned integer of the specified bit width. Valid widths are <= 64 and a multiple of eight and >= 8.
		/// </summary>
		public ulong ReadInt(byte bit_width){
			return Native.hByteStream_ReadInt(reference, bit_width);
		}

		/// <summary>
		/// Reads a null terminated string.
		/// </summary>
		/// <returns>The null terminated string.</returns>
		public string ReadNullTerminatedString(){
			return Marshal.PtrToStringAnsi (Native.hByteStream_ReadString0 (reference));
		} 

		/// <summary>
		/// Reads an ASCII string of the given byte length
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="len">Length.</param>
		public string ReadString(uint len){
			return Marshal.PtrToStringAnsi(Native.hByteStream_ReadString(reference, len));
		}

		/// <summary>
		/// Determines whether this instance is little endian.
		/// </summary>
		/// <returns><c>true</c> if this instance is little endian; otherwise, <c>false</c>.</returns>
		public bool IsLittleEndian(){
			return Native.hByteStream_IsLittleEndian (reference);
		}

		/// <summary>
		/// Positions to the specified offset.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public void Position(ulong offset){
			Native.hByteStream_Position (reference, offset);
		}

		/// <summary>
		/// Sets the endianness of the stream to read ints by.
		/// </summary>
		/// <param name="little">If set to <c>true</c> little.</param>
		public void SetEndian(bool little){
			Native.hByteStream_SetEndian (reference, little);
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <returns>The position.</returns>
		public ulong GetPosition(){
			return Native.hByteStream_GetPosition (reference);
		}

		/// <summary>
		/// Gets the size of this stream.
		/// </summary>
		/// <returns>The size.</returns>
		public int GetSize(){
			return (int)Native.hByteStream_GetSize (reference);
		}

	}

	/// <summary>
	/// An output stream of bytes
	/// </summary>
	public sealed class ByteOutStream {

		//Do not touch this!
		public readonly IntPtr reference = IntPtr.Zero;

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteOutStream"/> class, which writes to the passed in file which doesnt have to exist. However its directory must exist of undefined behaviour!
		/// </summary>
		/// <param name="filepath">Filepath.</param>
		public ByteOutStream(string filepath){
			this.reference = Native.hFileOutStream_Create (filepath);
			if (this.reference.Equals (IntPtr.Zero) || !IsValid())
				throw new LibTelltaleException ("Could not create backend filestream");	
		}

		/// <summary>
		/// Determines whether this stream is valid.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
		public bool IsValid(){
			return Native.hByteOutStream_Valid (reference);
		}

		/// <summary>
		/// Writes an unsigned int to this stream. You will have to cast to a ulong on calling if not a uint64!
		/// </summary>
		/// <param name="bit_width">Bit width.</param>
		/// <param name="num">Number.</param>
		public void WriteInt(byte bit_width, ulong num){
			Native.hByteOutStream_WriteInt (reference, bit_width, num);
		}

		/// <summary>
		/// Write the specified buffer to this stream.
		/// </summary>
		/// <param name="buf">Buffer.</param>
		public void Write(byte[] buf){
			IntPtr ptr = Marshal.AllocHGlobal (buf.Length);
			Marshal.Copy (buf, 0, ptr, buf.Length);
			Native.hByteOutStream_WriteBytes (reference, ptr, (uint)buf.Length);
			Marshal.FreeHGlobal (ptr);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LibTelltale.ByteOutStream"/> class, with the initial buffer size to save time reallocating the memory.
		/// </summary>
		/// <param name="initalSize">Inital size.</param>
		public ByteOutStream(uint initalSize){
			this.reference = Native.hByteOutStream_Create (initalSize);
			if (this.reference.Equals (IntPtr.Zero))
				throw new LibTelltaleException ("Could not create backend stream");	
		}

		/// <summary>
		/// Determines whether this instance is little endian.
		/// </summary>
		/// <returns><c>true</c> if this instance is little endian; otherwise, <c>false</c>.</returns>
		public bool IsLittleEndian(){
			return Native.hByteOutStream_IsLittleEndian (reference);
		}

		/// <summary>
		/// Gets the backend buffer if this is not writing to a file. This is useful if you are writing to a byte[] and you want to get the output array.
		/// </summary>
		/// <returns>The buffer.</returns>
		public byte[] GetBuffer(){
			IntPtr ptr = Native.hByteOutStream_GetBuffer (reference);
			if (ptr.Equals (IntPtr.Zero))
				return null;
			byte[] ret = new byte[GetSize ()];
			Marshal.Copy (ptr,ret, 0, (int)GetSize ());
			//Dont want to free the buffer!!
			return ret;
		}

		/// <summary>
		/// Position to the specified offset.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public void Position(ulong offset){
			Native.hByteOutStream_Position (reference, offset);
		}

		/// <summary>
		/// Sets the endian.
		/// </summary>
		public void SetEndian(bool little){
			Native.hByteOutStream_SetEndian (reference, little);
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <returns>The position.</returns>
		public ulong GetPosition(){
			return Native.hByteOutStream_GetPosition (reference);
		}

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <returns>The size.</returns>
		public ulong GetSize(){
			return Native.hByteOutStream_GetSize (reference);
		}

	}

	/// <summary>
	/// Native access to the telltale library DLL. This is a private class because it is used internally.
	/// </summary>
	class Native {

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_EntryRemove (IntPtr a, IntPtr b, bool c);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive2_EntryRemove (IntPtr a, IntPtr b, bool c);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_EntrySetName (IntPtr a, [MarshalAs(UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive2_Open (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive2_StreamOpen (IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive2_Free (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive2_Flush (IntPtr a,IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive2_StreamSet (IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive2_EntrySetName (IntPtr a, [MarshalAs(UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive2_EntryFind (IntPtr a, [MarshalAs(UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive2_EntryCreate ([MarshalAs(UnmanagedType.LPStr)] string name, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive_StreamOpen(IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive_Open (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive_Free (IntPtr a);

		[DllImport("LibTelltale.dll")]
		public static extern int TTArchive_Flush(IntPtr a, IntPtr func);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive_EntryCreate([MarshalAs(UnmanagedType.LPStr)] string name, IntPtr strm);

		[DllImport("LibTelltale.dll")]
		public static extern void TTArchive_StreamSet (IntPtr a, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr TTArchive_EntryFind (IntPtr a, [MarshalAs (UnmanagedType.LPStr)] string name);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteStream_Position(IntPtr stream, ulong off);

		[DllImport("LibTelltale.dll")]
		public static extern bool hByteStream_IsLittleEndian(IntPtr stream);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteStream_SetEndian(IntPtr s, bool little_endian);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteStream_ReadInt(IntPtr s, uint bitwidth);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteStream_GetPosition(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteStream_GetSize(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_ReadBytes(IntPtr s, uint size);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive2_EntryAdd(IntPtr p, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_EntryAdd(IntPtr p, IntPtr b);

		[DllImport("LibTelltale.dll")]
		public static extern byte hByteStream_ReadByte(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_Create (uint size);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_CreateFromBuffer (IntPtr p, uint size);

		[DllImport("LibTelltale.dll") ]
		public static extern IntPtr hByteStream_ReadString(IntPtr s, uint len);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_ReadString0(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hFileStream_Create([MarshalAs(UnmanagedType.LPStr)] string filepath);

		[DllImport("LibTelltale.dll")]
		public static extern bool hByteStream_Valid(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern bool hByteOutStream_Valid(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern uint hTTArchive2_GetEntryCount(IntPtr archive);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive2_GetEntryAt (IntPtr archive, uint index);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive_GetEntryAt (IntPtr archive, uint index);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive2_ClearEntries (IntPtr archive);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_Position(IntPtr stream, ulong off);

		[DllImport("LibTelltale.dll")]
		public static extern bool hByteOutStream_IsLittleEndian(IntPtr stream);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_SetEndian(IntPtr s, bool little_endian);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_WriteInt (IntPtr s, uint bitwidth, ulong num);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteOutStream_GetPosition(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteOutStream_GetBuffer(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteStream_GetBuffer(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern ulong hByteOutStream_GetSize(IntPtr s);

		[DllImport("LibTelltale.dll")]
		public static extern void hByteOutStream_WriteBytes(IntPtr s, IntPtr buf, uint size);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hByteOutStream_Create (uint size);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hFileOutStream_Create([MarshalAs(UnmanagedType.LPStr)] string filepath);

		[DllImport("LibTelltale.dll")]
		public static extern void hTTArchive_ClearEntries (IntPtr p);

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive2_Create ();

		[DllImport("LibTelltale.dll")]
		public static extern IntPtr hTTArchive_Create ();

		[DllImport("LibTelltale.dll")]
		public static extern uint hTTArchive_GetEntryCount(IntPtr archive);

	}

}
