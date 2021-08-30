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
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <summary>Reflection Array representation.</summary>
	/// <remarks>
	/// Reflection Array representation
	/// <br/><br/>See documentation for System.Reflection API.
	/// </remarks>
	/// <seealso cref="IReflector">IReflector</seealso>
	public interface IReflectArray
	{
		void Analyze(object obj, ArrayInfo info);

		int[] Dimensions(object arr);

		int Flatten(object a_shaped, int[] a_dimensions, int a_currentDimension, object[]
			 a_flat, int a_flatElement);

		object Get(object onArray, int index);

		IReflectClass GetComponentType(IReflectClass a_class);

		int GetLength(object array);

		bool IsNDimensional(IReflectClass a_class);

		object NewInstance(IReflectClass componentType, ArrayInfo info);

		object NewInstance(IReflectClass componentType, int length);

		object NewInstance(IReflectClass componentType, int[] dimensions);

		void Set(object onArray, int index, object element);

		int Shape(object[] a_flat, int a_flatElement, object a_shaped, int[] a_dimensions
			, int a_currentDimension);
	}
}
