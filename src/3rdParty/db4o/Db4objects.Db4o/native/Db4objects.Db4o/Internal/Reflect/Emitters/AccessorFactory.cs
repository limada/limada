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
using System.Reflection;

namespace Db4objects.Db4o.Internal.Reflect.Emitters
{
#if !CF
	public delegate void Setter(object o, object value);
	public delegate object Getter(object o);

	public static class AccessorFactory
	{
		private static readonly Dictionary<FieldInfo, Getter> _getters = new Dictionary<FieldInfo, Getter>();
		private static readonly Dictionary<FieldInfo, Setter> _setters = new Dictionary<FieldInfo, Setter>();
		
		public static Setter SetterFor(FieldInfo field)
		{
			return Produce(_setters, field, EmitSetter);
		}

		public static Getter GetterFor(FieldInfo field)
		{
			return Produce(_getters, field, EmitGetter);
		}

		private delegate TEmitter Producer<TEmitter>(FieldInfo field);

		private static TEmitter Produce<TEmitter>(Dictionary<FieldInfo, TEmitter> cache, FieldInfo field, Producer<TEmitter> producer)
		{
			TEmitter emitter;
			lock (cache)
			{
				if (!cache.TryGetValue(field, out emitter))
				{
					emitter = producer(field);
					cache[field] = emitter;
				}
			}
			return emitter;
		}

		private static Setter EmitSetter(FieldInfo field)
		{
			try
			{
				return new SetFieldEmitter(field).Emit();
			}
			catch
			{
				return delegate { };
			}
		}
		
		private static Getter EmitGetter(FieldInfo field)
		{
			try
			{
				return new GetFieldEmitter(field).Emit();
			}
			catch
			{
				return delegate { return null; };
			}
		}
	}
#endif
}
