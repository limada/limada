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
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class Serializer
	{
		public static StatefulBuffer Marshall(Transaction ta, object obj)
		{
			SerializedGraph serialized = Marshall(ta.Container(), obj);
			StatefulBuffer buffer = new StatefulBuffer(ta, serialized.Length());
			buffer.Append(serialized._bytes);
			buffer.UseSlot(serialized._id, 0, serialized.Length());
			return buffer;
		}

		public static SerializedGraph Marshall(ObjectContainerBase serviceProvider, object
			 obj)
		{
			MemoryBin memoryBin = new MemoryBin(223, GrowthStrategy());
			TransportObjectContainer carrier = NewTransportObjectContainer(serviceProvider, memoryBin
				);
			carrier.ProduceClassMetadata(carrier.Reflector().ForObject(obj));
			carrier.Store(obj);
			int id = (int)carrier.GetID(obj);
			carrier.Close();
			return new SerializedGraph(id, memoryBin.Data());
		}

		private static ConstantGrowthStrategy GrowthStrategy()
		{
			return new ConstantGrowthStrategy(300);
		}

		private static TransportObjectContainer NewTransportObjectContainer(ObjectContainerBase
			 serviceProvider, MemoryBin memoryBin)
		{
			TransportObjectContainer container = new TransportObjectContainer(serviceProvider
				, memoryBin);
			container.DeferredOpen();
			return container;
		}

		public static object Unmarshall(ObjectContainerBase serviceProvider, StatefulBuffer
			 buffer)
		{
			return Unmarshall(serviceProvider, buffer._buffer, buffer.GetID());
		}

		public static object Unmarshall(ObjectContainerBase serviceProvider, SerializedGraph
			 serialized)
		{
			return Unmarshall(serviceProvider, serialized._bytes, serialized._id);
		}

		public static object Unmarshall(ObjectContainerBase serviceProvider, byte[] bytes
			, int id)
		{
			if (id <= 0)
			{
				return null;
			}
			MemoryBin memoryBin = new MemoryBin(bytes, GrowthStrategy());
			TransportObjectContainer carrier = NewTransportObjectContainer(serviceProvider, memoryBin
				);
			object obj = carrier.GetByID(id);
			carrier.Activate(carrier.Transaction, obj, new FullActivationDepth());
			carrier.Close();
			return obj;
		}
	}
}
