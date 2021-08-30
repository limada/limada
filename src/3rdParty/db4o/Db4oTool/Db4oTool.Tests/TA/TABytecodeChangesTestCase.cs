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
using Cecil.FlowAnalysis.Utilities;
using Db4oTool.Core;
using Db4oTool.Tests.Core;
using Db4oUnit;
using Mono.Cecil;

namespace Db4oTool.Tests.TA
{
	class TABytecodeChangesTestCase : ITestCase
	{
		public void TestThisFieldPattern()
		{
			string assemblyPath = InstrumentResource();
			AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(assemblyPath);
			MethodDefinition method = GetMethodDefinition(assembly, "Subject", "get_Property");
			string expected = @"
System.String Subject::get_Property()
	IL_0000: nop
	IL_0001: ldarg.0
	IL_0002: ldc.i4.0
	IL_0003: callvirt void Db4objects.Db4o.TA.IActivatable::Activate(Db4objects.Db4o.Activation.ActivationPurpose)
	IL_0008: ldarg.0
	IL_0009: ldfld System.String Subject::_field
	IL_000e: stloc.0
	IL_000f: br.s IL_0011
	IL_0011: ldloc.0
	IL_0012: ret";
			AssertIgnoringWhiteSpace(expected, Formatter.FormatMethodBody(method));
		}

		private static void AssertIgnoringWhiteSpace(string expected, string actual)
		{
			Assert.AreEqual(NormalizeWhiteSpace(expected), NormalizeWhiteSpace(actual));
		}

		private static string NormalizeWhiteSpace(string expected)
		{
			return expected.Replace("\r\n", "\n").Trim();
		}

		private static MethodDefinition GetMethodDefinition(AssemblyDefinition assembly, string typeName, string methodName)
		{
			return CecilReflector.GetMethod(assembly.MainModule.GetType(typeName), methodName);
		}

		private string InstrumentResource()
		{
			string resourceName = ResourceServices.CompleteResourceName(GetType(), "TABytecodeChangesSubject");
			string path = CompilationServices.EmitAssemblyFromResource(resourceName);
			ShellUtilities.ProcessOutput output = InstrumentationServices.InstrumentAssembly("-ta", path);
			Assert.AreEqual(0, output.ExitCode);
			return path;
		}
	}
}
