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
using System;
using System.IO;

using Sharpen.Lang;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{
#if !CF && !SILVERLIGHT
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;

	/// <summary>
	/// translator for types that are marked with the Serializable attribute.
	/// The Serializable translator is provided to allow persisting objects that
	/// do not supply a convenient constructor. The use of this translator is
	/// recommended only if:<br />
	/// - the persistent type will never be refactored<br />
	/// - querying for type members is not necessary<br />
	/// </summary>
	public class TSerializable : IObjectConstructor
	{
		public Object OnStore(IObjectContainer objectContainer, Object obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize(memoryStream, obj);
			return memoryStream.GetBuffer();
		}

		public void OnActivate(IObjectContainer objectContainer, Object obj, Object members)
		{
		}

		public Object OnInstantiate(IObjectContainer objectContainer, Object obj)
		{
			MemoryStream memoryStream = new MemoryStream((byte[])obj);
			return new BinaryFormatter().Deserialize(memoryStream);
		}

		public System.Type StoredClass()
		{
			return typeof(byte[]);
		}

	}
#endif
}
