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
namespace Db4objects.Db4o.Ext
{
	/// <summary>Static constants to describe the status of objects.</summary>
	/// <remarks>Static constants to describe the status of objects.</remarks>
	public class Status
	{
		public const double Unused = -1.0;

		public const double Available = -2.0;

		public const double Queued = -3.0;

		public const double Completed = -4.0;

		public const double Processing = -5.0;

		public const double Error = -99.0;
	}
}
