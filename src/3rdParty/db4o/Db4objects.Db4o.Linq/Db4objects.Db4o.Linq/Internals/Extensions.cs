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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Db4objects.Db4o.Linq.Internals
{
	internal static class Extensions
	{
		public static Type[] GetParameterTypes(this MethodBase self)
		{
			return self.GetParameters().Select(p => p.ParameterType).ToArray();
		}

		public static Type MakeGenericTypeFrom(this Type self, Type type)
		{
			return self.MakeGenericType(type.GetGenericArguments());
		}

		public static Type GetFirstGenericArgument(this Type self)
		{
			return self.GetGenericArguments()[0];
		}

		public static bool IsGenericInstanceOf(this Type self, Type type)
		{
			return self.IsGenericType && self.GetGenericTypeDefinition() == type;
		}

		public static MethodInfo MakeGenericMethodFrom(this MethodInfo self, MethodInfo method)
		{
			return self.MakeGenericMethod(method.GetGenericArguments());
		}

		public static bool IsExtension(this MethodInfo self)
		{
			return self.GetCustomAttributes(typeof(ExtensionAttribute), false).Length > 0;
		}
	}
}
