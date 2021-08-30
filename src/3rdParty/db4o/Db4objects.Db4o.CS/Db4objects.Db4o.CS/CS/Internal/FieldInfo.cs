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
using Db4objects.Db4o.CS.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public class FieldInfo
	{
		public string _fieldName;

		public ClassInfo _fieldClass;

		public bool _isPrimitive;

		public bool _isArray;

		public bool _isNArray;

		public FieldInfo()
		{
		}

		public FieldInfo(string fieldName, ClassInfo fieldClass, bool isPrimitive, bool isArray
			, bool isNArray)
		{
			_fieldName = fieldName;
			_fieldClass = fieldClass;
			_isPrimitive = isPrimitive;
			_isArray = isArray;
			_isNArray = isNArray;
		}

		public virtual ClassInfo GetFieldClass()
		{
			return _fieldClass;
		}

		public virtual string GetFieldName()
		{
			return _fieldName;
		}
	}
}
