/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2010  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal.Fileheader;
using Sharpen;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// CachedIoAdapter is an IOAdapter for random access files, which caches data
	/// for IO access.
	/// </summary>
	/// <remarks>
	/// CachedIoAdapter is an IOAdapter for random access files, which caches data
	/// for IO access. Its functionality is similar to OS cache.<br/>
	/// Example:<br/>
	/// <code>delegateAdapter = new RandomAccessFileAdapter();</code><br/>
	/// <code>config.Io(new CachedIoAdapter(delegateAdapter));</code><br/>
	/// </remarks>
	public class CachedIoAdapter : IoAdapter
	{
		private CachedIoAdapter.Page _head;

		private CachedIoAdapter.Page _tail;

		private long _position;

		private int _pageSize;

		private int _pageCount;

		private long _fileLength;

		private long _filePointer;

		private IoAdapter _io;

		private bool _readOnly;

		private static int DefaultPageSize = 1024;

		private static int DefaultPageCount = 64;

		/// <summary>
		/// Creates an instance of CachedIoAdapter with the default page size and
		/// page count.
		/// </summary>
		/// <remarks>
		/// Creates an instance of CachedIoAdapter with the default page size and
		/// page count.
		/// </remarks>
		/// <param name="ioAdapter">delegate IO adapter (RandomAccessFileAdapter by default)</param>
		public CachedIoAdapter(IoAdapter ioAdapter) : this(ioAdapter, DefaultPageSize, DefaultPageCount
			)
		{
		}

		/// <summary>
		/// Creates an instance of CachedIoAdapter with a custom page size and page
		/// count.<br />
		/// </summary>
		/// <param name="ioAdapter">delegate IO adapter (RandomAccessFileAdapter by default)</param>
		/// <param name="pageSize">cache page size</param>
		/// <param name="pageCount">allocated amount of pages</param>
		public CachedIoAdapter(IoAdapter ioAdapter, int pageSize, int pageCount)
		{
			// private Hashtable4 _posPageMap = new Hashtable4(PAGE_COUNT);
			_io = ioAdapter;
			_pageSize = pageSize;
			_pageCount = pageCount;
		}

		/// <summary>Creates an instance of CachedIoAdapter with extended parameters.<br /></summary>
		/// <param name="path">database file path</param>
		/// <param name="lockFile">determines if the file should be locked</param>
		/// <param name="initialLength">initial file length, new writes will start from this point
		/// 	</param>
		/// <param name="readOnly">
		/// 
		/// if the file should be used in read-onlyt mode.
		/// </param>
		/// <param name="io">delegate IO adapter (RandomAccessFileAdapter by default)</param>
		/// <param name="pageSize">cache page size</param>
		/// <param name="pageCount">allocated amount of pages</param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public CachedIoAdapter(string path, bool lockFile, long initialLength, bool readOnly
			, IoAdapter io, int pageSize, int pageCount)
		{
			_readOnly = readOnly;
			_pageSize = pageSize;
			_pageCount = pageCount;
			InitCache();
			InitIOAdaptor(path, lockFile, initialLength, readOnly, io);
			_position = initialLength;
			_filePointer = initialLength;
			_fileLength = _io.GetLength();
		}

		/// <summary>Creates and returns a new CachedIoAdapter <br /></summary>
		/// <param name="path">database file path</param>
		/// <param name="lockFile">determines if the file should be locked</param>
		/// <param name="initialLength">initial file length, new writes will start from this point
		/// 	</param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override IoAdapter Open(string path, bool lockFile, long initialLength, bool
			 readOnly)
		{
			return new Db4objects.Db4o.IO.CachedIoAdapter(path, lockFile, initialLength, readOnly
				, _io, _pageSize, _pageCount);
		}

		/// <summary>Deletes the database file</summary>
		/// <param name="path">file path</param>
		public override void Delete(string path)
		{
			_io.Delete(path);
		}

		/// <summary>Checks if the file exists</summary>
		/// <param name="path">file path</param>
		public override bool Exists(string path)
		{
			return _io.Exists(path);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void InitIOAdaptor(string path, bool lockFile, long initialLength, bool readOnly
			, IoAdapter io)
		{
			_io = io.Open(path, lockFile, initialLength, readOnly);
		}

		private void InitCache()
		{
			_head = new CachedIoAdapter.Page(_pageSize);
			_head._prev = null;
			CachedIoAdapter.Page page = _head;
			CachedIoAdapter.Page next = _head;
			for (int i = 0; i < _pageCount - 1; ++i)
			{
				next = new CachedIoAdapter.Page(_pageSize);
				page._next = next;
				next._prev = page;
				page = next;
			}
			_tail = next;
		}

		/// <summary>Reads the file into the buffer using pages from cache.</summary>
		/// <remarks>
		/// Reads the file into the buffer using pages from cache. If the next page
		/// is not cached it will be read from the file.
		/// </remarks>
		/// <param name="buffer">destination buffer</param>
		/// <param name="length">how many bytes to read</param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override int Read(byte[] buffer, int length)
		{
			long startAddress = _position;
			int bytesToRead = length;
			int totalRead = 0;
			while (bytesToRead > 0)
			{
				CachedIoAdapter.Page page = GetPage(startAddress, true);
				int readBytes = page.Read(buffer, totalRead, startAddress, bytesToRead);
				MovePageToHead(page);
				if (readBytes <= 0)
				{
					break;
				}
				bytesToRead -= readBytes;
				startAddress += readBytes;
				totalRead += readBytes;
			}
			_position = startAddress;
			return totalRead == 0 ? -1 : totalRead;
		}

		/// <summary>Writes the buffer to cache using pages</summary>
		/// <param name="buffer">source buffer</param>
		/// <param name="length">how many bytes to write</param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Write(byte[] buffer, int length)
		{
			ValidateReadOnly();
			long startAddress = _position;
			int bytesToWrite = length;
			int bufferOffset = 0;
			while (bytesToWrite > 0)
			{
				// page doesn't need to loadFromDisk if the whole page is dirty
				bool loadFromDisk = (bytesToWrite < _pageSize) || (startAddress % _pageSize != 0);
				CachedIoAdapter.Page page = GetPage(startAddress, loadFromDisk);
				page.EnsureEndAddress(GetLength());
				int writtenBytes = page.Write(buffer, bufferOffset, startAddress, bytesToWrite);
				FlushIfHeaderBlockPage(page);
				MovePageToHead(page);
				bytesToWrite -= writtenBytes;
				startAddress += writtenBytes;
				bufferOffset += writtenBytes;
			}
			long endAddress = startAddress;
			_position = endAddress;
			_fileLength = Math.Max(endAddress, _fileLength);
		}

		private void FlushIfHeaderBlockPage(CachedIoAdapter.Page page)
		{
			if (ContainsHeaderBlock(page))
			{
				FlushPage(page);
			}
		}

		private void ValidateReadOnly()
		{
			if (_readOnly)
			{
				throw new Db4oIOException();
			}
		}

		/// <summary>Flushes cache to a physical storage</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Sync()
		{
			ValidateReadOnly();
			FlushAllPages();
			_io.Sync();
		}

		/// <summary>Returns the file length</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override long GetLength()
		{
			return _fileLength;
		}

		/// <summary>Flushes and closes the file</summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Close()
		{
			try
			{
				FlushAllPages();
			}
			finally
			{
				_io.Close();
			}
		}

		public override IoAdapter DelegatedIoAdapter()
		{
			return _io.DelegatedIoAdapter();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private CachedIoAdapter.Page GetPage(long startAddress, bool loadFromDisk)
		{
			CachedIoAdapter.Page page = GetPageFromCache(startAddress);
			if (page != null)
			{
				if (ContainsHeaderBlock(page))
				{
					GetPageFromDisk(page, startAddress);
				}
				page.EnsureEndAddress(_fileLength);
				return page;
			}
			// in case that page is not found in the cache
			page = GetFreePageFromCache();
			if (loadFromDisk)
			{
				GetPageFromDisk(page, startAddress);
			}
			else
			{
				ResetPageAddress(page, startAddress);
			}
			return page;
		}

		private bool ContainsHeaderBlock(CachedIoAdapter.Page page)
		{
			return page.StartAddress() <= FileHeader1.HeaderLength;
		}

		private void ResetPageAddress(CachedIoAdapter.Page page, long startAddress)
		{
			page.StartAddress(startAddress);
			page.EndAddress(startAddress + _pageSize);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private CachedIoAdapter.Page GetFreePageFromCache()
		{
			if (!_tail.IsFree())
			{
				FlushPage(_tail);
			}
			// _posPageMap.remove(new Long(tail.startPosition / PAGE_SIZE));
			return _tail;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private CachedIoAdapter.Page GetPageFromCache(long pos)
		{
			CachedIoAdapter.Page page = _head;
			while (page != null)
			{
				if (page.Contains(pos))
				{
					return page;
				}
				page = page._next;
			}
			return null;
		}

		// Page page = (Page) _posPageMap.get(new Long(pos/PAGE_SIZE));
		// return page;
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void FlushAllPages()
		{
			CachedIoAdapter.Page node = _head;
			while (node != null)
			{
				FlushPage(node);
				node = node._next;
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void FlushPage(CachedIoAdapter.Page page)
		{
			if (!page._dirty)
			{
				return;
			}
			IoSeek(page.StartAddress());
			WritePageToDisk(page);
			return;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void GetPageFromDisk(CachedIoAdapter.Page page, long pos)
		{
			long startAddress = pos - pos % _pageSize;
			page.StartAddress(startAddress);
			IoSeek(page._startAddress);
			int count = IoRead(page);
			if (count > 0)
			{
				page.EndAddress(startAddress + count);
			}
			else
			{
				page.EndAddress(startAddress);
			}
		}

		// _posPageMap.put(new Long(page.startPosition / PAGE_SIZE), page);
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private int IoRead(CachedIoAdapter.Page page)
		{
			int count = _io.Read(page._buffer);
			if (count > 0)
			{
				_filePointer = page._startAddress + count;
			}
			return count;
		}

		private void MovePageToHead(CachedIoAdapter.Page page)
		{
			if (page == _head)
			{
				return;
			}
			if (page == _tail)
			{
				CachedIoAdapter.Page tempTail = _tail._prev;
				tempTail._next = null;
				_tail._next = _head;
				_tail._prev = null;
				_head._prev = page;
				_head = _tail;
				_tail = tempTail;
			}
			else
			{
				page._prev._next = page._next;
				page._next._prev = page._prev;
				page._next = _head;
				_head._prev = page;
				page._prev = null;
				_head = page;
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void WritePageToDisk(CachedIoAdapter.Page page)
		{
			ValidateReadOnly();
			try
			{
				_io.Write(page._buffer, page.Size());
				_filePointer = page.EndAddress();
				page._dirty = false;
			}
			catch (Db4oIOException e)
			{
				_readOnly = true;
				throw;
			}
		}

		/// <summary>Moves the pointer to the specified file position</summary>
		/// <param name="pos">position within the file</param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Seek(long pos)
		{
			_position = pos;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		private void IoSeek(long pos)
		{
			if (_filePointer != pos)
			{
				_io.Seek(pos);
				_filePointer = pos;
			}
		}

		private class Page
		{
			internal byte[] _buffer;

			internal long _startAddress = -1;

			internal long _endAddress;

			internal readonly int _bufferSize;

			internal bool _dirty;

			internal CachedIoAdapter.Page _prev;

			internal CachedIoAdapter.Page _next;

			private byte[] zeroBytes;

			public Page(int size)
			{
				_bufferSize = size;
				_buffer = new byte[_bufferSize];
			}

			internal virtual void EnsureEndAddress(long fileLength)
			{
				long bufferEndAddress = _startAddress + _bufferSize;
				if (_endAddress < bufferEndAddress && fileLength > _endAddress)
				{
					long newEndAddress = Math.Min(fileLength, bufferEndAddress);
					if (zeroBytes == null)
					{
						zeroBytes = new byte[_bufferSize];
					}
					System.Array.Copy(zeroBytes, 0, _buffer, (int)(_endAddress - _startAddress), (int
						)(newEndAddress - _endAddress));
					_endAddress = newEndAddress;
				}
			}

			internal virtual long EndAddress()
			{
				return _endAddress;
			}

			internal virtual void StartAddress(long address)
			{
				_startAddress = address;
			}

			internal virtual long StartAddress()
			{
				return _startAddress;
			}

			internal virtual void EndAddress(long address)
			{
				_endAddress = address;
			}

			internal virtual int Size()
			{
				return (int)(_endAddress - _startAddress);
			}

			internal virtual int Read(byte[] @out, int outOffset, long startAddress, int length
				)
			{
				int bufferOffset = (int)(startAddress - _startAddress);
				int pageAvailbeDataSize = (int)(_endAddress - startAddress);
				int readBytes = Math.Min(pageAvailbeDataSize, length);
				if (readBytes <= 0)
				{
					// meaning reach EOF
					return -1;
				}
				System.Array.Copy(_buffer, bufferOffset, @out, outOffset, readBytes);
				return readBytes;
			}

			internal virtual int Write(byte[] data, int dataOffset, long startAddress, int length
				)
			{
				int bufferOffset = (int)(startAddress - _startAddress);
				int pageAvailabeBufferSize = _bufferSize - bufferOffset;
				int writtenBytes = Math.Min(pageAvailabeBufferSize, length);
				System.Array.Copy(data, dataOffset, _buffer, bufferOffset, writtenBytes);
				long endAddress = startAddress + writtenBytes;
				if (endAddress > _endAddress)
				{
					_endAddress = endAddress;
				}
				_dirty = true;
				return writtenBytes;
			}

			internal virtual bool Contains(long address)
			{
				return (_startAddress != -1 && address >= _startAddress && address < _startAddress
					 + _bufferSize);
			}

			internal virtual bool IsFree()
			{
				return _startAddress == -1;
			}
		}
	}
}
