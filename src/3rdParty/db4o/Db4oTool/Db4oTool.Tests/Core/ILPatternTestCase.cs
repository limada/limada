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
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Db4oTool.Tests.Core
{
	public class ILPatternTestCase : ITestCase
	{
		public void TestSequenceBackwardsMatch()
		{
			ILPattern sequence = ILPattern.Sequence(OpCodes.Stsfld, OpCodes.Ldsfld);

			MethodDefinition method = CreateTestMethod(TestSequence1);
			Instruction lastInstruction = LastInstruction(method);
			ILPattern.MatchContext context = sequence.BackwardsMatch(lastInstruction);
			Assert.IsTrue(context.Success);
			Assert.AreSame(method.Body.Instructions[0], context.Instruction);
		}

		public void TestSequenceIsBackwardsMatch()
		{
			ILPattern sequence = ILPattern.Sequence(OpCodes.Stsfld, OpCodes.Ldsfld);

			Instruction lastInstruction = CreateTestMethodAndReturnLastInstruction(TestSequence1);
			Assert.IsTrue(sequence.IsBackwardsMatch(lastInstruction));

			sequence = ILPattern.Sequence(OpCodes.Ldsfld, OpCodes.Stsfld);
			Assert.IsTrue(!sequence.IsBackwardsMatch(lastInstruction));
		}
		
		public void TestComplexSequenceIsBackwardsMatch()
		{
			ILPattern sequence = ILPattern.Sequence(
				ILPattern.Optional(OpCodes.Ret),
				ILPattern.Instruction(OpCodes.Stsfld),
				ILPattern.Alternation(OpCodes.Ldfld, OpCodes.Ldsfld));

			Instruction lastInstruction = CreateTestMethodAndReturnLastInstruction(TestSequence1);
			Assert.IsTrue(sequence.IsBackwardsMatch(lastInstruction));

			lastInstruction = CreateTestMethodAndReturnLastInstruction(TestSequence2);
			Assert.IsTrue(sequence.IsBackwardsMatch(lastInstruction));
		}

		delegate void Emitter(ILProcessor il);

		private static Instruction CreateTestMethodAndReturnLastInstruction(Emitter emitter)
		{
			return LastInstruction(CreateTestMethod(emitter));
		}

		private static Instruction LastInstruction(MethodDefinition method)
		{
			return method.Body.Instructions[method.Body.Instructions.Count - 1];
		}

		static MethodDefinition CreateTestMethod(Emitter emitter)
		{
			TypeReference type = new TypeReference ("", "Test", null);
			MethodDefinition test = new MethodDefinition("Test", MethodAttributes.Public, type);
			emitter(test.Body.GetILProcessor());
			return test;
		}

		private static void TestSequence1(ILProcessor il)
		{
			TypeReference type = new TypeReference("", "Test", null);
			FieldDefinition blank = new FieldDefinition("Test", FieldAttributes.Public, type);
			il.Emit(OpCodes.Nop);
			il.Emit(OpCodes.Ldsfld, blank);
			il.Emit(OpCodes.Stsfld, blank);
			il.Emit(OpCodes.Ret);
		}

		private static void TestSequence2(ILProcessor il)
		{
			TypeReference type = new TypeReference ("", "Test", null);
			FieldDefinition blank = new FieldDefinition("Test", FieldAttributes.Public, type);
			il.Emit(OpCodes.Nop);
			il.Emit(OpCodes.Ldfld, blank);
			il.Emit(OpCodes.Stsfld, blank);
			il.Emit(OpCodes.Ret);
			il.Emit(OpCodes.Nop);
		}
	}
}
