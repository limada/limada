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
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class TimeStampIdGenerator
	{
		public const int BitsReservedForCounter = 15;

		public const int CounterLimit = 64;

		private long _counter;

		private long _lastTime;

		public static long IdToMilliseconds(long id)
		{
			return id >> BitsReservedForCounter;
		}

		public static long MillisecondsToId(long milliseconds)
		{
			return milliseconds << BitsReservedForCounter;
		}

		public TimeStampIdGenerator(long minimumNext)
		{
			InternalSetMinimumNext(minimumNext);
		}

		public TimeStampIdGenerator() : this(0)
		{
		}

		public virtual long Generate()
		{
			long t = Now();
			if (t > _lastTime)
			{
				_lastTime = t;
				_counter = 0;
				return MillisecondsToId(t);
			}
			UpdateTimeOnCounterLimitOverflow();
			_counter++;
			UpdateTimeOnCounterLimitOverflow();
			return Last();
		}

		protected virtual long Now()
		{
			return Runtime.CurrentTimeMillis();
		}

		private void UpdateTimeOnCounterLimitOverflow()
		{
			if (_counter < CounterLimit)
			{
				return;
			}
			long timeIncrement = _counter / CounterLimit;
			_lastTime += timeIncrement;
			_counter -= (timeIncrement * CounterLimit);
		}

		public virtual long Last()
		{
			return MillisecondsToId(_lastTime) + _counter;
		}

		public virtual bool SetMinimumNext(long newMinimum)
		{
			if (newMinimum <= Last())
			{
				return false;
			}
			InternalSetMinimumNext(newMinimum);
			return true;
		}

		private void InternalSetMinimumNext(long newNext)
		{
			_lastTime = IdToMilliseconds(newNext);
			long timePart = MillisecondsToId(_lastTime);
			_counter = newNext - timePart;
			UpdateTimeOnCounterLimitOverflow();
		}
	}
}
