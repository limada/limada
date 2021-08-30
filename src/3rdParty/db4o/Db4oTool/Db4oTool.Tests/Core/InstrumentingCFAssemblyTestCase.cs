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
using System.IO;
using System.Reflection;
using Db4objects.Db4o.TA;
using Db4oTool.Core;
using Db4oUnit;
using Mono.Cecil;

namespace Db4oTool.Tests.Core
{
	public class InstrumentingCFAssemblyTestCase : ITestCase
	{
		private const string CF20_VERSION = "2.0.0.0";
		private const string CF35_VERSION = "3.5.0.0";

		public void TestCF20Instrumentation()
		{
			AssertCFInstrumentation(CF20_VERSION);
		}

		public void TestCF35Instrumentation()
		{
			AssertCFInstrumentation(CF35_VERSION);
		}

		private void AssertCFInstrumentation(string version)
		{
			string assemblyPath = CompileAndInstrumentCFAssembly(version);
			AssertInstrumentedCFAssembly(assemblyPath);
		}

		private static void AssertInstrumentedCFAssembly(string assemblyPath)
		{
			AppDomain appDomain = AppDomain.CreateDomain("CF_InstrumentationTest");

			try
			{
				appDomain.DoCallBack(new InstrumentationAsserter(assemblyPath).Run);
			}
			finally
			{
				AppDomain.Unload(appDomain);
			}
			
		}

		private string CompileAndInstrumentCFAssembly(string version)
		{
			string assemblyPath = String.Empty;
			CompilationServices.ExtraParameters.Using(
				"/nostdlib+",

				delegate
					{
						assemblyPath =
							CompilationServices.EmitAssemblyFromResource(
								ResourceServices.CompleteResourceName(GetType(), "CFInstrumentationSubject"),
								ReferencesFor(version));

						Db4oTool.Program.Run(ProgramOptionsFor(assemblyPath));
					});

			return assemblyPath;
		}

		private static IEnumerable<string> CFVersions()
		{
			yield return CF20_VERSION;
			yield return CF35_VERSION;
		}

		private static ProgramOptions ProgramOptionsFor(string assemblyPath)
		{
			ProgramOptions po = new ProgramOptions();
			po.ProcessArgs(new string[] {"-ta", assemblyPath,});

			return po;
		}

		private Assembly[] ReferencesFor(string version)
		{
			string cfAssembliesFolder = CompactFrameworkServices.FolderFor(version);
			
			return new Assembly[]
			       	{
						Assembly.LoadFile(Path.Combine(cfAssembliesFolder, "mscorlib.dll")),
						Assembly.LoadFile(Path.Combine(cfAssembliesFolder, "System.dll")),
					};
		}
	}

	[Serializable]
	internal class InstrumentationAsserter
	{
		private readonly string _assemblyPath;

		public InstrumentationAsserter(string assemblyPath)
		{
			_assemblyPath = assemblyPath;
		}

		public void Run()
		{
			Assembly assembly = Assembly.LoadFrom(_assemblyPath);
			Assert.IsTrue(typeof(IActivatable).IsAssignableFrom(assembly.GetType("Db4oTool.Tests.Core.Resources.CFInstrumentationSubject")));
		}
	}
}
