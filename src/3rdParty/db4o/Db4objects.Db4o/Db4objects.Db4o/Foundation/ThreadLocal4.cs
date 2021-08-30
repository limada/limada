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
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>
	/// ThreadLocal implementation for less capable platforms such as JRE 1.1 and
	/// Silverlight.
	/// </summary>
	/// <remarks>
	/// ThreadLocal implementation for less capable platforms such as JRE 1.1 and
	/// Silverlight.
	/// This class is not intended to be used directly, use
	/// <see cref="DynamicVariable">DynamicVariable</see>
	/// .
	/// WARNING: This implementation might leak Thread references unless
	/// <see cref="Set(object)">Set(object)</see>
	/// is called with null on the right thread to clean it up. This
	/// behavior is currently guaranteed by
	/// <see cref="DynamicVariable">DynamicVariable</see>
	/// .
	/// </remarks>
	public class ThreadLocal4
	{
		private readonly IDictionary _values = new Hashtable();

		public virtual void Set(object value)
		{
			lock (this)
			{
				if (value == null)
				{
					Sharpen.Collections.Remove(_values, Thread.CurrentThread());
				}
				else
				{
					_values[Thread.CurrentThread()] = value;
				}
			}
		}

		public virtual object Get()
		{
			lock (this)
			{
				return _values[Thread.CurrentThread()];
			}
		}

		protected object InitialValue()
		{
			return null;
		}
	}
}
