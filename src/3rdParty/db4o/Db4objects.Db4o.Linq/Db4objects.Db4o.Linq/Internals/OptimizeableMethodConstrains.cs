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

namespace Db4objects.Db4o.Linq.Internals
{
	internal class OptimizeableMethodConstrains
	{
		public static bool CanBeOptimized(MethodInfo method)
		{
			return IsIListOrICollectionOfTMethod(method) || IsStringMethod(method);
		}

		public static bool IsStringMethod(MethodInfo method)
		{
			return method.DeclaringType == typeof(string);
		}

		public static bool IsIListOrICollectionOfTMethod(MethodInfo method)
		{
			Type declaringType = method.DeclaringType;
			return declaringType.IsGenericInstanceOf(typeof(ICollection<>)) 
			       || typeof(IList).IsAssignableFrom(declaringType) 
				   || declaringType == typeof(Enumerable);
		}
	}
}
