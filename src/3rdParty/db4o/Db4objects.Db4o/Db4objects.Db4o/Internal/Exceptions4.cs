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
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class Exceptions4
	{
		public static void ThrowRuntimeException(int code)
		{
			ThrowRuntimeException(code, null, null);
		}

		public static void ThrowRuntimeException(int code, Exception cause)
		{
			ThrowRuntimeException(code, null, cause);
		}

		public static void ThrowRuntimeException(int code, string msg)
		{
			ThrowRuntimeException(code, msg, null);
		}

		public static void ThrowRuntimeException(int code, string msg, Exception cause)
		{
			ThrowRuntimeException(code, msg, cause, true);
		}

		[System.ObsoleteAttribute]
		public static void ThrowRuntimeException(int code, string msg, Exception cause, bool
			 doLog)
		{
			if (doLog)
			{
				Db4objects.Db4o.Internal.Messages.LogErr(Db4oFactory.Configure(), code, msg, cause
					);
			}
			throw new Db4oException(Db4objects.Db4o.Internal.Messages.Get(code, msg));
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oException"></exception>
		public static void CatchAllExceptDb4oException(Exception exc)
		{
			if (exc is Db4oException)
			{
				throw (Db4oException)exc;
			}
		}

		public static Exception ShouldNeverBeCalled()
		{
			throw new Exception();
		}

		public static void ShouldNeverHappen()
		{
			throw new Exception();
		}

		public static Exception VirtualException()
		{
			throw new Exception();
		}
	}
}
