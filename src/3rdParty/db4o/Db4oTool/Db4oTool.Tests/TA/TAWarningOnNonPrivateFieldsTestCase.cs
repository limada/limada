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
using Mono.Cecil;

namespace Db4oTool.Tests.TA
{
	class TAWarningOnNonPrivateFieldsTestCase : TAOutputListenerTestCaseBase
	{
		public void TestWarningOnNonPrivateFields()
		{
			AssemblyDefinition assembly = GenerateAssembly("TAClassWithNonPrivateFieldsSubject");
			InstrumentAndAssert(
				AssemblyPath(assembly), 
				true,
				ResultFor("TAClassWithPublicFieldSubject", "value"),
				ResultFor("TAClassWithProtectedFieldSubject", "value"));
		}

		public void TestNoWarningsForNonInstrumentedClasses()
		{
			AssemblyDefinition assembly = GenerateAssembly("TANoFalsePositiveWarningsForNonPrivateFields");
			InstrumentAndAssert(
							AssemblyPath(assembly),
							"-v -ta -by-name:TAFilteredOut -not",
							true,
							ResultFor("TAMixOfPersistentAndNoPersistentFields", "_persistentInt"));
		}

		private static string AssemblyPath(AssemblyDefinition assembly)
		{
			return assembly.MainModule.FullyQualifiedName;
		}

		private static string ResultFor(string typeName, string fieldName)
		{
			return string.Format("Found non-private field '{0}' in instrumented type '{1}'", fieldName, typeName);
		}
	}
}
