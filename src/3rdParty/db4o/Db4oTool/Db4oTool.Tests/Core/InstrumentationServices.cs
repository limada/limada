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
using Db4oTool.Core;

namespace Db4oTool.Tests.Core
{
	static class InstrumentationServices
	{
		public static ShellUtilities.ProcessOutput InstrumentAssembly(string options, string path)
		{
			string[] commandLine = BuildCommandLine(options, path);
			return System.Diagnostics.Debugger.IsAttached
			       	? ShellUtilities.shellm(InstrumentationUtilityPath, commandLine)
			       	: ShellUtilities.shell(InstrumentationUtilityPath, commandLine);
		}

		public static string[] BuildCommandLine(string options, string path)
		{
			string[] cmdLine = options.Split(' ');
			return ArrayServices.Append(cmdLine, path);
		}

		public static string InstrumentationUtilityPath
		{
			get { return typeof(InstrumentationPipeline).Module.FullyQualifiedName; }
		}
	}
}
