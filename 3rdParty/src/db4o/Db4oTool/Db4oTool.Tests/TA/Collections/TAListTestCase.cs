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
using Db4oTool.Core;

namespace Db4oTool.Tests.TA.Collections
{
	class TAListTestCase : TACollectionsTestCaseBase
	{
		public void TestMethodWithInterfaceParameter()
		{
			AssertConstructorInstrumentation("InitInterface");
			AssertConstructorInstrumentation("CollectionInitInterface");
		}

		public void TestLocalsAsInterface()
		{
			AssertConstructorInstrumentation("LocalsAsIList");
			AssertConstructorInstrumentation("CollectionLocalsAsIList");
		}

		public void TestMethodReturningNewListAsInterface()
		{
			AssertConstructorInstrumentation("CreateList");
			AssertConstructorInstrumentation("CollectionCreateList");
		}

		public void TestAssignmentOfConstructorLessListToInterface()
		{
			AssertConstructorInstrumentation("ParameterLessConstructor");
			AssertConstructorInstrumentation("CollectionParameterLessConstructor");
		}

		public void TestConstructorsWarnings()
		{
			AssertConstructorInstrumentationWarning("InitConcrete");
			AssertConstructorInstrumentationWarning("AssignmentOfConcreteListToLocal");
			AssertConstructorInstrumentationWarning("AssignmentOfConcreteListToField");
			AssertConstructorInstrumentationWarning("PublicCreateConcreteList");
		}

		public void TestSuccessfulCasts()
		{
			AssertSuccessfulCast("CastFollowedByParameterLessMethod");
			AssertSuccessfulCast("CastFollowedByMethodWithSingleArgument");
			AssertSuccessfulCast("CastConsumedByPropertyAccess");
		}

		public void TestFailingCasts()
		{
			AssertFailingCast("CastConsumedByLocal");
			AssertFailingCast("CastConsumedByField");
			AssertFailingCast("CastConsumedByArgument");
			AssertFailingCast("CastConsumedByMethodReturn");
		}

		protected override Configuration Configuration(string assemblyLocation)
        {
            Configuration conf = base.Configuration(assemblyLocation);
            conf.PreserveDebugInfo = true;
            return conf;
        }

		#region Overrides of TACollectionsTestCaseBase

		protected override string TestResource
		{
			get { return "TACollectionsScenarios"; }
		}

		protected override Type ReplacementType
		{
			get { return typeof(ActivatableList<string>); }
		}

		protected override Type OriginalType
		{
			get { return typeof(List<string>); }
		}

		#endregion
	}
}
