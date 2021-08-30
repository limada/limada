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
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal.Encoding;

namespace Db4objects.Db4o.Config.Encoding
{
	/// <summary>All built in String encodings</summary>
	/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.StringEncoding(IStringEncoding)
	/// 	">Db4objects.Db4o.Config.IConfiguration.StringEncoding(IStringEncoding)</seealso>
	public class StringEncodings
	{
		public static IStringEncoding Utf8()
		{
			return new UTF8StringEncoding();
		}

		public static IStringEncoding Unicode()
		{
			return new UnicodeStringEncoding();
		}

		public static IStringEncoding Latin()
		{
			return new LatinStringEncoding();
		}
	}
}
