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
using Db4objects.Db4o.Collections;

namespace Db4oTool.Tests.TA.Collections
{
	class TADictionaryTestCase : TACollectionsTestCaseBase
	{
		public void TestConstructorIsReplaced()
		{
			AssertConstructorInstrumentation("ConstructorWithInicialCapacity", new Type[] {typeof(int)});
			
			// FIXME: Represent generic parameters in members
			//AssertConstructorInstrumentation("ConstructorWithDictionary", new Type[] { typeof(IDictionary<,>) });
		}

		public void TestCastIsReplaced()
		{
			AssertSuccessfulCast("CastFollowedByValuePropertyAccess");
		}

		public void TestAssignmentToConcreteType()
		{
			AssertConstructorInstrumentationWarning("InitConcrete");
		}

		#region Overrides of TACollectionsTestCaseBase

		protected override string TestResource
		{
			get { return "TAActivatableDictionaryScenarios"; }
		}

		protected override Type ReplacementType
		{
			get { return typeof(ActivatableDictionary<string, int>); }
		}

		protected override Type OriginalType
		{
			get { return typeof(Dictionary<string, int>); }
		}

		#endregion
	}
}
