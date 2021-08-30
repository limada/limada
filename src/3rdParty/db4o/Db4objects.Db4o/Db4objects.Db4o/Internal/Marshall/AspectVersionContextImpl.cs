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
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	/// <exclude></exclude>
	public class AspectVersionContextImpl : IAspectVersionContext
	{
		private readonly int _declaredAspectCount;

		private AspectVersionContextImpl(int count)
		{
			_declaredAspectCount = count;
		}

		public virtual int DeclaredAspectCount()
		{
			return _declaredAspectCount;
		}

		public virtual void DeclaredAspectCount(int count)
		{
			throw new InvalidOperationException();
		}

		public static readonly Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl
			 AlwaysEnabled = new Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl(
			int.MaxValue);

		public static readonly Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl
			 CheckAlwaysEnabled = new Db4objects.Db4o.Internal.Marshall.AspectVersionContextImpl
			(int.MaxValue - 1);
	}
}
