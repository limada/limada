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

namespace Db4objects.Db4o.Ext
{
	/// <summary>
	/// extended functionality for the
	/// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
	/// interface.
	/// <br /><br />Every db4o
	/// <see cref="Db4objects.Db4o.IObjectSet">IObjectSet</see>
	/// always is an ExtObjectSet so a cast is possible.<br /><br />
	/// <see cref="Db4objects.Db4o.IObjectSet.Ext()">Db4objects.Db4o.IObjectSet.Ext()</see>
	/// is a convenient method to perform the cast.<br /><br />
	/// The ObjectSet functionality is split to two interfaces to allow newcomers to
	/// focus on the essential methods.
	/// </summary>
	public interface IExtObjectSet : IObjectSet
	{
		/// <summary>returns an array of internal IDs that correspond to the contained objects.
		/// 	</summary>
		/// <remarks>
		/// returns an array of internal IDs that correspond to the contained objects.
		/// <br /><br />
		/// </remarks>
		/// <seealso cref="IExtObjectContainer.GetID(object)">IExtObjectContainer.GetID(object)
		/// 	</seealso>
		/// <seealso cref="IExtObjectContainer.GetByID(long)">IExtObjectContainer.GetByID(long)
		/// 	</seealso>
		long[] GetIDs();

		/// <summary>returns the item at position [index] in this ObjectSet.</summary>
		/// <remarks>
		/// returns the item at position [index] in this ObjectSet.
		/// <br /><br />
		/// The object will be activated.
		/// </remarks>
		/// <param name="index">the index position in this ObjectSet.</param>
		/// <returns>the activated object.</returns>
		object Get(int index);
	}
}
