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
using System.Reflection;
using Db4oTool.Tests.Core;

namespace Db4oTool.Tests.TA
{
	class TANonStorableTypeTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "TAUnsafeInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get { return "-ta -vv"; }
		}

		protected override string EmitAssemblyFromResource(string resource, Assembly[] references)
		{
			string returnValue = null;
			CompilationServices.Unsafe.Using(true, delegate
           	{
           		returnValue = BaseEmitAssemblyFromResource(resource, references);
           	});
			return returnValue;
		}

		protected override bool ShouldVerify(string resource)
		{
			return false;
		}

		private string BaseEmitAssemblyFromResource(string resource, Assembly[] references)
		{
			return base.EmitAssemblyFromResource(resource, references);
		}
	}
}
