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
using System.Collections;
using Db4objects.Db4o.IO;
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	internal class PagingMemoryBin : IBin
	{
		private readonly int _pageSize;

		private IList _pages = new ArrayList();

		private int _lastPageLength;

		public PagingMemoryBin(int pageSize) : this(pageSize, 0)
		{
		}

		public PagingMemoryBin(int pageSize, long initialLength)
		{
			_pageSize = pageSize;
			EnsureLength(initialLength);
		}

		public virtual long Length()
		{
			if (_pages.Count == 0)
			{
				return 0;
			}
			return (_pages.Count - 1) * _pageSize + _lastPageLength;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual int Read(long pos, byte[] buffer, int length)
		{
			long avail = Length() - pos;
			if (avail <= 0)
			{
				return -1;
			}
			int bytesToRead = Math.Min((int)avail, length);
			int offset = PageOffset(pos);
			int pageIdx = PageIdx(pos);
			int bytesRead = 0;
			while (bytesRead < bytesToRead)
			{
				byte[] curPage = ((byte[])_pages[pageIdx]);
				int chunkLength = Math.Min(length - bytesRead, _pageSize - offset);
				System.Array.Copy(curPage, offset, buffer, bytesRead, chunkLength);
				bytesRead += chunkLength;
				pageIdx++;
				offset = 0;
			}
			return bytesToRead;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Sync()
		{
		}

		public virtual int SyncRead(long position, byte[] bytes, int bytesToRead)
		{
			return Read(position, bytes, bytesToRead);
		}

		public virtual void Close()
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Write(long pos, byte[] buffer, int length)
		{
			EnsureLength(pos + length);
			int offset = PageOffset(pos);
			int pageIdx = PageIdx(pos);
			int bytesWritten = 0;
			while (bytesWritten < length)
			{
				byte[] curPage = ((byte[])_pages[pageIdx]);
				int chunkLength = Math.Min(length - bytesWritten, _pageSize - offset);
				System.Array.Copy(buffer, bytesWritten, curPage, offset, chunkLength);
				bytesWritten += chunkLength;
				pageIdx++;
				offset = 0;
			}
		}

		private void EnsureLength(long length)
		{
			if (length <= 0)
			{
				return;
			}
			long lastPos = length - 1;
			int lastPosPageIdx = PageIdx(lastPos);
			int lastPosPageLength = PageOffset(lastPos) + 1;
			if (lastPosPageIdx == _pages.Count - 1)
			{
				_lastPageLength = Math.Max(lastPosPageLength, _lastPageLength);
				return;
			}
			if (lastPosPageIdx < _pages.Count)
			{
				return;
			}
			for (int newPageIdx = _pages.Count; newPageIdx <= lastPosPageIdx; newPageIdx++)
			{
				_pages.Add(new byte[_pageSize]);
			}
			_lastPageLength = lastPosPageLength;
		}

		private int PageIdx(long pos)
		{
			return (int)(pos / _pageSize);
		}

		private int PageOffset(long pos)
		{
			return (int)(pos % _pageSize);
		}

		public virtual void Sync(IRunnable runnable)
		{
			runnable.Run();
		}
	}
}
