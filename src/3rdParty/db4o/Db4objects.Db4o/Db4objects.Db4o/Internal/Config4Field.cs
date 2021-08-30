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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	public class Config4Field : Config4Abstract, IObjectField, IDeepClone
	{
		private readonly Config4Class _configClass;

		private bool _used;

		private static readonly KeySpec IndexedKey = new KeySpec(TernaryBool.Unspecified);

		protected Config4Field(Config4Class a_class, KeySpecHashtable4 config) : base(config
			)
		{
			_configClass = a_class;
		}

		internal Config4Field(Config4Class a_class, string a_name)
		{
			_configClass = a_class;
			SetName(a_name);
		}

		private Config4Class ClassConfig()
		{
			return _configClass;
		}

		internal override string ClassName()
		{
			return ClassConfig().GetName();
		}

		public virtual object DeepClone(object param)
		{
			return new Db4objects.Db4o.Internal.Config4Field((Config4Class)param, _config);
		}

		public virtual void Rename(string newName)
		{
			ClassConfig().Config().Rename(Renames.ForField(ClassName(), GetName(), newName));
			SetName(newName);
		}

		public virtual void Indexed(bool flag)
		{
			PutThreeValued(IndexedKey, flag);
		}

		public virtual void InitOnUp(Transaction systemTrans, FieldMetadata fieldMetadata
			)
		{
			ObjectContainerBase anyStream = systemTrans.Container();
			if (!anyStream.MaintainsIndices())
			{
				return;
			}
			if (!fieldMetadata.SupportsIndex())
			{
				Indexed(false);
			}
			TernaryBool indexedFlag = _config.GetAsTernaryBool(IndexedKey);
			if (indexedFlag.DefiniteNo())
			{
				fieldMetadata.DropIndex((LocalTransaction)systemTrans);
				return;
			}
			if (UseExistingIndex(systemTrans, fieldMetadata))
			{
				return;
			}
			if (!indexedFlag.DefiniteYes())
			{
				return;
			}
			fieldMetadata.CreateIndex();
		}

		private bool UseExistingIndex(Transaction systemTrans, FieldMetadata fieldMetadata
			)
		{
			return fieldMetadata.GetIndex(systemTrans) != null;
		}

		public virtual void Used(bool flag)
		{
			_used = flag;
		}

		public virtual bool Used()
		{
			return _used;
		}
	}
}
