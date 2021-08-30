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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o
{
	/// <summary>factory class to start db4o database engines.</summary>
	/// <remarks>
	/// factory class to start db4o database engines.
	/// <br /><br />This class provides static methods to<br />
	/// - open single-user databases
	/// <see cref="OpenFile(string)">OpenFile(string)</see>
	/// <br />
	/// - open db4o servers
	/// <see cref="OpenServer(string, int)">OpenServer(string, int)</see>
	/// <br />
	/// - connect to db4o servers
	/// <see cref="OpenClient(string, int, string, string)">OpenClient(string, int, string, string)
	/// 	</see>
	/// <br />
	/// - provide access to the global configuration context
	/// <see cref="Configure()">Configure()</see>
	/// <br />
	/// - print the version number of this db4o version
	/// <see cref="Main(java.lang.String[])">Main(java.lang.String[])</see>
	/// 
	/// </remarks>
	/// <seealso cref="ExtDb4o">ExtDb4o for extended functionality.</seealso>
	public class Db4oFactory
	{
		internal static readonly Config4Impl i_config = new Config4Impl();

		static Db4oFactory()
		{
			Platform4.GetDefaultConfiguration(i_config);
		}

		/// <summary>prints the version name of this db4o version to <code>System.out</code>.
		/// 	</summary>
		/// <remarks>prints the version name of this db4o version to <code>System.out</code>.
		/// 	</remarks>
		public static void Main(string[] args)
		{
			Sharpen.Runtime.Out.WriteLine(Version());
		}

		/// <summary>
		/// returns the global db4o
		/// <see cref="IConfiguration">IConfiguration</see>
		/// context
		/// for the running CLR session.
		/// <br/><br/>
		/// The
		/// <see cref="IConfiguration">IConfiguration</see>
		/// can be overriden in each
		/// <see cref="IExtObjectContainer.Configure">ObjectContainer</see>
		/// .<br/><br/>
		/// </summary>
		/// <returns>
		/// the global
		/// <see cref="IConfiguration">configuration</see>
		/// context
		/// 
		/// </returns>
		public static IConfiguration Configure()
		{
			return i_config;
		}

		/// <summary>
		/// Creates a fresh
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// instance.
		/// </summary>
		/// <returns>a fresh, independent configuration with all options set to their default values
		/// 	</returns>
		[System.ObsoleteAttribute(@"Use Db4oEmbedded.NewConfiguration() instead.")]
		public static IConfiguration NewConfiguration()
		{
			Config4Impl config = new Config4Impl();
			Platform4.GetDefaultConfiguration(config);
			return config;
		}

		/// <summary>
		/// Creates a clone of the global db4o
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// .
		/// </summary>
		/// <returns>
		/// a fresh configuration with all option values set to the values
		/// currently configured for the global db4o configuration context
		/// </returns>
		[System.ObsoleteAttribute(@"use explicit configuration via Db4oEmbedded.NewConfiguration() instead"
			)]
		public static IConfiguration CloneConfiguration()
		{
			return (Config4Impl)((IDeepClone)Db4oFactory.Configure()).DeepClone(null);
		}

		/// <summary>
		/// Operates just like
		/// <see cref="Db4objects.Db4o.Db4oFactory.OpenClient">
		/// Db4objects.Db4o.Db4oFactory.OpenClient
		/// </see>, but uses
		/// the global db4o
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// context.
		/// opens an
		/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
		/// client and connects it to the specified named server and port.
		/// <br/><br/>
		/// The server needs to
		/// <see cref="Db4objects.Db4o.IObjectServer.GrantAccess">allow access</see>
		/// for the specified user and password.
		/// <br/><br/>
		/// A client
		/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
		/// can be cast to
		/// <see cref="Db4objects.Db4o.Ext.IExtClient">IExtClient</see>
		/// to use extended
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer">IExtObjectContainer</see>
		/// 
		/// and
		/// <see cref="Db4objects.Db4o.Ext.IExtClient">IExtClient</see>
		/// methods.
		/// <br/><br/>
		/// This method is obsolete, see the Db4objects.Db4o.CS.Db4oClientServer class in
		/// Db4objects.Db4o.CS.dll for methods to open db4o servers and db4o clients.
		/// </summary>
		/// <param name="config">
		/// a custom
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// instance to be obtained via
		/// <see cref="Db4objects.Db4o.Db4oEmbedded.NewConfiguration">
		/// Db4objects.Db4o.Db4oEmbedded.NewConfiguration
		/// </see>
		/// </param>
		/// <param name="hostName">the host name</param>
		/// <param name="port">the port the server is using</param>
		/// <param name="user">the user name</param>
		/// <param name="password">the user password</param>
		/// <returns>
		/// an open
		/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
		/// </returns>
		/// <seealso cref="Db4objects.Db4o.IObjectServer.GrantAccess">
		/// Db4objects.Db4o.IObjectServer.GrantAccess
		/// </seealso>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">
		/// I/O operation failed or was unexpectedly interrupted.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException">
		/// open operation failed because the database file
		/// is in old format and
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates">
		/// Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates
		/// </see>
		/// 
		/// is set to false.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidPasswordException">
		/// password supplied for the connection is
		/// invalid.
		/// </exception>
		public static IObjectContainer OpenClient(string hostName, int port, string user, 
			string password)
		{
			return OpenClient(Db4oFactory.CloneConfiguration(), hostName, port, user, password
				);
		}

		/// <summary>
		/// opens an
		/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
		/// client and connects it to the specified named server and port.
		/// <br/><br/>
		/// The server needs to
		/// <see cref="Db4objects.Db4o.IObjectServer.GrantAccess">allow access</see>
		/// for the specified user and password.
		/// <br/><br/>
		/// A client
		/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
		/// can be cast to
		/// <see cref="Db4objects.Db4o.Ext.IExtClient">IExtClient</see>
		/// to use extended
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectContainer">IExtObjectContainer</see>
		/// 
		/// and
		/// <see cref="Db4objects.Db4o.Ext.IExtClient">IExtClient</see>
		/// methods.
		/// <br/><br/>
		/// This method is obsolete, see the Db4objects.Db4o.CS.Db4oClientServer class in
		/// Db4objects.Db4o.CS.dll for methods to open db4o servers and db4o clients.
		/// </summary>
		/// <param name="config">
		/// a custom
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// instance to be obtained via
		/// <see cref="Db4objects.Db4o.Db4oEmbedded.NewConfiguration">
		/// Db4objects.Db4o.Db4oEmbedded.NewConfiguration
		/// </see>
		/// </param>
		/// <param name="hostName">the host name</param>
		/// <param name="port">the port the server is using</param>
		/// <param name="user">the user name</param>
		/// <param name="password">the user password</param>
		/// <returns>
		/// an open
		/// <see cref="Db4objects.Db4o.IObjectContainer">IObjectContainer</see>
		/// </returns>
		/// <seealso cref="Db4objects.Db4o.IObjectServer.GrantAccess">
		/// Db4objects.Db4o.IObjectServer.GrantAccess
		/// </seealso>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">
		/// I/O operation failed or was unexpectedly interrupted.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException">
		/// open operation failed because the database file
		/// is in old format and
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates">
		/// Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates
		/// </see>
		/// 
		/// is set to false.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidPasswordException">
		/// password supplied for the connection is
		/// invalid.
		/// </exception>
		public static IObjectContainer OpenClient(IConfiguration config, string hostName, 
			int port, string user, string password)
		{
			return ((Config4Impl)config).ClientServerFactory().OpenClient(config, hostName, port
				, user, password);
		}

		/// <summary>
		/// Operates just like
		/// <see cref="Db4oFactory.OpenFile">Db4oFactory.OpenFile</see>
		/// , but uses
		/// the global db4o
		/// <see cref="IConfiguration">IConfiguration</see>
		/// context.
		/// opens an
		/// <see cref="IObjectContainer">IObjectContainer</see>
		/// on the specified database file for local use.
		/// <br/><br/>A database file can only be opened once, subsequent attempts to open
		/// another
		/// <see cref="IObjectContainer">IObjectContainer</see>
		/// against the same file will result in
		/// a
		/// <see cref="DatabaseFileLockedException">DatabaseFileLockedException</see>
		/// .<br/><br/>
		/// Database files can only be accessed for readwrite access from one process
		/// at one time. All versions except for db4o mobile edition use an
		/// internal mechanism to lock the database file for other processes.
		/// <br/><br/>
		/// 
		/// </summary>
		/// <param name="databaseFileName">an absolute or relative path to the database file</param>
		/// <returns>
		/// an open
		/// <see cref="IObjectContainer">IObjectContainer</see>
		/// 
		/// </returns>
		/// <seealso cref="IConfiguration.ReadOnly">IConfiguration.ReadOnly</seealso>
		/// <seealso cref="IConfiguration.Encrypt">IConfiguration.Encrypt</seealso>
		/// <seealso cref="IConfiguration.Password">IConfiguration.Password</seealso>
		/// <exception cref="Db4oIOException">
		/// I/O operation failed or was unexpectedly interrupted.
		/// 
		/// </exception>
		/// <exception cref="DatabaseFileLockedException">
		/// the required database file is locked by
		/// another process.
		/// 
		/// </exception>
		/// <exception cref="IncompatibleFileFormatException">
		/// runtime
		/// <see cref="IConfiguration">configuration</see>
		/// is not compatible
		/// with the configuration of the database file.
		/// 
		/// </exception>
		/// <exception cref="OldFormatException">
		/// open operation failed because the database file
		/// is in old format and
		/// <see cref="IConfiguration.AllowVersionUpdates">
		/// IConfiguration.AllowVersionUpdates
		/// </see>
		/// is set to false.
		/// </exception>
		/// <exception cref="DatabaseReadOnlyException">
		/// database was configured as read-only.
		/// </exception>
		public static IObjectContainer OpenFile(string databaseFileName)
		{
			return Db4oFactory.OpenFile(CloneConfiguration(), databaseFileName);
		}

		/// <summary>
		/// opens an
		/// <see cref="IObjectContainer">IObjectContainer</see>
		/// on the specified database file for local use.
		/// <br/><br/>A database file can only be opened once, subsequent attempts to open
		/// another
		/// <see cref="IObjectContainer">IObjectContainer</see>
		/// against the same file will result in
		/// a
		/// <see cref="DatabaseFileLockedException">DatabaseFileLockedException</see>
		/// .<br/><br/>
		/// Database files can only be accessed for readwrite access from one process
		/// at one time. All versions except for db4o mobile edition use an
		/// internal mechanism to lock the database file for other processes.
		/// <br/><br/>
		/// 
		/// </summary>
		/// <param name="config">
		/// a custom
		/// <see cref="IConfiguration">IConfiguration</see>
		/// instance to be obtained via
		/// <see cref="Db4oFactory.NewConfiguration">Db4oFactory.NewConfiguration</see>
		/// 
		/// </param>
		/// <param name="databaseFileName">an absolute or relative path to the database file</param>
		/// <returns>
		/// an open
		/// <see cref="IObjectContainer">IObjectContainer</see>
		/// 
		/// </returns>
		/// <seealso cref="IConfiguration.ReadOnly">IConfiguration.ReadOnly</seealso>
		/// <seealso cref="IConfiguration.Encrypt">IConfiguration.Encrypt</seealso>
		/// <seealso cref="IConfiguration.Password">IConfiguration.Password</seealso>
		/// <exception cref="Db4oIOException">
		/// I/O operation failed or was unexpectedly interrupted.
		/// 
		/// </exception>
		/// <exception cref="DatabaseFileLockedException">
		/// the required database file is locked by
		/// another process.
		/// 
		/// </exception>
		/// <exception cref="IncompatibleFileFormatException">
		/// runtime
		/// <see cref="IConfiguration">configuration</see>
		/// is not compatible
		/// with the configuration of the database file.
		/// 
		/// </exception>
		/// <exception cref="OldFormatException">
		/// open operation failed because the database file
		/// is in old format and
		/// <see cref="IConfiguration.AllowVersionUpdates">
		/// IConfiguration.AllowVersionUpdates
		/// 
		/// </see>
		/// 
		/// is set to false.
		/// 
		/// </exception>
		/// <exception cref="DatabaseReadOnlyException">
		/// database was configured as read-only.
		/// 
		/// </exception>
		public static IObjectContainer OpenFile(IConfiguration config, string databaseFileName
			)
		{
			return ObjectContainerFactory.OpenObjectContainer(Db4oLegacyConfigurationBridge.AsEmbeddedConfiguration
				(config), databaseFileName);
		}

		/// <summary>
		/// Operates just like
		/// <see cref="Db4objects.Db4o.Db4oFactory.OpenServer">
		/// Db4objects.Db4o.Db4oFactory.OpenServer
		/// </see>
		/// , but uses
		/// the global db4o
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// context.
		/// Opens an
		/// <see cref="Db4objects.Db4o.IObjectServer">IObjectServer</see>
		/// on the specified database file and port.
		/// <br/><br/>
		/// If the server does not need to listen on a port because it will only be used
		/// in embedded mode with
		/// <see cref="Db4objects.Db4o.IObjectServer.OpenClient">
		/// Db4objects.Db4o.IObjectServer.OpenClient
		/// </see>
		/// , specify '0' as the
		/// port number.
		/// <br/><br/>This method is obsolete, see the Db4objects.Db4o.CS.Db4oClientServer class in
		/// Db4objects.Db4o.CS.dll for methods to open db4o servers and db4o clients.
		/// </summary>
		/// <param name="databaseFileName">an absolute or relative path to the database file</param>
		/// <param name="port">
		/// the port to be used, or 0, if the server should not open a port,
		/// because it will only be used with
		/// <see cref="Db4objects.Db4o.IObjectServer.OpenClient">
		/// Db4objects.Db4o.IObjectServer.OpenClient
		/// </see>
		/// .
		/// Specify a value &lt; 0 if an arbitrary free port should be chosen - see
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectServer.Port">
		/// Db4objects.Db4o.Ext.IExtObjectServer.Port
		/// </see>
		/// .
		/// </param>
		/// <returns>
		/// an
		/// <see cref="Db4objects.Db4o.IObjectServer">IObjectServer</see>
		/// listening
		/// on the specified port.
		/// </returns>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ReadOnly">
		/// Db4objects.Db4o.Config.IConfiguration.ReadOnly
		/// </seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.Encrypt">
		/// Db4objects.Db4o.Config.IConfiguration.Encrypt
		/// </seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.Password">
		/// Db4objects.Db4o.Config.IConfiguration.Password
		/// </seealso>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">
		/// I/O operation failed or was unexpectedly interrupted.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseFileLockedException">
		/// the required database file is locked by
		/// another process.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.IncompatibleFileFormatException">
		/// runtime
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">configuration</see>
		/// is not compatible
		/// with the configuration of the database file.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException">
		/// open operation failed because the database file
		/// is in old format and
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates">
		/// Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates
		/// </see>
		/// 
		/// is set to false.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException">
		/// database was configured as read-only.
		/// </exception>
		public static IObjectServer OpenServer(string databaseFileName, int port)
		{
			return OpenServer(CloneConfiguration(), databaseFileName, port);
		}

		/// <summary>
		/// opens an
		/// <see cref="Db4objects.Db4o.IObjectServer">IObjectServer</see>
		/// on the specified database file and port.
		/// <br/><br/>
		/// If the server does not need to listen on a port because it will only be used
		/// in embedded mode with
		/// <see cref="Db4objects.Db4o.IObjectServer.OpenClient">
		/// Db4objects.Db4o.IObjectServer.OpenClient
		/// </see>
		/// , specify '0' as the
		/// port number.
		/// <br/><br/>This method is obsolete, see the Db4objects.Db4o.CS.Db4oClientServer class in
		/// Db4objects.Db4o.CS.dll for methods to open db4o servers and db4o clients.
		/// </summary>
		/// <param name="config">
		/// a custom
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// instance to be obtained via
		/// <see cref="Db4objects.Db4o.Db4oEmbedded.NewConfiguration">
		/// Db4objects.Db4o.Db4oEmbedded.NewConfiguration
		/// </see>
		/// </param>
		/// <param name="databaseFileName">an absolute or relative path to the database file</param>
		/// <param name="port">
		/// the port to be used, or 0, if the server should not open a port,
		/// because it will only be used with
		/// <see cref="Db4objects.Db4o.IObjectServer.OpenClient">
		/// Db4objects.Db4o.IObjectServer.OpenClient
		/// </see>
		/// .
		/// Specify a value &lt; 0 if an arbitrary free port should be chosen - see
		/// <see cref="Db4objects.Db4o.Ext.IExtObjectServer.Port">
		/// Db4objects.Db4o.Ext.IExtObjectServer.Port
		/// </see>
		/// .
		/// </param>
		/// <returns>
		/// an
		/// <see cref="Db4objects.Db4o.IObjectServer">IObjectServer</see>
		/// listening
		/// on the specified port.
		/// </returns>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.ReadOnly">
		/// Db4objects.Db4o.Config.IConfiguration.ReadOnly
		/// </seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.Encrypt">
		/// Db4objects.Db4o.Config.IConfiguration.Encrypt
		/// </seealso>
		/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.Password">
		/// Db4objects.Db4o.Config.IConfiguration.Password
		/// </seealso>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException">
		/// I/O operation failed or was unexpectedly interrupted.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseFileLockedException">
		/// the required database file is locked by
		/// another process.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.IncompatibleFileFormatException">
		/// runtime
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">configuration</see>
		/// is not compatible
		/// with the configuration of the database file.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.OldFormatException">
		/// open operation failed because the database file
		/// is in old format and
		/// <see cref="Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates">
		/// Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates
		/// </see>
		/// 
		/// is set to false.
		/// </exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException">
		/// database was configured as read-only.
		/// </exception>
		public static IObjectServer OpenServer(IConfiguration config, string databaseFileName
			, int port)
		{
			return ((Config4Impl)config).ClientServerFactory().OpenServer(config, databaseFileName
				, port);
		}

		/// <summary>returns the version name of the used db4o version.</summary>
		/// <remarks>
		/// returns the version name of the used db4o version.
		/// <br /><br />
		/// </remarks>
		/// <returns>version information as a <code>String</code>.</returns>
		public static string Version()
		{
			return "db4o " + Db4oVersion.Name;
		}
	}
}
