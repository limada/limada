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
using Db4oTool.Core;
using Db4oTool.Tests.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Tests.Core
{
	/// <summary>
	/// Prepends Console.WriteLine("TRACE: " + method) to every method
	/// in the assembly.
	/// </summary>
	public class TraceInstrumentation : AbstractAssemblyInstrumentation
	{
		override protected void ProcessMethod(MethodDefinition method)
		{
			if (!method.HasBody) return;
			
			MethodBody body = method.Body;
			Instruction firstInstruction = body.Instructions[0];
			ILProcessor il = body.GetILProcessor();
			
			// ldstr "TRACE: " + method
			il.InsertBefore(firstInstruction,
			                    il.Create(OpCodes.Ldstr, "TRACE: " + method));
			
			// call Console.WriteLine(string)
			MethodReference Console_WriteLine = Import(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
			il.InsertBefore(firstInstruction,
			                    il.Create(OpCodes.Call, Console_WriteLine));
		}
	}

    class CustomInstrumentationTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "CustomInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get { return "-instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests"; }
		}
	}
}
