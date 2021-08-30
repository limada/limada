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
using System.IO;
using System.Reflection;
using Cecil.FlowAnalysis.Utilities;
using Db4objects.Db4o.Instrumentation.Api;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4objects.Db4o.Instrumentation.Cecil
{
	internal class CecilMethodBuilder : IMethodBuilder
	{
		private readonly MethodDefinition _method;
		private readonly ILProcessor _il;

		public CecilMethodBuilder(MethodDefinition method)
		{
			_method = method;
			_il = method.Body.GetILProcessor();
		}

		public IReferenceProvider References
		{
			get { return CecilReferenceProvider.ForModule(_method.DeclaringType.Module.Assembly.MainModule); }
		}

		public void Ldc(object value)
		{
			Type type = value.GetType();
			TypeCode code = Type.GetTypeCode(type);
			switch (code)
			{
				case TypeCode.SByte:
					_il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
					break;
				case TypeCode.Int32:
					_il.Emit(OpCodes.Ldc_I4, (Int32)value);
					break;
				case TypeCode.Int64:
					_il.Emit(OpCodes.Ldc_I8, (Int64)value);
					break;
				case TypeCode.String:
					_il.Emit(OpCodes.Ldstr, (String)value);
					break;
				default:
					throw new NotImplementedException(code.ToString());
			}
		}

		public void LoadArgument(int index)
		{
			switch (index)
			{
				case 0:
					_il.Emit(OpCodes.Ldarg_0);
					break;
				case 1:
					_il.Emit(OpCodes.Ldarg_1);
					break;
				default:
					// TODO: This is wrong. Emit expects an VariableDefinition for a Ldarg .
                    //       But actually no code passes idexes other than 0 and 1 
                    _il.Emit(OpCodes.Ldarg, index);
					break;
			}
		}

		public void Pop()
		{
			_il.Emit(OpCodes.Pop);
		}

		public void LoadArrayElement(ITypeRef elementType)
		{
			throw new NotImplementedException();
		}

		public void Add(ITypeRef operandType)
		{
			throw new NotImplementedException();
		}

		public void Subtract(ITypeRef operandType)
		{
			throw new NotImplementedException();
		}

		public void Multiply(ITypeRef operandType)
		{
			throw new NotImplementedException();
		}

		public void Divide(ITypeRef operandType)
		{
			throw new NotImplementedException();
		}

		public void Invoke(IMethodRef method, CallingConvention convention)
		{
			_il.Emit(OpCodeForConvention(convention), CecilMethodRef.GetReference(method));
		}

		private static OpCode OpCodeForConvention(CallingConvention convention)
		{
			return convention == CallingConvention.Static
			       	? OpCodes.Call
			       	: OpCodes.Callvirt;
		}

		public void Invoke(MethodInfo method)
		{
			throw new NotImplementedException();
		}

		public void LoadField(IFieldRef fieldRef)
		{
			_il.Emit(OpCodes.Ldfld, GetReference(fieldRef));
		}

		private static FieldReference GetReference(IFieldRef fieldRef)
		{
			return CecilFieldRef.GetReference(fieldRef);
		}

		public void LoadStaticField(IFieldRef fieldRef)
		{
			_il.Emit(OpCodes.Ldsfld, GetReference(fieldRef));
		}

		public void Box(ITypeRef boxedType)
		{
			TypeReference type = CecilTypeRef.GetReference(boxedType);
			if (!type.IsValueType) return;
			_il.Emit(OpCodes.Box, type);
		}

		public void EndMethod()
		{
			_il.Emit(OpCodes.Ret);
		}

		public void Print(TextWriter @out)
		{
			Formatter.WriteMethodBody(@out, _method);
		}
	}
}