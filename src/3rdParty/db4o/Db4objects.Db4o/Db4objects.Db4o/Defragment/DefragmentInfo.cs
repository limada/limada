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
namespace Db4objects.Db4o.Defragment
{
	/// <summary>A message from the defragmentation process.</summary>
	/// <remarks>
	/// A message from the defragmentation process. This is a stub only
	/// and will be refined.
	/// Currently instances of these class will only be created and sent
	/// to registered listeners when invalid IDs are encountered during
	/// the defragmentation process. These probably are harmless and the
	/// result of a user-initiated delete operation.
	/// </remarks>
	/// <seealso cref="Defragment">Defragment</seealso>
	public class DefragmentInfo
	{
		private string _msg;

		public DefragmentInfo(string msg)
		{
			_msg = msg;
		}

		public override string ToString()
		{
			return _msg;
		}
	}
}
