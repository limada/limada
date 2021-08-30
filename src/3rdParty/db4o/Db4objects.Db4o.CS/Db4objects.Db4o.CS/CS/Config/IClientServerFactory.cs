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
	/// <summary>factory to open C/S server and client implementations.</summary>
	/// <remarks>factory to open C/S server and client implementations.</remarks>
	/// <seealso cref="Db4objects.Db4o.Db4oFactory.OpenClient(Db4objects.Db4o.Config.IConfiguration, string, int, string, string)
	/// 	">Db4objects.Db4o.Db4oFactory.OpenClient(Db4objects.Db4o.Config.IConfiguration, string, int, string, string)
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.Db4oFactory.OpenServer(Db4objects.Db4o.Config.IConfiguration, string, int)
	/// 	"></seealso>
	public interface IClientServerFactory
	{
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidPasswordException"></exception>
		IObjectContainer OpenClient(IClientConfiguration config, string hostName, int port
			, string user, string password);

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.IncompatibleFileFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseFileLockedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		IObjectServer OpenServer(IServerConfiguration config, string databaseFileName, int
			 port);
	}
}
