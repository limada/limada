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
using Db4oUnit;
using Mono.Cecil;

namespace Db4oTool.Tests.TA
{
	internal class TAEnsureDb4oReferenceIsAdded : TATestCaseBase
	{
		public void Test()
		{
			AssemblyDefinition assembly = GenerateAssembly("Db4oReferenceSubject");

			Assert.IsFalse(FindDb4oReference(assembly), "Db4o is already referenced");

			InstrumentAssembly(assembly);

			Assert.IsTrue(FindDb4oReference(assembly), "Db4o must have been added.");
		}

		private bool FindDb4oReference(AssemblyDefinition assembly)
		{
			foreach (ModuleDefinition module in assembly.Modules)
			{
				foreach (AssemblyNameReference reference in module.AssemblyReferences)
				{
					if (reference.Name == "Db4objects.Db4o") return true;
				}
			}

			return false;
		}
	}
}
