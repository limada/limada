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
	/// db4o-specific exception.<br /><br />
	/// This exception is thrown when an old file format was detected
	/// and
	/// <see cref="Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates(bool)">Db4objects.Db4o.Config.IConfiguration.AllowVersionUpdates(bool)
	/// 	</see>
	/// is set to false.
	/// </summary>
	[System.Serializable]
	public class OldFormatException : Db4oFatalException
	{
		/// <summary>Constructor with the default message.</summary>
		/// <remarks>Constructor with the default message.</remarks>
		public OldFormatException() : base(Db4objects.Db4o.Internal.Messages.OldDatabaseFormat
			)
		{
		}
	}
}
