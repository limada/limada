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
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4oUnit;

public class Item
{
	private string _name;

	public Item(string name)
	{
		_name = name;
	}

	public string Name
	{
		get { return _name; }
	}
}

class ByUpperNameUnoptimizable : Predicate
{
	public bool Match(Item candidate)
	{
		return candidate.Name.ToUpper() == "FOO";
	}
}

class ByName : Predicate
{
	public bool Match(Item candidate)
	{
		return candidate.Name == "bar";
	}
}

public class UnoptimizablePredicateSubject : Db4oTool.Tests.Core.InstrumentedTestCase
{
	override public void SetUp()
	{
		_container.Store(new Item("foo"));
		_container.Store(new Item("bar"));
	}
	
	public void TestByUpperName()
	{
		IObjectSet result = _container.Query(new ByUpperNameUnoptimizable());
		Assert.AreEqual(1, result.Count);
		Assert.AreEqual("foo", (result[0] as Item).Name);
	}
	
	public void TestByName()
	{
		IObjectSet result = _container.Query(new ByName());
		Assert.AreEqual(1, result.Count);
		Assert.AreEqual("bar", (result[0] as Item).Name);
	}
}