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
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Ext
{
	/// <summary>extended functionality for the ObjectServer interface.</summary>
	/// <remarks>
	/// extended functionality for the ObjectServer interface.
	/// <br /><br />Every ObjectServer also always is an ExtObjectServer
	/// so a cast is possible.<br /><br />
	/// <see cref="Db4objects.Db4o.IObjectServer.Ext()">Db4objects.Db4o.IObjectServer.Ext()
	/// 	</see>
	/// is a convenient method to perform the cast.<br /><br />
	/// The functionality is split to two interfaces to allow newcomers to
	/// focus on the essential methods.
	/// </remarks>
	public interface IExtObjectServer : IObjectServer
	{
		/// <summary>backs up the database file used by the ObjectServer.</summary>
		/// <remarks>
		/// backs up the database file used by the ObjectServer.
		/// <br /><br />While the backup is running, the ObjectServer can continue to be
		/// used. Changes that are made while the backup is in progress, will be applied to
		/// the open ObjectServer and to the backup.<br /><br />
		/// While the backup is running, the ObjectContainer should not be closed.<br /><br />
		/// If a file already exists at the specified path, it will be overwritten.<br /><br />
		/// </remarks>
		/// <param name="path">a fully qualified path</param>
		/// <exception cref="System.IO.IOException"></exception>
		void Backup(string path);

		/// <summary>returns the number of connected clients.</summary>
		/// <remarks>returns the number of connected clients.</remarks>
		int ClientCount();

		/// <summary>
		/// returns the
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">Db4objects.Db4o.Config.IConfiguration
		/// 	</see>
		/// context for this ObjectServer.
		/// <br /><br />
		/// Upon opening an ObjectServer with any of the factory methods in the
		/// <see cref="Db4objects.Db4o.Db4oFactory">Db4objects.Db4o.Db4oFactory</see>
		/// class, the global
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">Db4objects.Db4o.Config.IConfiguration
		/// 	</see>
		/// context
		/// is copied into the ObjectServer. The
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">Db4objects.Db4o.Config.IConfiguration
		/// 	</see>
		/// can be modified individually for
		/// each ObjectServer without any effects on the global settings.<br /><br />
		/// </summary>
		/// <returns>the Configuration context for this ObjectServer</returns>
		/// <seealso cref="Db4objects.Db4o.Db4oFactory.Configure()">Db4objects.Db4o.Db4oFactory.Configure()
		/// 	</seealso>
		IConfiguration Configure();

		/// <summary>returns the ObjectContainer used by the server.</summary>
		/// <remarks>
		/// returns the ObjectContainer used by the server.
		/// <br /><br />
		/// </remarks>
		/// <returns>the ObjectContainer used by the server</returns>
		IObjectContainer ObjectContainer();

		/// <summary>removes client access permissions for the specified user.</summary>
		/// <remarks>
		/// removes client access permissions for the specified user.
		/// <br /><br />
		/// </remarks>
		/// <param name="userName">the name of the user</param>
		void RevokeAccess(string userName);

		/// <returns>The local port this server uses, 0 if disconnected or in embedded mode</returns>
		int Port();
	}
}
