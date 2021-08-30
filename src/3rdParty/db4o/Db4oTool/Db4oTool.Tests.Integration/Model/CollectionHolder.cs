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

namespace Db4oTool.Tests.Integration.Model
{
	public class CollectionHolder<T>
	{
		private string _name;
		private IList<T> _list;
		private IDictionary<string, T> _dictionary;

		public CollectionHolder()
		{
			// db4o creation constructor
		}

		public CollectionHolder(string name, params T[] items)
		{
			_name = name;
			_list = new List<T>(items);
			_dictionary = NewDictionary(items);
		}

		public IList<T> List
		{
			get { return _list; }
		}

		public IDictionary<string, T> Dictionary
		{
			get { return _dictionary; }
		}

		public override string ToString()
		{
			return _name + ": " + _list + "";
		}

		private static IDictionary<string, T> NewDictionary(T[] items)
		{
			IDictionary<string, T> dictionary = new Dictionary<string, T>();
			foreach (T item in items)
			{
				dictionary[item.ToString()] = item;
			}
			return dictionary;
		}

	}
}
