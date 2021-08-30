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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Algorithms4
	{
		private const int QsortLengthThreshold = 7;

		public static void Sort(ISortable4 sortable)
		{
			Sort(sortable, 0, sortable.Size());
		}

		public static void Sort(ISortable4 sortable, int start, int end)
		{
			int length = end - start;
			if (length < QsortLengthThreshold)
			{
				InsertionSort(sortable, start, end);
				return;
			}
			Qsort(sortable, start, end);
		}

		public static void Qsort(ISortable4 sortable, int start, int end)
		{
			int length = end - start;
			int middle = start + length / 2;
			if (length > 7)
			{
				int bottom = start;
				int top = end - 1;
				if (length > 40)
				{
					length /= 8;
					bottom = MiddleValueIndex(sortable, bottom, bottom + length, bottom + (2 * length
						));
					middle = MiddleValueIndex(sortable, middle - length, middle, middle + length);
					top = MiddleValueIndex(sortable, top - (2 * length), top - length, top);
				}
				middle = MiddleValueIndex(sortable, bottom, middle, top);
			}
			int a;
			int b;
			int c;
			int d;
			a = b = start;
			c = d = end - 1;
			while (true)
			{
				while (b <= c && sortable.Compare(b, middle) <= 0)
				{
					if (sortable.Compare(b, middle) == 0)
					{
						middle = NewPartionIndex(middle, a, b);
						Swap(sortable, a++, b);
					}
					b++;
				}
				while (c >= b && sortable.Compare(c, middle) >= 0)
				{
					if (sortable.Compare(c, middle) == 0)
					{
						middle = NewPartionIndex(middle, c, d);
						Swap(sortable, c, d--);
					}
					c--;
				}
				if (b > c)
				{
					break;
				}
				middle = NewPartionIndex(middle, b, c);
				Swap(sortable, b++, c--);
			}
			length = Math.Min(a - start, b - a);
			Swap(sortable, start, b - length, length);
			length = Math.Min(d - c, end - 1 - d);
			Swap(sortable, b, end - length, length);
			length = b - a;
			if (length > 0)
			{
				Sort(sortable, start, start + length);
			}
			length = d - c;
			if (length > 0)
			{
				Sort(sortable, end - length, end);
			}
		}

		public static void InsertionSort(ISortable4 sortable, int start, int end)
		{
			for (int i = start + 1; i < end; i++)
			{
				for (int j = i; j > start && sortable.Compare(j - 1, j) > 0; j--)
				{
					Swap(sortable, j - 1, j);
				}
			}
		}

		private static int NewPartionIndex(int oldPartionIndex, int leftSwapIndex, int rightSwapIndex
			)
		{
			if (leftSwapIndex == oldPartionIndex)
			{
				return rightSwapIndex;
			}
			else
			{
				if (rightSwapIndex == oldPartionIndex)
				{
					return leftSwapIndex;
				}
			}
			return oldPartionIndex;
		}

		private static int MiddleValueIndex(ISortable4 sortable, int a, int b, int c)
		{
			if (sortable.Compare(a, b) < 0)
			{
				if (sortable.Compare(b, c) < 0)
				{
					return b;
				}
				else
				{
					if (sortable.Compare(a, c) < 0)
					{
						return c;
					}
					else
					{
						return a;
					}
				}
			}
			else
			{
				if (sortable.Compare(b, c) > 0)
				{
					return b;
				}
				else
				{
					if (sortable.Compare(a, c) > 0)
					{
						return c;
					}
					else
					{
						return a;
					}
				}
			}
		}

		private static void Swap(ISortable4 sortable, int left, int right)
		{
			if (left == right)
			{
				return;
			}
			sortable.Swap(left, right);
		}

		private static void Swap(ISortable4 sortable, int from, int to, int length)
		{
			while (length-- > 0)
			{
				Swap(sortable, from++, to++);
			}
		}
	}
}
