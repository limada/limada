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
using Db4objects.Db4o.CS.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public abstract class ShutdownMode
	{
		public class NormalMode : ShutdownMode
		{
			internal NormalMode()
			{
			}

			public override bool IsFatal()
			{
				return false;
			}
		}

		public class FatalMode : ShutdownMode
		{
			private Exception _exc;

			internal FatalMode(Exception exc)
			{
				_exc = exc;
			}

			public virtual Exception Exc()
			{
				return _exc;
			}

			public override bool IsFatal()
			{
				return true;
			}
		}

		public static readonly ShutdownMode Normal = new ShutdownMode.NormalMode();

		public static ShutdownMode Fatal(Exception exc)
		{
			return new ShutdownMode.FatalMode(exc);
		}

		public abstract bool IsFatal();

		private ShutdownMode()
		{
		}
	}
}
