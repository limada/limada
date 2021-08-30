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
using System.Collections.Generic;
using Db4oTool.Tests.TA; // MockActivator
using Db4oUnit;

class Tagged
{
    public string tags;

    public Tagged(string tags_)
    {
        tags = tags_;
    }
}

struct ValueTypeSubject
{
    public ValueTypeSubject(int i)
    {
        intValue = i;
    }

    public int intValue;
}

class FieldSetterTestSubject
{
    public int intValue;
    public volatile byte volatileByte;
    public Tagged refValue;
    public ValueTypeSubject valueType;
    public List<int> intList;
}

class TAFieldSetterSubject : ITestCase
{
    public void TestFieldSetterActivatesObject()
    {
        FieldSetterTestSubject obj = new FieldSetterTestSubject();
        MockActivator a = ActivatorFor(obj);
        Assert.AreEqual(0, a.ReadCount);
     
        obj.intValue = 10;
        int writeCount = 1;
		Assert.AreEqual(writeCount++, a.WriteCount);

        obj.refValue = null;
		Assert.AreEqual(writeCount++, a.WriteCount);

        obj.volatileByte = 3;
		Assert.AreEqual(writeCount++, a.WriteCount);
		
		Assert.AreEqual(0, a.ReadCount);
        obj.valueType.intValue = 4;
		Assert.AreEqual(1, a.ReadCount);

        obj.valueType = new ValueTypeSubject(5);
		Assert.AreEqual(writeCount++, a.WriteCount);

        obj.intList = new List<int>(6);
		Assert.AreEqual(writeCount++, a.WriteCount);
		Assert.AreEqual(1, a.ReadCount);
    }

    private static MockActivator ActivatorFor(object p)
    {
        return MockActivator.ActivatorFor(p);
    }
}