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
using Db4objects.Db4o.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>A dynamic variable is a value associated to a specific thread and scope.
	/// 	</summary>
	/// <remarks>
	/// A dynamic variable is a value associated to a specific thread and scope.
	/// The value is brought into scope with the
	/// <see cref="With(object, IClosure4)">With(object, IClosure4)</see>
	/// method.
	/// </remarks>
	public class DynamicVariable
	{
		public static DynamicVariable NewInstance()
		{
			return new DynamicVariable();
		}

		private readonly ThreadLocal _value = new ThreadLocal();

		public virtual object Value
		{
			get
			{
				object value = _value.Get();
				return value == null ? DefaultValue() : value;
			}
			set
			{
				_value.Set(value);
			}
		}

		protected virtual object DefaultValue()
		{
			return null;
		}

		public virtual object With(object value, IClosure4 block)
		{
			object previous = _value.Get();
			_value.Set(value);
			try
			{
				return block.Run();
			}
			finally
			{
				_value.Set(previous);
			}
		}

		public virtual void With(object value, IRunnable block)
		{
			object previous = _value.Get();
			_value.Set(value);
			try
			{
				block.Run();
			}
			finally
			{
				_value.Set(previous);
			}
		}
	}
}
