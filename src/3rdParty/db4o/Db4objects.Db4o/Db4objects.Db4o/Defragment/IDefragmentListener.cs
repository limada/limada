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
using Db4objects.Db4o.Defragment;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>Listener for defragmentation process messages.</summary>
	/// <remarks>Listener for defragmentation process messages.</remarks>
	/// <seealso cref="Defragment">Defragment</seealso>
	public interface IDefragmentListener
	{
		/// <summary>
		/// This method will be called when the defragment process encounters
		/// file layout anomalies during the defragmentation process.
		/// </summary>
		/// <remarks>
		/// This method will be called when the defragment process encounters
		/// file layout anomalies during the defragmentation process.
		/// </remarks>
		/// <param name="info">The message from the defragmentation process.</param>
		void NotifyDefragmentInfo(DefragmentInfo info);
	}
}
