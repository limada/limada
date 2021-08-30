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
using System.Collections;
using System.Collections.Generic;

namespace Sharpen
{
	public class Collections
	{
        public static void AddAll(System.Collections.IList list, System.Collections.IEnumerable added)
        {
            foreach (object o in added)
            {
                list.Add(o);
            }
        }

        public static bool AddAll<T>(ICollection<T> list, System.Collections.Generic.IEnumerable<T> added)
        {
            foreach (T o in added)
            {
                list.Add(o);
            }
            return true;
        }

		public static object Remove(IDictionary dictionary, object key)
		{
			object removed = dictionary[key];
			dictionary.Remove(key);
			return removed;
		}

	    public static object[] ToArray(ICollection collection)
	    {
	    	object[] result = new object[collection.Count];
			collection.CopyTo(result, 0);
			return result;
	    }

		public static T[] ToArray<T>(ICollection collection, T[] result)
		{
			collection.CopyTo(result, 0);
			return result;
		}

		public static T[] ToArray<T>(ICollection<T> collection, T[] result)
		{
			collection.CopyTo(result, 0);
			return result;
		}
	}
}
