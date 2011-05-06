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
using System.Collections.Generic;
using Db4oTool.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.NQ
{
	public class CFNQEnabler : AbstractAssemblyInstrumentation
	{
		private QueryInvocationProcessor _processor;

		override protected void ProcessMethod(MethodDefinition method)
		{
			if (null == method.Body) return;

			// TraceMethodBody(method);

			List<Instruction> instructions = CollectQueryInvocations(method);
			foreach (Instruction instruction in instructions)
			{
				ProcessQueryInvocation(method, instruction);
			}

			//TraceMethodBody(method);
		}

		void ProcessQueryInvocation(MethodDefinition parent, Instruction queryInvocation)
		{
			if (null == _processor) _processor = new QueryInvocationProcessor(_context);
			_processor.Process(parent, queryInvocation);
		}

		private List<Instruction> CollectQueryInvocations(MethodDefinition method)
		{
			return new List<Instruction>(EnumerateQueryInvocations(method));
		}

		private IEnumerable<Instruction> EnumerateQueryInvocations(MethodDefinition method)
		{
			foreach (Instruction instruction in method.Body.Instructions)
			{
				if (IsObjectContainerQueryOnPredicateInvocation(instruction))
				{
					yield return instruction;
				}
			}
		}

		private bool IsObjectContainerQueryOnPredicateInvocation(Instruction instruction)
		{
			if (instruction.OpCode.Value != OpCodes.Callvirt.Value) return false;
			GenericInstanceMethod methodRef = instruction.Operand as GenericInstanceMethod;
			if (null == methodRef) return false;
			if (1 == string.Compare("query", methodRef.Name, true)) return false;
			if (1 != methodRef.Parameters.Count) return false;
			return IsSystemPredicateInstance(methodRef.Parameters[0].ParameterType);
		}

		private bool IsSystemPredicateInstance(TypeReference type)
		{
			GenericInstanceType genericType = type as GenericInstanceType;
			if (null == genericType) return false;
			return genericType.FullName.StartsWith("System.Predicate");
		}
	}
}
