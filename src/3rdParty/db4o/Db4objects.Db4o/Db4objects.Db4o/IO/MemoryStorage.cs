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
using System.Collections;
using System.IO;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// <see cref="IStorage">IStorage</see>
	/// implementation that produces
	/// <see cref="IBin">IBin</see>
	/// instances
	/// that operate in memory.
	/// Use this
	/// <see cref="IStorage">IStorage</see>
	/// to work with db4o as an in-memory database.
	/// </summary>
	public class MemoryStorage : IStorage
	{
		private readonly IDictionary _bins = new Hashtable();

		private readonly IGrowthStrategy _growthStrategy;

		public MemoryStorage() : this(new DoublingGrowthStrategy())
		{
		}

		public MemoryStorage(IGrowthStrategy growthStrategy)
		{
			_growthStrategy = growthStrategy;
		}

		/// <summary>
		/// returns true if a MemoryBin with the given URI name already exists
		/// in this Storage.
		/// </summary>
		/// <remarks>
		/// returns true if a MemoryBin with the given URI name already exists
		/// in this Storage.
		/// </remarks>
		public virtual bool Exists(string uri)
		{
			return _bins.Contains(uri);
		}

		/// <summary>opens a MemoryBin for the given URI (name can be freely chosen).</summary>
		/// <remarks>opens a MemoryBin for the given URI (name can be freely chosen).</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual IBin Open(BinConfiguration config)
		{
			IBin storage = ProduceStorage(config);
			return config.ReadOnly() ? new ReadOnlyBin(storage) : storage;
		}

		/// <summary>Returns the memory bin for the given URI for external use.</summary>
		/// <remarks>Returns the memory bin for the given URI for external use.</remarks>
		public virtual MemoryBin Bin(string uri)
		{
			return ((MemoryBin)_bins[uri]);
		}

		/// <summary>Registers the given bin for this storage with the given URI.</summary>
		/// <remarks>Registers the given bin for this storage with the given URI.</remarks>
		public virtual void Bin(string uri, MemoryBin bin)
		{
			_bins[uri] = bin;
		}

		private IBin ProduceStorage(BinConfiguration config)
		{
			IBin storage = Bin(config.Uri());
			if (null != storage)
			{
				return storage;
			}
			MemoryBin newStorage = new MemoryBin(new byte[(int)config.InitialLength()], _growthStrategy
				);
			_bins[config.Uri()] = newStorage;
			return newStorage;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Delete(string uri)
		{
			Sharpen.Collections.Remove(_bins, uri);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Rename(string oldUri, string newUri)
		{
			MemoryBin bin = ((MemoryBin)Sharpen.Collections.Remove(_bins, oldUri));
			if (bin == null)
			{
				throw new IOException("Bin not found: " + oldUri);
			}
			_bins[newUri] = bin;
		}
	}
}
