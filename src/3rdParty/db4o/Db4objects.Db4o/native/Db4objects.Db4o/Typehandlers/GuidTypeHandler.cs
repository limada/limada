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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.native.Db4objects.Db4o.Typehandlers;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Db4o.Typehandlers
{
    /// <summary>
    /// DB4O type handler for efficiently storing and activating System.Guid values.
    /// </summary>
    /// <author>Judah Himango</author>
    public class GuidTypeHandler: IValueTypeHandler, IQueryableTypeHandler, IIndexableTypeHandler
	{
		private const int GuidSize = 16;
		
        #region IValueTypeHandler Members

        public object Read(IReadContext context)
        {
        	return ReadFrom(context);
        }

        #endregion

        #region ITypeHandler4 Members

        public bool CanHold(IReflectClass type)
        {
        	return NetReflector.ToNative(type).Equals(typeof (Guid));
        }

    	public void Defragment(IDefragmentContext context)
        {
            IncrementOffset(context);
        }

        public void Delete(IDeleteContext context)
        {
			context.Seek(context.Offset() + GuidSize);
        }

        public void Write(IWriteContext context, object obj)
        {
        	WriteGuid(obj, context);
        }

    	#endregion

        #region IComparable4 Members

        public IPreparedComparison PrepareComparison(IContext context, object obj)
        {
            return new ComparablePreparedComparison<Guid>(obj);
        }

        #endregion

		#region IQueryableTypeHandler Members

		public bool DescendsIntoMembers()
		{
			return false;
		}

		#endregion

		#region IIndexableTypeHandler Members 

		public int LinkLength()
    	{
    		return GuidSize;
    	}

    	public object ReadIndexEntry(IContext context, ByteArrayBuffer reader)
    	{
    		return ReadFrom(reader);
    	}

    	public void WriteIndexEntry(IContext context, ByteArrayBuffer writer, object obj)
    	{
			WriteGuid(obj, writer);
    	}

    	public void DefragIndexEntry(DefragmentContextImpl context)
    	{
    		IncrementOffset(context);
    	}

    	public object IndexEntryToObject(IContext context, object indexEntry)
    	{
    		if (indexEntry.GetType() != typeof(Guid))
    		{
    			throw new InvalidOperationException();	
    		}
			return indexEntry;
    	}

    	public object ReadIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer buffer)
    	{
    		return ReadFrom(buffer);
    	}

    	public object ReadIndexEntry(IObjectIdContext context)
    	{
    		return ReadFrom(context);
		}

		#endregion

		private static Guid ReadFrom(IReadBuffer buffer)
		{
			byte[] guidBytes = new byte[GuidSize];
			buffer.ReadBytes(guidBytes);
			return new Guid(guidBytes);
		}

		private static void WriteGuid(object obj, IWriteBuffer context)
		{
			Guid id = (Guid)obj;
			context.WriteBytes(id.ToByteArray());
		}

		private static void IncrementOffset(IDefragmentContext context)
		{
			context.IncrementOffset(GuidSize);
		}
	}
}