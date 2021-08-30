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
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>Wrapper base class for all classes that wrap Storage.</summary>
	/// <remarks>
	/// Wrapper base class for all classes that wrap Storage.
	/// Each class that adds functionality to a Storage must
	/// extend this class.
	/// </remarks>
	/// <seealso cref="BinDecorator"></seealso>
	public class StorageDecorator : IStorage
	{
		protected readonly IStorage _storage;

		public StorageDecorator(IStorage storage)
		{
			_storage = storage;
		}

		public virtual bool Exists(string uri)
		{
			return _storage.Exists(uri);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual IBin Open(BinConfiguration config)
		{
			return Decorate(config, _storage.Open(config));
		}

		protected virtual IBin Decorate(BinConfiguration config, IBin bin)
		{
			return bin;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Delete(string uri)
		{
			_storage.Delete(uri);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Rename(string oldUri, string newUri)
		{
			_storage.Rename(oldUri, newUri);
		}
	}
}
