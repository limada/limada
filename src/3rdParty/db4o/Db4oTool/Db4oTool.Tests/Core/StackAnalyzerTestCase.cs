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
using System.Reflection;
using System.Text;
using Db4oTool.Core;
using Db4oUnit;
using Mono.Cecil;

namespace Db4oTool.Tests.Core
{
	partial class StackAnalyzerTestCase : ITestCase
	{
		public void TestSuccessFullScenarios()
		{
			AssertStackIsConsumed(typeof(SuccessTestScenarios));
		}

		public void TestFailureScenarios()
		{
			AssertStackIsNotConsumed(typeof(FailureTestScenarios));
		}
		
		public void TestStackAnalysisResult()
		{
			AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(GetType().Module.Assembly.Location);

			Type testsDeclaringType = typeof(StackAnalysisResultScenarios);			

			StringBuilder sb = new StringBuilder();
			foreach (MethodInfo testMethod in testsDeclaringType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
			{
				try
				{
					MethodDefinition methodDefinition = MethodReferenceFor(testMethod, assembly).Resolve();
					ExpectedStackAnalysisResultAttribute expected = ExpectedStackAnalysisResultFor(testMethod);
					StackAnalysisResult actual = StackAnalyzer.IsConsumedBy(IsMethodCallOnList, FindCastOrNewObj(methodDefinition), methodDefinition.DeclaringType.Module);

					Assert.AreEqual(expected.OpCode, actual.Consumer.OpCode.ToString());
					Assert.AreEqual(expected.Offset, actual.Offset);
					Assert.AreEqual(expected.StackHeight, actual.StackHeight);
					Assert.AreEqual(expected.Match, true);
				}
				catch (Exception ex)
				{
					sb.AppendFormat("Exception while processing method {0}\r\n{1}", testMethod, ex);
				}
			}

			if (sb.Length > 0)
			{
				Assert.Fail(sb.ToString());
			}
		}
	}
}