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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect.Core
{
	/// <exclude></exclude>
	public class ReflectorUtils
	{
		public static IReflectClass ReflectClassFor(IReflector reflector, object clazz)
		{
			if (clazz is IReflectClass)
			{
				return (IReflectClass)clazz;
			}
			if (clazz is Type)
			{
				return reflector.ForClass((Type)clazz);
			}
			if (clazz is string)
			{
				return reflector.ForName((string)clazz);
			}
			return reflector.ForObject(clazz);
		}

		public static IReflectField Field(IReflectClass claxx, string name)
		{
			while (claxx != null)
			{
				try
				{
					return claxx.GetDeclaredField(name);
				}
				catch (Exception)
				{
				}
				claxx = claxx.GetSuperclass();
			}
			return null;
		}

		public static void ForEachField(IReflectClass claxx, IProcedure4 procedure)
		{
			while (claxx != null)
			{
				IReflectField[] declaredFields = claxx.GetDeclaredFields();
				for (int reflectFieldIndex = 0; reflectFieldIndex < declaredFields.Length; ++reflectFieldIndex)
				{
					IReflectField reflectField = declaredFields[reflectFieldIndex];
					procedure.Apply(reflectField);
				}
				claxx = claxx.GetSuperclass();
			}
		}
	}
}
