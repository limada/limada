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
using System.Collections;
using System.Collections.Generic;
using Db4oUnit;
using System.Globalization;
using System.Threading;

class Foo : IEnumerable<Foo>
{
	Foo _child;

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<Foo>)this).GetEnumerator();
	}

	public IEnumerator<Foo> GetEnumerator()
	{
		foreach (Foo a in _child)
		{
			yield return a;
			foreach (Foo b in a)
			{
				yield return b;
			}
		}
	}

	public IEnumerable Bar(bool raise)
	{
		string prefix = "child is: ";
		if (raise) throw new ApplicationException();
		foreach (Foo child in _child.Bar(false))
			yield return prefix + child;
	}
}

public class PreserveDebugInfoSubject : ITestCase
{
	public void Test()
	{
        CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
        try
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            try
            {
                foreach (object o in new Foo().Bar(true))
				{
				}
            }
            catch (Exception x)
            {
                string message = x.ToString();
                Assert.IsTrue(message.Contains("PreserveDebugInfoSubject.cs:line 32"), message);
            }
        }
        finally
        {
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
        }
	}
}