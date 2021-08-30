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
using Sharpen.Lang;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>A collection of static methods that should be part of the runtime environment but are not.
	/// 	</summary>
	/// <remarks>A collection of static methods that should be part of the runtime environment but are not.
	/// 	</remarks>
	/// <exclude></exclude>
	public class Runtime4
	{
		/// <summary>sleeps without checked exceptions</summary>
		public static void Sleep(long millis)
		{
			try
			{
				Thread.Sleep(millis);
			}
			catch (Exception)
			{
			}
		}

		/// <summary>sleeps with implicit exception</summary>
		/// <exception cref="Db4objects.Db4o.Foundation.RuntimeInterruptedException"></exception>
		public static void SleepThrowsOnInterrupt(long millis)
		{
			try
			{
				Thread.Sleep(millis);
			}
			catch (Exception e)
			{
				throw new RuntimeInterruptedException(e.ToString());
			}
		}

		/// <summary>
		/// Keeps executing a block of code until it either returns true or millisecondsTimeout
		/// elapses.
		/// </summary>
		/// <remarks>
		/// Keeps executing a block of code until it either returns true or millisecondsTimeout
		/// elapses.
		/// </remarks>
		public static bool Retry(long millisecondsTimeout, IClosure4 block)
		{
			return Retry(millisecondsTimeout, 1, block);
		}

		public static bool Retry(long millisecondsTimeout, int millisecondsBetweenRetries
			, IClosure4 block)
		{
			StopWatch watch = new AutoStopWatch();
			do
			{
				if ((((bool)block.Run())))
				{
					return true;
				}
				Sleep(millisecondsBetweenRetries);
			}
			while (watch.Peek() < millisecondsTimeout);
			return false;
		}
	}
}
