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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Db4oTool.Core;
using Db4oTool.TA;
using Db4oTool.Tests.Core;
using Db4oUnit;
using Mono.Cecil;

namespace Db4oTool.Tests.TA
{
	public abstract class TATestCaseBase : ITestCase
	{
		protected string InstrumentAssembly(AssemblyDefinition testAssembly)
		{
			return InstrumentAssembly(testAssembly, false);
		}

		protected static AssemblyDefinition GenerateAssembly(string resourceName, params Assembly[] references)
		{
			return Db4oToolTestServices.AssemblyFromResource(resourceName, typeof (TATestCaseBase), true, delegate { }, references);
		}

		private string InstrumentAssembly(AssemblyDefinition assembly, IAssemblyInstrumentation instrumentation)
		{
			StringWriter output = new StringWriter();
			Trace.Listeners.Add(new TextWriterTraceListener(output));

			string assemblyFullPath = assembly.MainModule.FullyQualifiedName;
			InstrumentationContext context = new InstrumentationContext(Configuration(assemblyFullPath), assembly);

			instrumentation.Run(context);
			context.SaveAssembly();

			VerifyAssembly(assemblyFullPath);

			return output.ToString();
		}

		protected string InstrumentAssembly(AssemblyDefinition testAssembly, bool instrumentCollections)
		{
			return InstrumentAssembly(testAssembly, new TAInstrumentation(instrumentCollections));
		}

		protected static void VerifyAssembly(string assemblyPath)
		{
			new VerifyAssemblyTest(assemblyPath).Run();
		}

		protected virtual Configuration Configuration(string assemblyLocation)
		{
			Configuration configuration = new Configuration(assemblyLocation);
			configuration.TraceSwitch.Level = TraceLevel.Info;
 
			return configuration;
		}
	}
}
