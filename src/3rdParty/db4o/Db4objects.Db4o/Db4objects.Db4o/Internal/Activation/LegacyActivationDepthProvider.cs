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
	public class LegacyActivationDepthProvider : IActivationDepthProvider
	{
		public static readonly IActivationDepthProvider Instance = new LegacyActivationDepthProvider
			();

		public virtual IActivationDepth ActivationDepthFor(ClassMetadata classMetadata, ActivationMode
			 mode)
		{
			if (mode.IsPrefetch())
			{
				return new LegacyActivationDepth(1, mode);
			}
			int globalLegacyActivationDepth = ConfigImpl(classMetadata).ActivationDepth();
			Config4Class config = classMetadata.ConfigOrAncestorConfig();
			int defaultDepth = null == config ? globalLegacyActivationDepth : config.AdjustActivationDepth
				(globalLegacyActivationDepth);
			return new LegacyActivationDepth(defaultDepth, mode);
		}

		public virtual IActivationDepth ActivationDepth(int depth, ActivationMode mode)
		{
			return new LegacyActivationDepth(depth, mode);
		}

		private Config4Impl ConfigImpl(ClassMetadata classMetadata)
		{
			return classMetadata.Container().ConfigImpl;
		}
	}
}
