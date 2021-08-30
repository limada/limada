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
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>Strategy for file/byte array growth by a constant factor</summary>
	public class ConstantGrowthStrategy : IGrowthStrategy
	{
		private readonly int _growth;

		/// <param name="growth">The constant growth size</param>
		public ConstantGrowthStrategy(int growth)
		{
			_growth = growth;
		}

		/// <summary>
		/// returns the incremented size after the growth
		/// strategy has been applied
		/// </summary>
		/// <param name="curSize">the original size</param>
		/// <returns>the new size</returns>
		public virtual long NewSize(long curSize, long requiredSize)
		{
			long newSize = curSize;
			while (newSize < requiredSize)
			{
				newSize += _growth;
			}
			return newSize;
		}
	}
}
