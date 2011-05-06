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
using Db4objects.Db4o.TA;
using Db4oUnit;

unsafe public class PointerContainer
{
	public int* foo;
}

public delegate void FooHandler();

public class DelegateContainer
{
	public FooHandler foo;

	public EventHandler<EventArgs> bar;

	public EventHandler baz;
}

public class IDoHaveSerializableFields : DelegateContainer
{
	public int fooBar;
}

public class BaseClassWithSerializableField
{
	public int foo;
}

public class DerivedClass : BaseClassWithSerializableField
{
	public EventHandler<EventArgs> bar;
}

public class TAUnsafeInstrumentationSubject : ITestCase
{
	public void TestDelegateIsNotInstrumented()
	{
		Assert.IsFalse(typeof(IActivatable).IsAssignableFrom(typeof(FooHandler)));
	}

	public void TestClassWithoutPersistentFieldsAreNotInstrumented()
	{
		Assert.IsFalse(typeof(IActivatable).IsAssignableFrom(typeof(DelegateContainer)));
		Assert.IsFalse(typeof(IActivatable).IsAssignableFrom(typeof(PointerContainer)));
	}

	public void TestOneSerializableFieldTriggersInstrumentation()
	{
		Assert.IsTrue(typeof(IActivatable).IsAssignableFrom(typeof(IDoHaveSerializableFields)));
		Assert.IsTrue(typeof(IActivatable).IsAssignableFrom(typeof(DerivedClass)));
	}
}

