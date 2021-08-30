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
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	/// <summary>
	/// Activates a fixed depth of the object graph regardless of
	/// any existing activation depth configuration settings.
	/// </summary>
	/// <remarks>
	/// Activates a fixed depth of the object graph regardless of
	/// any existing activation depth configuration settings.
	/// </remarks>
	public class FixedActivationDepth : ActivationDepthImpl
	{
		private readonly int _depth;

		public FixedActivationDepth(int depth, ActivationMode mode) : base(mode)
		{
			_depth = depth;
		}

		public FixedActivationDepth(int depth) : this(depth, ActivationMode.Activate)
		{
		}

		public override bool RequiresActivation()
		{
			return _depth > 0;
		}

		public override IActivationDepth Descend(ClassMetadata metadata)
		{
			if (_depth < 1)
			{
				return this;
			}
			return new Db4objects.Db4o.Internal.Activation.FixedActivationDepth(_depth - 1, _mode
				);
		}

		// TODO code duplication in fixed activation/update depth
		public virtual Db4objects.Db4o.Internal.Activation.FixedActivationDepth AdjustDepthToBorders
			()
		{
			return new Db4objects.Db4o.Internal.Activation.FixedActivationDepth(DepthUtil.AdjustDepthToBorders
				(_depth));
		}
	}
}
