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
namespace Db4oTool.Tests.Core
{
	class PreserveDebugInfoTestCase : SingleResourceTestCase
	{
		protected override bool AcceptsReleaseMode
		{
			get { return false; }
		}

		protected override string ResourceName
		{
			get { return "PreserveDebugInfoSubject"; }
		}

		protected override string CommandLine
		{
			get
			{
				return "-debug"
					+ " -instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests";
			}
		}

		// FIXME: remove this method after updating cecil
		// (waiting for Module.FullLoad patch to be applied)
		protected override void InstrumentAssembly(string path)
		{
			 //base.InstrumentAssembly(path);
		}
	}
}
