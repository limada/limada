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
using Db4oTool.Core;
using Db4oUnit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Tests
{
	class ReflectionServices
	{
		public static Instruction FindInstruction(AssemblyDefinition assembly, string typeName, string testMethodName, OpCode testInstruction)
		{
			MethodDefinition testMethod = FindMethod(assembly, typeName, testMethodName);
			Assert.IsNotNull(testMethod);

			return FindInstruction(testMethod, testInstruction);
		}

		public static Instruction FindInstruction(MethodDefinition method, OpCode opCode)
		{
			Instruction current = method.Body.Instructions[0];

			Instruction instruction = current;
			while (instruction != null && instruction.OpCode != opCode)
			{
				instruction = instruction.Next;
			}

			Assert.IsNotNull(instruction);
			Assert.AreEqual(opCode, instruction.OpCode);
			current = instruction;
			return current;
		}

		public static MethodDefinition FindMethod(AssemblyDefinition assembly, string typeName, string methodName)
		{
			TypeDefinition testType = assembly.MainModule.GetType(typeName);
			return CecilReflector.GetMethod(testType, methodName);
		}
	}
}
