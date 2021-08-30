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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query;

namespace Db4objects.Db4o.Internal.Query
{
	public class SodaQueryComparator : IComparer, IIntComparator
	{
		public class Ordering
		{
			private SodaQueryComparator.Direction _direction;

			private string[] _fieldPath;

			[System.NonSerialized]
			internal IList _resolvedPath;

			public Ordering(SodaQueryComparator.Direction direction, string[] fieldPath)
			{
				_direction = direction;
				_fieldPath = fieldPath;
			}

			public virtual SodaQueryComparator.Direction Direction()
			{
				return _direction;
			}

			public virtual string[] FieldPath()
			{
				return _fieldPath;
			}
		}

		public class Direction
		{
			public static readonly SodaQueryComparator.Direction Ascending = new SodaQueryComparator.Direction
				(0);

			public static readonly SodaQueryComparator.Direction Descending = new SodaQueryComparator.Direction
				(1);

			private int value;

			private Direction()
			{
			}

			private Direction(int value)
			{
				this.value = value;
			}

			public override bool Equals(object obj)
			{
				return ((SodaQueryComparator.Direction)obj).value == value;
			}

			public override string ToString()
			{
				return this.Equals(Ascending) ? "ASCENDING" : "DESCENDING";
			}
		}

		private readonly LocalObjectContainer _container;

		private readonly LocalTransaction _transaction;

		private readonly ClassMetadata _extentType;

		private readonly SodaQueryComparator.Ordering[] _orderings;

		private readonly IDictionary _bufferCache = new Hashtable();

		private readonly IDictionary _fieldValueCache = new Hashtable();

		public SodaQueryComparator(LocalObjectContainer container, Type extentType, SodaQueryComparator.Ordering
			[] orderings) : this(container, container.ProduceClassMetadata(container.Reflector
			().ForClass(extentType)), orderings)
		{
		}

		public SodaQueryComparator(LocalObjectContainer container, ClassMetadata extent, 
			SodaQueryComparator.Ordering[] orderings)
		{
			_container = container;
			_transaction = ((LocalTransaction)_container.Transaction);
			_extentType = extent;
			_orderings = orderings;
			ResolveFieldPaths(orderings);
		}

		private void ResolveFieldPaths(SodaQueryComparator.Ordering[] orderings)
		{
			for (int fieldPathIndex = 0; fieldPathIndex < orderings.Length; ++fieldPathIndex)
			{
				SodaQueryComparator.Ordering fieldPath = orderings[fieldPathIndex];
				fieldPath._resolvedPath = ResolveFieldPath(fieldPath.FieldPath());
			}
		}

		public virtual IList Sort(long[] ids)
		{
			ArrayList idList = ListFrom(ids);
			idList.Sort(this);
			return idList;
		}

		private ArrayList ListFrom(long[] ids)
		{
			ArrayList idList = new ArrayList(ids.Length);
			for (int idIndex = 0; idIndex < ids.Length; ++idIndex)
			{
				long id = ids[idIndex];
				idList.Add((int)id);
			}
			return idList;
		}

		private IList ResolveFieldPath(string[] fieldPath)
		{
			IList fields = new ArrayList(fieldPath.Length);
			ClassMetadata currentType = _extentType;
			for (int fieldNameIndex = 0; fieldNameIndex < fieldPath.Length; ++fieldNameIndex)
			{
				string fieldName = fieldPath[fieldNameIndex];
				FieldMetadata field = currentType.FieldMetadataForName(fieldName);
				currentType = field.FieldType();
				fields.Add(field);
			}
			return fields;
		}

		public virtual int Compare(object x, object y)
		{
			return Compare(((int)x), ((int)y));
		}

		public virtual int Compare(int x, int y)
		{
			for (int orderingIndex = 0; orderingIndex < _orderings.Length; ++orderingIndex)
			{
				SodaQueryComparator.Ordering ordering = _orderings[orderingIndex];
				int result = CompareByField(x, y, ordering._resolvedPath);
				if (result != 0)
				{
					return ordering.Direction().Equals(SodaQueryComparator.Direction.Ascending) ? result
						 : -result;
				}
			}
			return 0;
		}

		private int CompareByField(int x, int y, IList path)
		{
			object xFieldValue = GetFieldValue(x, path);
			object yFieldValue = GetFieldValue(y, path);
			FieldMetadata field = ((FieldMetadata)path[path.Count - 1]);
			return field.PrepareComparison(_transaction.Context(), xFieldValue).CompareTo(yFieldValue
				);
		}

		private object GetFieldValue(int id, IList path)
		{
			for (int i = 0; i < path.Count - 1; ++i)
			{
				object obj = GetFieldValue(id, ((FieldMetadata)path[i]));
				if (null == obj)
				{
					return null;
				}
				id = _container.GetID(_transaction, obj);
			}
			return GetFieldValue(id, ((FieldMetadata)path[path.Count - 1]));
		}

		internal class FieldValueKey
		{
			private int _id;

			private FieldMetadata _field;

			public FieldValueKey(int id, FieldMetadata field)
			{
				_id = id;
				_field = field;
			}

			public override int GetHashCode()
			{
				return _field.GetHashCode() ^ _id;
			}

			public override bool Equals(object obj)
			{
				SodaQueryComparator.FieldValueKey other = (SodaQueryComparator.FieldValueKey)obj;
				return _field == other._field && _id == other._id;
			}
		}

		private object GetFieldValue(int id, FieldMetadata field)
		{
			SodaQueryComparator.FieldValueKey key = new SodaQueryComparator.FieldValueKey(id, 
				field);
			object cachedValue = _fieldValueCache[key];
			if (null != cachedValue)
			{
				return cachedValue;
			}
			object fieldValue = ReadFieldValue(id, field);
			_fieldValueCache[key] = fieldValue;
			return fieldValue;
		}

		private object ReadFieldValue(int id, FieldMetadata field)
		{
			ByteArrayBuffer buffer = BufferFor(id);
			HandlerVersion handlerVersion = field.ContainingClass().SeekToField(_transaction, 
				buffer, field);
			if (handlerVersion == HandlerVersion.Invalid)
			{
				return null;
			}
			QueryingReadContext context = new QueryingReadContext(_transaction, handlerVersion
				._number, buffer, id);
			return field.Read(context);
		}

		private ByteArrayBuffer BufferFor(int id)
		{
			ByteArrayBuffer cachedBuffer = ((ByteArrayBuffer)_bufferCache[id]);
			if (null != cachedBuffer)
			{
				return cachedBuffer;
			}
			ByteArrayBuffer buffer = _container.ReadBufferById(_transaction, id);
			_bufferCache[id] = buffer;
			return buffer;
		}
	}
}
