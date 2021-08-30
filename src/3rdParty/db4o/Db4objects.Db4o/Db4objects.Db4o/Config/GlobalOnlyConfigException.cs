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

namespace Db4objects.Db4o.Config
{
	/// <summary>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a global configuration
	/// setting is attempted on an open object container.
	/// </summary>
	/// <remarks>
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when a global configuration
	/// setting is attempted on an open object container.
	/// </remarks>
	/// <seealso cref="IConfiguration.BlockSize(int)">IConfiguration.BlockSize(int)</seealso>
	/// <seealso cref="IConfiguration.Encrypt(bool)">IConfiguration.Encrypt(bool)</seealso>
	/// <seealso cref="IConfiguration.Io(Db4objects.Db4o.IO.IoAdapter)">IConfiguration.Io(Db4objects.Db4o.IO.IoAdapter)
	/// 	</seealso>
	/// <seealso cref="IConfiguration.Password(string)">IConfiguration.Password(string)</seealso>
	[System.Serializable]
	public class GlobalOnlyConfigException : Db4oRecoverableException
	{
	}
}
