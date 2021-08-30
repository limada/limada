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
using Db4objects.Db4o;
using Db4oTool.Core;
using Db4oTool.Tests.Core;
using Db4oUnit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Tests.TA.Collections
{
	abstract class TACollectionsTestCaseBase : TATestCaseBase, ITestLifeCycle
	{
		protected abstract string TestResource { get; }
		protected abstract Type ReplacementType { get; }	
		protected abstract Type OriginalType { get; }

		protected override Configuration Configuration(string assemblyLocation)
		{
			Configuration config = base.Configuration(assemblyLocation);
			config.PreserveDebugInfo = true;
			return config;
		}

		internal static void AssertInstruction(Instruction actual, OpCode opCode, MemberReference expectedCtor)
		{
			Assert.AreEqual(opCode, actual.OpCode);
			MethodReference actualCtor = (MethodReference)actual.Operand;
			Assert.AreEqual(expectedCtor.DeclaringType.Name, actualCtor.DeclaringType.Name, opCode.ToString());
			Assert.AreEqual(expectedCtor, actualCtor.Resolve(), opCode.ToString());
		}

		protected void AssertWarning(Action<AssemblyDefinition> action, string expectedWarning)
		{
			string output = InstrumentAndRunInIsolatedAppDomain(action);
			Assert.IsTrue(
				output.Contains(expectedWarning),
				string.Format("Expected warning '{0}' not emitted\r\n\r\nActual instrumentation output:\r\n{1}", expectedWarning, output));
		}

		private void InstrumentAssembly(string symbolName)
		{
            CompilationServices.ExtraParameters.Using("/d:" + symbolName, delegate
            {
                InstrumentAssembly(GenerateAssembly(TestResource), true);
            });
		}

		protected void AssertSuccessfulCast(string testMethodName)
		{
			InstrumentAndRunInIsolatedAppDomain(new CastAsserter(ReplacementType, TestResource, testMethodName).AssertIt);
		}

		internal static MethodReference ContructorFor(TypeReference type, params Type[] parameterTypes)
		{
			TypeDefinition definition = type.Resolve();
			return CecilReflector.GetMethod(definition, ".ctor", parameterTypes);
		}

		private string InstrumentAndRunInIsolatedAppDomain(Action<AssemblyDefinition> action)
		{
			AssemblyDefinition assembly = GenerateAssembly(TestResource);
			string instrumentationOutput = InstrumentAssembly(assembly, true);

			AppDomain testDomain = AppDomain.CreateDomain("TACollectionsDomain", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

			try
			{
				testDomain.DoCallBack(new IsolatedAppDomainTestRunner(assembly.MainModule.FullyQualifiedName, action).Run);
			}
			finally
			{
				if (testDomain != null)
				{
					AppDomain.Unload(testDomain);
				}
			}

			return instrumentationOutput;
		}

		public void SetUp()
		{
			ShellUtilities.CopyToTemp(typeof(IObjectContainer).Module.Assembly.Location);
		}

		public void TearDown()
		{
		}
		
		public Instruction FindInstruction(AssemblyDefinition assembly, string testMethodName, OpCode testInstruction)
		{
			return ReflectionServices.FindInstruction(assembly, TestResource, testMethodName, testInstruction);
		}
		
		internal static TypeReference Import(AssemblyDefinition assembly, Type type)
		{
			return assembly.MainModule.Import(type);
		}
		
		protected void AssertConstructorInstrumentation(string methodName, params Type[] argumentTypes)
		{
			InstrumentAndRunInIsolatedAppDomain(new ConstructorInstrumentationAsserter(TestResource, methodName, ReplacementType, argumentTypes).AssertIt);
		}
		
		protected void AssertConstructorInstrumentationWarning(string methodName)
		{
			InstrumentAndRunInIsolatedAppDomain(new ConstructorInstrumentationAsserter(TestResource, methodName, OriginalType).AssertIt);
		}

		protected void AssertFailingCast(string testMethodName)
		{
			try
			{
				InstrumentAssembly(testMethodName.ToUpperInvariant());
				Assert.Fail("An exception should be thrown in the call above");
			}
			catch (InvalidOperationException e)
			{
				string expected = ExpectedFailingCastMessage();
				Assert.IsTrue(e.Message.Contains(expected), string.Format("Expected: {0}, Actual: {1}", expected, e.Message));
			}
		}

		private string ExpectedFailingCastMessage()
		{
			return string.Format("Casts to {0} are only allowed for property access/method calls", OriginalType.ToString().Replace("[", "<").Replace("]", ">"));
		}
	}

	[Serializable]
	internal class ConstructorInstrumentationAsserter
	{
		public ConstructorInstrumentationAsserter(string testTypeName, string methodName, Type type, params Type[] parameterTypes)
		{
			_testTypeName = testTypeName;
			_methodName = methodName;
			_type = type;
			_parameterTypes = parameterTypes;
		}

		public void AssertIt(AssemblyDefinition assembly)
		{
			Instruction current = ReflectionServices.FindInstruction(assembly, _testTypeName, _methodName, OpCodes.Newobj);
			MethodReference foundCtor = TACollectionsTestCaseBase.ContructorFor(TACollectionsTestCaseBase.Import(assembly, _type), _parameterTypes);
			Assert.IsNotNull(foundCtor);
			TACollectionsTestCaseBase.AssertInstruction(current, OpCodes.Newobj, foundCtor);
		}

		private readonly string _testTypeName;
		private readonly string _methodName;
		private readonly Type _type;
		private readonly Type[] _parameterTypes;
	}

	[Serializable]
	internal class CastAsserter
	{
		public CastAsserter(Type replacementType, string testTypeName, string testMethodName)
		{
			_replacementType = replacementType;
			_testTypeName = testTypeName;
			_testMethodName = testMethodName;
		}

		public void AssertIt(AssemblyDefinition assembly)
		{
			Instruction current = ReflectionServices.FindInstruction(assembly, _testTypeName, _testMethodName, OpCodes.Castclass);

			TypeReference castTarget = ((TypeReference)current.Operand).Resolve();
			Assert.AreEqual(assembly.MainModule.Import(_replacementType).Resolve(), castTarget);
		}

		private readonly Type _replacementType;
		private readonly string _testTypeName;
		private readonly string _testMethodName;
	}

	[Serializable]
	class IsolatedAppDomainTestRunner
	{
		public IsolatedAppDomainTestRunner(string assemblyPath, Action<AssemblyDefinition> test)
		{
			_assemblyPath = assemblyPath;
			_test = test;
		}

		public void Run()
		{
			AssemblyDefinition instrumentedAssembly = AssemblyDefinition.ReadAssembly(_assemblyPath);
			_test(instrumentedAssembly);
		}

		private readonly string _assemblyPath;
		private readonly Action<AssemblyDefinition> _test;
	}
}
