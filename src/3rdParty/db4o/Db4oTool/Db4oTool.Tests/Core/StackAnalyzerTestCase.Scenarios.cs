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

namespace Db4oTool.Tests.Core
{
	partial class StackAnalyzerTestCase
	{
		internal class SuccessTestScenarios
		{
			public void CastFollowedByParameterLessMethodCall()
			{
				((List<int>)_list).Sort();
			}

			public void CastFollowedByMethodExpectingDelegate()
			{
				((List<int>)_list).Sort(Comparison);
			}

			public void CastFollowedByMethodWithOneArgCall()
			{
				((List<int>)_list).BinarySearch(10);
			}

			public void CastFollowedByMethodWithMultipleArgCall()
			{
				((List<int>)_list).BinarySearch(10, null);
			}

			public void CastFollowedByChainedMethodCall()
			{
				((List<int>)_list).BinarySearch(GetValue());
			}

			public void CastFollowedByComplexChainedMethodCall()
			{
				((List<int>)_list).BinarySearch(GetValue(2));
			}
			
			public void CastFollowedByDeepChainedMethodCall()
			{
				((List<int>)_list).BinarySearch(GetValue(GetValue(2)));
			}

			public void CastFollowedByStaticChainedMethodCall()
			{
				((List<int>)_list).BinarySearch(StaticGetValue());
			}
			
			public void CastFollowedByMethodCallWithConditionals()
			{
				((List<int>)_list).BinarySearch(_list.Count > 10 ? 42 : -42);
			}

			public void UnnecessaryCast()
			{
				((List<int>)_list).Add(42);
			}

			private int GetValue()
			{
				return _value;
			}

			private int GetValue(int n)
			{
				return _value + n;
			}
			
			private static int StaticGetValue()
			{
				return Environment.ProcessorCount;
			}

			private static int Comparison(int lhf, int rhs)
			{
				return 0;
			}

			private IList<int> _list;
			private int _value;
		}

		internal class FailureTestScenarios
		{
			public void StoringCastResult()
			{
				List<int> target = ((List<int>)_list);
			}

			public void PassingCastResultAsArgument1()
			{
				SomeMethod(((List<int>) _list));
			}

			public void PassingCastResultAsArgument2()
			{
				SomeMethod(2, ((List<int>)_list));
			}

			private static void SomeMethod(int list, List<int> ints)
			{
				throw new NotImplementedException();
			}

			private static void SomeMethod(List<int> list)
			{
				throw new NotImplementedException();
			}

			private IList<int> _list;
		}
		
		internal class StackAnalysisResultScenarios
		{
			public object _list;
			[ExpectedStackAnalysisResult(OpCode = "callvirt", Offset = 2, Match = true, StackHeight = 0)]
			public void CastFollowedByMethodCall()
			{
				((List<int>) _list).Add(10);
			}

			[ExpectedStackAnalysisResult(OpCode = "call", Offset = 2, Match = true, StackHeight = 0)]
			public void InstantiationFollowedByMethodCall(int n)
			{
				new List<int>().Add(10);
			}

			[ExpectedStackAnalysisResult(OpCode = "stloc.0", Offset = 1, Match = true, StackHeight = 0)]
			public void InstantiationFollowedByLocalAssignment()
			{
				IList<int> list = new List<int>();
			}

			[ExpectedStackAnalysisResult(OpCode = "call", Offset = 1, Match = true, StackHeight = -1)]
			public void InstantiationUsedAsFirstArgument()
			{
				Internal(new List<int>());
			}

			[ExpectedStackAnalysisResult(OpCode = "call", Offset = 2, Match = true, StackHeight = -1)]
			public void InstantiationUsedAsFirstArgumentOnMultParameterMethod()
			{
				Internal(new List<int>(), -1);
			}

			[ExpectedStackAnalysisResult(OpCode = "call", Offset = 1, Match = true, StackHeight = -2)]
			public void InstantiationUsedAsLastArgument()
			{
				Internal(0, new List<int>());
			}

			[ExpectedStackAnalysisResult(OpCode = "call", Offset = 2, Match = true, StackHeight = -1)]
			public void InstantiationUsedAsFirstArgumentInComplexCallWithStatic()
			{
				Internal(new List<int>(), StaticGetValue());
			}

			[ExpectedStackAnalysisResult(OpCode = "call", Offset = 2, Match = true, StackHeight = -1)]
			public void InstantiationUsedAsFirstArgumentInComplexCallWithNonStatic()
			{
				Internal(new List<int>(), GetValue());
			}

			private void Internal(IList<int> l)
			{
			}
			
			private void Internal(int value, IList<int> l)
			{
			}
			
			private void Internal(IList<int> l, int value)
			{
			}

			private static int StaticGetValue()
			{
				return 42;
			}
			
			private static int GetValue()
			{
				return 42;
			}
		}
	}
}