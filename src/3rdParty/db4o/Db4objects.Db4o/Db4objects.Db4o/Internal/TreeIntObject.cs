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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class TreeIntObject : TreeInt
	{
		public object _object;

		public TreeIntObject(int a_key) : base(a_key)
		{
		}

		public TreeIntObject(int a_key, object a_object) : base(a_key)
		{
			_object = a_object;
		}

		public override object ShallowClone()
		{
			return ShallowCloneInternal(new Db4objects.Db4o.Internal.TreeIntObject(_key));
		}

		protected override Tree ShallowCloneInternal(Tree tree)
		{
			Db4objects.Db4o.Internal.TreeIntObject tio = (Db4objects.Db4o.Internal.TreeIntObject
				)base.ShallowCloneInternal(tree);
			tio._object = _object;
			return tio;
		}

		public virtual object GetObject()
		{
			return _object;
		}

		public virtual void SetObject(object obj)
		{
			_object = obj;
		}

		public override object Read(ByteArrayBuffer a_bytes)
		{
			int key = a_bytes.ReadInt();
			object obj = null;
			if (_object is TreeInt)
			{
				obj = new TreeReader(a_bytes, (IReadable)_object).Read();
			}
			else
			{
				obj = ((IReadable)_object).Read(a_bytes);
			}
			return new Db4objects.Db4o.Internal.TreeIntObject(key, obj);
		}

		public override void Write(ByteArrayBuffer a_writer)
		{
			a_writer.WriteInt(_key);
			if (_object == null)
			{
				a_writer.WriteInt(0);
			}
			else
			{
				if (_object is TreeInt)
				{
					TreeInt.Write(a_writer, (TreeInt)_object);
				}
				else
				{
					((IReadWriteable)_object).Write(a_writer);
				}
			}
		}

		public override int OwnLength()
		{
			if (_object == null)
			{
				return Const4.IntLength * 2;
			}
			return Const4.IntLength + ((IReadable)_object).MarshalledLength();
		}

		internal override bool VariableLength()
		{
			return true;
		}

		public static Db4objects.Db4o.Internal.TreeIntObject Add(Db4objects.Db4o.Internal.TreeIntObject
			 tree, int key, object value)
		{
			return ((Db4objects.Db4o.Internal.TreeIntObject)Tree.Add(tree, new Db4objects.Db4o.Internal.TreeIntObject
				(key, value)));
		}
	}
}
