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
namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class HandlerVersion
	{
		public readonly int _number;

		public static readonly Db4objects.Db4o.Internal.Handlers.HandlerVersion Invalid = 
			new Db4objects.Db4o.Internal.Handlers.HandlerVersion(-1);

		public HandlerVersion(int number)
		{
			_number = number;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			return ((Db4objects.Db4o.Internal.Handlers.HandlerVersion)obj)._number == _number;
		}
	}
}
