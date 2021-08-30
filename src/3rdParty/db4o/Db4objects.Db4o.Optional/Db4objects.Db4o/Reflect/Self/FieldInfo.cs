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
namespace Db4objects.Db4o.Reflect.Self
{
	public class FieldInfo
	{
		private string _name;

		private System.Type _clazz;

		private bool _isPublic;

		private bool _isStatic;

		private bool _isTransient;

		public FieldInfo(string name, System.Type clazz, bool isPublic, bool isStatic, bool
			 isTransient)
		{
			_name = name;
			_clazz = clazz;
			_isPublic = isPublic;
			_isStatic = isStatic;
			_isTransient = isTransient;
		}

		public virtual string Name()
		{
			return _name;
		}

		public virtual System.Type Type()
		{
			return _clazz;
		}

		public virtual bool IsPublic()
		{
			return _isPublic;
		}

		public virtual bool IsStatic()
		{
			return _isStatic;
		}

		public virtual bool IsTransient()
		{
			return _isTransient;
		}
	}
}
