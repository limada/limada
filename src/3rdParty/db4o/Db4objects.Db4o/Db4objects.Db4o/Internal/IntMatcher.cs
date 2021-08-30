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
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class IntMatcher
	{
		public abstract bool Match(int i);

		private sealed class _IntMatcher_13 : IntMatcher
		{
			public _IntMatcher_13()
			{
			}

			public override bool Match(int i)
			{
				return i == 0;
			}
		}

		public static readonly IntMatcher Zero = new _IntMatcher_13();

		private sealed class _IntMatcher_19 : IntMatcher
		{
			public _IntMatcher_19()
			{
			}

			public override bool Match(int i)
			{
				return i > 0;
			}
		}

		public static readonly IntMatcher Positive = new _IntMatcher_19();

		private sealed class _IntMatcher_25 : IntMatcher
		{
			public _IntMatcher_25()
			{
			}

			public override bool Match(int i)
			{
				return i < 0;
			}
		}

		public static readonly IntMatcher Negative = new _IntMatcher_25();
	}
}
