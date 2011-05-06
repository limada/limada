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
using Mono.Cecil;

namespace Db4oTool.Tests.Core
{
	public class AcceptNoneFilter : ITypeFilter
	{
		public bool Accept(TypeDefinition typeDef)
		{
			return false;
		}
	}

    class ByFilterTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "ByFilterInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get
			{
				return "-by-filter:Db4oTool.Tests.Core.AcceptNoneFilter,Db4oTool.Tests"
					+ " -instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests";
			}
		}
	}
}
