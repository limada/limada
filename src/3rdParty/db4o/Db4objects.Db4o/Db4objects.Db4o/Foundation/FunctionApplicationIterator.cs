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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class FunctionApplicationIterator : MappingIterator
	{
		private readonly IFunction4 _function;

		public FunctionApplicationIterator(IEnumerator iterator, IFunction4 function) : base
			(iterator)
		{
			if (null == function)
			{
				throw new ArgumentNullException();
			}
			_function = function;
		}

		protected override object Map(object current)
		{
			return _function.Apply(current);
		}
	}
}
