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
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;

namespace Db4objects.Db4o.CS.Config
{
	/// <summary>
	/// Implement this interface for configuration items that encapsulate
	/// a batch of configuration settings or that need to be applied
	/// to ObjectServers after they are opened.
	/// </summary>
	/// <remarks>
	/// Implement this interface for configuration items that encapsulate
	/// a batch of configuration settings or that need to be applied
	/// to ObjectServers after they are opened.
	/// </remarks>
	/// <since>7.12</since>
	public interface IServerConfigurationItem
	{
		/// <summary>Gives a chance for the item to augment the configuration.</summary>
		/// <remarks>Gives a chance for the item to augment the configuration.</remarks>
		/// <param name="configuration">the configuration that the item was added to</param>
		void Prepare(IServerConfiguration configuration);

		/// <summary>Gives a chance for the item to configure the just opened ObjectContainer.
		/// 	</summary>
		/// <remarks>Gives a chance for the item to configure the just opened ObjectContainer.
		/// 	</remarks>
		/// <param name="container">the ObjectContainer to configure</param>
		void Apply(IObjectServer server);
	}
}
