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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	internal class ObjectAnalyzer
	{
		private readonly ObjectContainerBase _container;

		private readonly object _obj;

		private Db4objects.Db4o.Internal.ClassMetadata _classMetadata;

		private Db4objects.Db4o.Internal.ObjectReference _ref;

		private bool _notStorable;

		internal ObjectAnalyzer(ObjectContainerBase container, object obj)
		{
			_container = container;
			_obj = obj;
		}

		internal virtual void Analyze(Transaction trans)
		{
			_ref = trans.ReferenceForObject(_obj);
			if (_ref != null)
			{
				_classMetadata = _ref.ClassMetadata();
				return;
			}
			IReflectClass claxx = _container.Reflector().ForObject(_obj);
			if (claxx == null)
			{
				NotStorable(_obj, claxx);
				return;
			}
			if (!DetectClassMetadata(trans, claxx))
			{
				return;
			}
			if (IsValueType(_classMetadata))
			{
				NotStorable(_obj, _classMetadata.ClassReflector());
			}
		}

		private bool DetectClassMetadata(Transaction trans, IReflectClass claxx)
		{
			_classMetadata = _container.GetActiveClassMetadata(claxx);
			if (_classMetadata != null)
			{
				if (!_classMetadata.IsStorable())
				{
					NotStorable(_obj, claxx);
					return false;
				}
				return true;
			}
			_classMetadata = _container.ProduceClassMetadata(claxx);
			if (_classMetadata == null || !_classMetadata.IsStorable())
			{
				NotStorable(_obj, claxx);
				return false;
			}
			// The following may return a reference if the object is held
			// in a static variable somewhere ( often: Enums) that gets
			// stored or associated on initialization of the ClassMetadata.
			_ref = trans.ReferenceForObject(_obj);
			return true;
		}

		private void NotStorable(object obj, IReflectClass claxx)
		{
			_container.NotStorable(claxx, obj);
			_notStorable = true;
		}

		internal virtual bool NotStorable()
		{
			return _notStorable;
		}

		private bool IsValueType(Db4objects.Db4o.Internal.ClassMetadata classMetadata)
		{
			return classMetadata.IsValueType();
		}

		internal virtual Db4objects.Db4o.Internal.ObjectReference ObjectReference()
		{
			return _ref;
		}

		public virtual Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			return _classMetadata;
		}
	}
}
