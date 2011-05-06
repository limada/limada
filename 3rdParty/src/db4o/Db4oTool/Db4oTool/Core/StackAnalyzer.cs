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
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Core
{
	public class StackAnalyzer
	{
		public static StackAnalysisResult IsConsumedBy(Predicate<Instruction> predicate, Instruction start, ModuleDefinition module)
		{
			return new StackAnalyzer(module, start, predicate).Analyze();
		}

		private StackAnalyzer(ModuleDefinition module, Instruction instruction, Predicate<Instruction> predicate)
		{
			_module = module;
			_instruction = instruction;
			_predicate = predicate;
		}

		private StackAnalysisResult Analyze()
		{
			//TODO: Take _instruction stack behavior into account.
			int stackHeight = 1;
			int offset = 0;
			foreach(Instruction current in InstructionsFollowing(_instruction))
			{
				offset++;
				
				int produced, consumed;
				StackChangesFor(current, out consumed, out produced);

				stackHeight -= consumed;
				if (stackHeight <= 0)
				{
					return new StackAnalysisResult(current, offset, _predicate(current), stackHeight);
				}

				stackHeight += produced;
			}

			return new StackAnalysisResult();
		}

		private static IEnumerable<Instruction> InstructionsFollowing(Instruction start)
		{
			Instruction current = start.Next;
			
			while (true)
			{
				if (current == null)
				{
					yield break;	
				}

				yield return current;

				if (current.OpCode.FlowControl == FlowControl.Cond_Branch)
				{
					Instruction branchTarget = (Instruction)current.Operand;
					current = branchTarget;
				}
				else
				{
					current = current.Next;
				}
			}
		}

		private void StackChangesFor(Instruction instruction, out int consumed, out int produced)
		{
			consumed = 0;
			switch (instruction.OpCode.StackBehaviourPop)
			{
				case StackBehaviour.Pop0:
					consumed = 0;
					break;

				case StackBehaviour.Popi:
				case StackBehaviour.Popref:
				case StackBehaviour.Pop1:
					consumed = 1;
					break;
				
				case StackBehaviour.Pop1_pop1:
				case StackBehaviour.Popi_pop1:
				case StackBehaviour.Popi_popi:
				case StackBehaviour.Popi_popi8:
				case StackBehaviour.Popi_popr4:
				case StackBehaviour.Popi_popr8:
				case StackBehaviour.Popref_pop1:
				case StackBehaviour.Popref_popi:
					consumed = 2;
					break;
				
				case StackBehaviour.Popi_popi_popi:
				case StackBehaviour.Popref_popi_popi:
				case StackBehaviour.Popref_popi_popi8:
				case StackBehaviour.Popref_popi_popr4:
				case StackBehaviour.Popref_popi_popr8:
				case StackBehaviour.Popref_popi_popref:
					consumed = 3;
					break;

				case StackBehaviour.Varpop:
					consumed = AnalyzeVariablePop(instruction);
					break;
			}

			if (instruction.OpCode.StackBehaviourPush == StackBehaviour.Push0)
			{
				produced = 0;
			}
			else if (instruction.OpCode.StackBehaviourPush == StackBehaviour.Push1_push1)
			{
				produced = 2;
			}
			else if (instruction.OpCode.StackBehaviourPush == StackBehaviour.Varpush)
			{
				produced = IsNonVoidMethodCall(instruction) ? 1 : 0;
			}
			else
			{
				produced = 1;
			}
		}

		private static int AnalyzeVariablePop(Instruction methodCall)
		{
			MethodReference methodReference = (MethodReference) methodCall.Operand;
			MethodDefinition methodDefinition = methodReference.Resolve();
			return (methodDefinition.IsStatic || methodDefinition.IsConstructor ? 0 : 1) + methodDefinition.Parameters.Count;
		}

		private static bool IsNonVoidMethodCall(Instruction instruction)
		{
			if (instruction.OpCode != OpCodes.Call && instruction.OpCode != OpCodes.Callvirt) return false;

			MethodReference method = (MethodReference) instruction.Operand;
			return method.ReturnType.Resolve().FullName != "System.Void";
		}

		private readonly ModuleDefinition _module;
		private readonly Instruction _instruction;
		private readonly Predicate<Instruction> _predicate;
	}
}
