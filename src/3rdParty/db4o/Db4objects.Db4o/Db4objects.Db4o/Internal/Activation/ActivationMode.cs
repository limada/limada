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
namespace Db4objects.Db4o.Internal.Activation
{
	public sealed class ActivationMode
	{
		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Activate
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Deactivate
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Peek = 
			new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Prefetch
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		public static readonly Db4objects.Db4o.Internal.Activation.ActivationMode Refresh
			 = new Db4objects.Db4o.Internal.Activation.ActivationMode();

		private ActivationMode()
		{
		}

		public override string ToString()
		{
			if (IsActivate())
			{
				return "ACTIVATE";
			}
			if (IsDeactivate())
			{
				return "DEACTIVATE";
			}
			if (IsPrefetch())
			{
				return "PREFETCH";
			}
			if (IsRefresh())
			{
				return "REFRESH";
			}
			return "PEEK";
		}

		public bool IsDeactivate()
		{
			return this == Deactivate;
		}

		public bool IsActivate()
		{
			return this == Activate;
		}

		public bool IsPeek()
		{
			return this == Peek;
		}

		public bool IsPrefetch()
		{
			return this == Prefetch;
		}

		public bool IsRefresh()
		{
			return this == Refresh;
		}
	}
}
