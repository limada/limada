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
using System.Collections.Generic;

namespace Sharpen.Lang
{
#if CF

	class ThreadLocal : Db4objects.Db4o.Foundation.ThreadLocal4
	{
	}

#else

	class ThreadLocal
	{
		[ThreadStatic]
		private static Dictionary<ThreadLocal, object> _locals;

		public object Get()
		{
			object value;
			if (Locals.TryGetValue(this, out value))
				return value;
			return null;
		}

		public void Set(object value)
		{
			if (value == null)
				Locals.Remove(this);
			else
				Locals[this] = value;
		}

		private static Dictionary<ThreadLocal, object> Locals
		{
			get
			{
				Dictionary<ThreadLocal, object> value = _locals;
				if (value == null)
					_locals = value = new Dictionary<ThreadLocal, object>();
				return value;
			}
		}

	}
#endif
}

