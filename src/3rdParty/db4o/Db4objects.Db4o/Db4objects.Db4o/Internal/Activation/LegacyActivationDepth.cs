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
	/// <summary>
	/// Activates an object graph to a specific depth respecting any
	/// activation configuration settings that might be in effect.
	/// </summary>
	/// <remarks>
	/// Activates an object graph to a specific depth respecting any
	/// activation configuration settings that might be in effect.
	/// </remarks>
	public class LegacyActivationDepth : ActivationDepthImpl
	{
		private readonly int _depth;

		public LegacyActivationDepth(int depth) : this(depth, ActivationMode.Activate)
		{
		}

		public LegacyActivationDepth(int depth, ActivationMode mode) : base(mode)
		{
			_depth = depth;
		}

		public override IActivationDepth Descend(ClassMetadata metadata)
		{
			if (null == metadata)
			{
				return new Db4objects.Db4o.Internal.Activation.LegacyActivationDepth(_depth - 1, 
					_mode);
			}
			return new Db4objects.Db4o.Internal.Activation.LegacyActivationDepth(DescendDepth
				(metadata), _mode);
		}

		private int DescendDepth(ClassMetadata metadata)
		{
			int depth = ConfiguredActivationDepth(metadata) - 1;
			if (metadata.IsStruct())
			{
				// 	We also have to instantiate structs completely every time.
				return Math.Max(1, depth);
			}
			return depth;
		}

		private int ConfiguredActivationDepth(ClassMetadata metadata)
		{
			Config4Class config = metadata.ConfigOrAncestorConfig();
			if (config != null && _mode.IsActivate())
			{
				return config.AdjustActivationDepth(_depth);
			}
			return _depth;
		}

		public override bool RequiresActivation()
		{
			return _depth > 0;
		}
	}
}
