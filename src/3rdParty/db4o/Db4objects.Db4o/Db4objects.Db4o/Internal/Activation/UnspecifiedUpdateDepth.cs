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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public abstract class UnspecifiedUpdateDepth : IUpdateDepth
	{
		protected UnspecifiedUpdateDepth()
		{
		}

		public virtual bool SufficientDepth()
		{
			return true;
		}

		public virtual bool Negative()
		{
			return true;
		}

		public override string ToString()
		{
			return GetType().FullName;
		}

		public virtual IUpdateDepth Adjust(ClassMetadata clazz)
		{
			FixedUpdateDepth depth = (FixedUpdateDepth)ForDepth(clazz.UpdateDepthFromConfig()
				).Descend();
			return depth;
		}

		public virtual IUpdateDepth AdjustUpdateDepthForCascade(bool isCollection)
		{
			throw new InvalidOperationException();
		}

		public virtual IUpdateDepth Descend()
		{
			throw new InvalidOperationException();
		}

		protected abstract FixedUpdateDepth ForDepth(int depth);

		public abstract bool CanSkip(ObjectReference arg1);
	}
}
