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
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// extended client functionality for the
	/// <see cref="IExtObjectContainer">IExtObjectContainer</see>
	/// interface.
	/// <br /><br />Both
	/// <see cref="Db4objects.Db4o.Db4oFactory.OpenClient(string, int, string, string)">Db4o.openClient()
	/// 	</see>
	/// methods always
	/// return an <code>ExtClient</code> object so a cast is possible.<br /><br />
	/// The ObjectContainer functionality is split into multiple interfaces to allow newcomers to
	/// focus on the essential methods.
	/// </summary>
	public interface IExtClient : IExtObjectContainer
	{
		/// <summary>checks if the client is currently connected to a server.</summary>
		/// <remarks>checks if the client is currently connected to a server.</remarks>
		/// <returns>true if the client is alive.</returns>
		bool IsAlive();
	}
}
