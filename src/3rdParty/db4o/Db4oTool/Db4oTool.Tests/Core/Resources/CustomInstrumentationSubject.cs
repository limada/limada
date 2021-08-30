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
using System.IO;
using Db4oUnit;
using Db4oTool.Tests.Core;

public class CustomInstrumentationSubject : ITestCase
{
	static void Foo()
	{
		Bar();
	}

	static void Bar()
	{
	}

	public void TestInstrumentation()
	{
		string stdout = ShellUtilities.WithStdout(Foo);

		string expected = @"
TRACE: System.Void CustomInstrumentationSubject::Foo()
TRACE: System.Void CustomInstrumentationSubject::Bar()
";
		Assert.AreEqual(expected.Trim(), stdout);
	}
}