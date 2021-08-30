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
using System.Collections.Generic;

class TAActivatableDictionaryScenarios
{
	public void ConstructorWithInicialCapacity()
	{
		IDictionary<string, int> dict = new Dictionary<string, int>(10);
	}

	public void ConstructorWithDictionary(Dictionary<string, int> source)
	{
		IDictionary<string, int> dict = new Dictionary<string, int>(source);
	}

	public void CastFollowedByValuePropertyAccess()
	{
		IDictionary<int, DateTime> dic = new Dictionary<int, DateTime>();
		object values = ((Dictionary<int, DateTime>)dic).Values;
	}

	public void InitConcrete()
	{
		Dictionary<int, DateTime> dic = new Dictionary<int, DateTime>();
	}
}
