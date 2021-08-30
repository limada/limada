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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class StoredFieldImpl : IStoredField
	{
		private readonly Transaction _transaction;

		private readonly Db4objects.Db4o.Internal.FieldMetadata _fieldMetadata;

		public StoredFieldImpl(Transaction transaction, Db4objects.Db4o.Internal.FieldMetadata
			 fieldMetadata)
		{
			_transaction = transaction;
			_fieldMetadata = fieldMetadata;
		}

		public virtual void CreateIndex()
		{
			lock (Lock())
			{
				_fieldMetadata.CreateIndex();
			}
		}

		public virtual void DropIndex()
		{
			lock (Lock())
			{
				_fieldMetadata.DropIndex();
			}
		}

		private object Lock()
		{
			return _transaction.Container().Lock();
		}

		public virtual Db4objects.Db4o.Internal.FieldMetadata FieldMetadata()
		{
			return _fieldMetadata;
		}

		public virtual object Get(object onObject)
		{
			return _fieldMetadata.Get(_transaction, onObject);
		}

		public virtual string GetName()
		{
			return _fieldMetadata.GetName();
		}

		public virtual IReflectClass GetStoredType()
		{
			return _fieldMetadata.GetStoredType();
		}

		public virtual bool HasIndex()
		{
			return _fieldMetadata.HasIndex();
		}

		public virtual bool IsArray()
		{
			return _fieldMetadata.IsArray();
		}

		public virtual void Rename(string name)
		{
			lock (Lock())
			{
				_fieldMetadata.Rename(name);
			}
		}

		public virtual void TraverseValues(IVisitor4 visitor)
		{
			_fieldMetadata.TraverseValues(_transaction, visitor);
		}

		public override int GetHashCode()
		{
			return _fieldMetadata.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}
			return _fieldMetadata.Equals(((Db4objects.Db4o.Internal.StoredFieldImpl)obj)._fieldMetadata
				);
		}
	}
}
