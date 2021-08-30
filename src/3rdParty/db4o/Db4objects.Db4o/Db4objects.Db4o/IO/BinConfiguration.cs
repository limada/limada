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
namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	public class BinConfiguration
	{
		private readonly string _uri;

		private readonly bool _lockFile;

		private readonly long _initialLength;

		private readonly bool _readOnly;

		private readonly int _blockSize;

		public BinConfiguration(string uri, bool lockFile, long initialLength, bool readOnly
			) : this(uri, lockFile, initialLength, readOnly, 1)
		{
		}

		public BinConfiguration(string uri, bool lockFile, long initialLength, bool readOnly
			, int blockSize)
		{
			_uri = uri;
			_lockFile = lockFile;
			_initialLength = initialLength;
			_readOnly = readOnly;
			_blockSize = blockSize;
		}

		public virtual string Uri()
		{
			return _uri;
		}

		public virtual bool LockFile()
		{
			return _lockFile;
		}

		public virtual long InitialLength()
		{
			return _initialLength;
		}

		public virtual bool ReadOnly()
		{
			return _readOnly;
		}

		public virtual int BlockSize()
		{
			return _blockSize;
		}

		public override string ToString()
		{
			return "BinConfiguration(Uri: " + _uri + ", Locked: " + _lockFile + ", ReadOnly: "
				 + _readOnly + ", BlockSize: " + _blockSize + ")";
		}
	}
}
