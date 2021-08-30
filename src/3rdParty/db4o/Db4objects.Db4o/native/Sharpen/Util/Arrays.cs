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

namespace Sharpen.Util
{
	public class Arrays
	{
		public static void Fill<T>(T[] array, T value)
		{	
			for (int i=0; i<array.Length; ++i)
			{
				array[i] = value;
			}
		}
        
        public static void Fill<T>(T[] array, int fromIndex, int toIndex, T value)
        {
            for (int i = fromIndex; i < toIndex; ++i)
            {
                array[i] = value;
            }
        }

		public static bool Equals<T>(T[] x, T[] y)
		{
			if (x == null) return y == null;
			if (y == null) return false;
			if (x.Length != y.Length) return false;
			for (int i = 0; i < x.Length; ++i)
			{
				if (!object.Equals(x[i], y[i])) return false;
			}
			return true;
		}

		public static List<T> AsList<T>(T[] array)
        {
            return new List<T>(array); 
        }
    }
}
