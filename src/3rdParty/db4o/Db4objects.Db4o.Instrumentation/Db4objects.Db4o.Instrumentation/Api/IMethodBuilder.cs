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
using System.IO;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Instrumentation.Api
{
	/// <summary>Cross platform interface for bytecode emission.</summary>
	/// <remarks>Cross platform interface for bytecode emission.</remarks>
	public interface IMethodBuilder
	{
		IReferenceProvider References
		{
			get;
		}

		void Ldc(object value);

		void LoadArgument(int index);

		void Pop();

		void LoadArrayElement(ITypeRef elementType);

		void Add(ITypeRef operandType);

		void Subtract(ITypeRef operandType);

		void Multiply(ITypeRef operandType);

		void Divide(ITypeRef operandType);

		void Invoke(IMethodRef method, CallingConvention convention);

		void Invoke(MethodInfo method);

		void LoadField(IFieldRef fieldRef);

		void LoadStaticField(IFieldRef fieldRef);

		void Box(ITypeRef boxedType);

		void EndMethod();

		void Print(TextWriter @out);
	}
}
