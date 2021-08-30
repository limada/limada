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
using Db4oTool.Tests.Core;
using Mono.Cecil;

namespace Db4oTool.Tests
{
	class Db4oToolTestServices
	{
		public static AssemblyDefinition AssemblyFromResource(string resourceName, Type simblingType, bool loadSymbols, Action<string> sourceHandler, params Assembly[] references)
		{
			string assemblyPath = CompilationServices.EmitAssemblyFromResource(
										ResourceServices.CompleteResourceName(simblingType, resourceName),
										sourceHandler,
										references);

			ReaderParameters parameters = new ReaderParameters();
			parameters.ReadSymbols = loadSymbols;
			return AssemblyDefinition.ReadAssembly(assemblyPath, parameters);
		}
	}
}
