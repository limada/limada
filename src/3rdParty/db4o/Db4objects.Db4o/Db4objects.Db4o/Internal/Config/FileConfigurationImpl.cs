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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Config
{
	internal class FileConfigurationImpl : IFileConfiguration
	{
		private readonly Config4Impl _config;

		public FileConfigurationImpl(Config4Impl config)
		{
			_config = config;
		}

		public virtual int BlockSize
		{
			set
			{
				int bytes = value;
				_config.BlockSize(bytes);
			}
		}

		public virtual int DatabaseGrowthSize
		{
			set
			{
				int bytes = value;
				_config.DatabaseGrowthSize(bytes);
			}
		}

		public virtual void DisableCommitRecovery()
		{
			_config.DisableCommitRecovery();
		}

		public virtual IFreespaceConfiguration Freespace
		{
			get
			{
				return _config.Freespace();
			}
		}

		public virtual ConfigScope GenerateUUIDs
		{
			set
			{
				ConfigScope setting = value;
				_config.GenerateUUIDs(setting);
			}
		}

		public virtual ConfigScope GenerateVersionNumbers
		{
			set
			{
				ConfigScope setting = value;
				_config.GenerateVersionNumbers(setting);
			}
		}

		public virtual bool GenerateCommitTimestamps
		{
			set
			{
				bool setting = value;
				_config.GenerateCommitTimestamps(setting);
			}
		}

		/// <exception cref="Db4objects.Db4o.Config.GlobalOnlyConfigException"></exception>
		public virtual IStorage Storage
		{
			get
			{
				return _config.Storage;
			}
			set
			{
				IStorage factory = value;
				_config.Storage = factory;
			}
		}

		public virtual bool LockDatabaseFile
		{
			set
			{
				bool flag = value;
				_config.LockDatabaseFile(flag);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		public virtual long ReserveStorageSpace
		{
			set
			{
				long byteCount = value;
				_config.ReserveStorageSpace(byteCount);
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual string BlobPath
		{
			set
			{
				string path = value;
				_config.SetBlobPath(path);
			}
		}

		public virtual bool ReadOnly
		{
			set
			{
				bool flag = value;
				_config.ReadOnly(flag);
			}
		}

		public virtual bool RecoveryMode
		{
			set
			{
				bool flag = value;
				_config.RecoveryMode(flag);
			}
		}

		public virtual bool AsynchronousSync
		{
			set
			{
				bool flag = value;
				_config.AsynchronousSync(flag);
			}
		}
	}
}
