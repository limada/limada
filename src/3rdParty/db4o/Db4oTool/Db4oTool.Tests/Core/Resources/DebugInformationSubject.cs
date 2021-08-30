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

public class DebugInformationSubject
{
	public void SimpleSourceLine()
	{
		Test o = new Test();
		o.MethodCall(10);
	}

	public void SimpleIfBody(int n)
	{
		if (n > 10)
		{
			Test o = new Test();
			o.MethodCall(10);
		}
	}

	public void IfAndElseBranch(int n)
	{
		Test o = new Test();
		if (n > 10)
		{
			o.MethodCall(10);
		}
		else
		{
			n = 0;
		}
	}

	public void ElseBranch(int n)
	{
		Test o = new Test();
		if (n > 10)
		{
			n = 0;
		}
		else
		{
			o.MethodCall(10);
		}
	}

	public void AssignmentExpressionAndComparison()
	{
		Test o = new Test();
		int v;
		if ( (v = o.MethodCall(10)) > 1 )
		{
			Console.WriteLine(v);
		}
	}

	public void TryBody()
	{
		try
		{
			Test o = new Test();
			o.MethodCall(10);
		}
		finally
		{
		}
	}

	public void CatchBody()
	{
		try
		{
		}
		catch
		{
			Test o = new Test();
			o.MethodCall(10);
		}
	}
}

public class Test
{
	public int MethodCall(int n)
	{
		return 1;
	}
}
