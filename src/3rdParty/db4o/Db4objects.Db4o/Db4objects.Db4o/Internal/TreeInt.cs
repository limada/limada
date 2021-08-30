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
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal
{
	/// <summary>Base class for balanced trees.</summary>
	/// <remarks>Base class for balanced trees.</remarks>
	/// <exclude></exclude>
	public class TreeInt : Tree, IReadWriteable
	{
		public static Db4objects.Db4o.Internal.TreeInt Add(Db4objects.Db4o.Internal.TreeInt
			 tree, int value)
		{
			return (Db4objects.Db4o.Internal.TreeInt)((Db4objects.Db4o.Internal.TreeInt)Tree.
				Add(tree, new Db4objects.Db4o.Internal.TreeInt(value)));
		}

		public static Db4objects.Db4o.Internal.TreeInt RemoveLike(Db4objects.Db4o.Internal.TreeInt
			 tree, int value)
		{
			return (Db4objects.Db4o.Internal.TreeInt)Tree.RemoveLike(tree, new Db4objects.Db4o.Internal.TreeInt
				(value));
		}

		public static Tree AddAll(Tree tree, IIntIterator4 iter)
		{
			if (!iter.MoveNext())
			{
				return tree;
			}
			Db4objects.Db4o.Internal.TreeInt firstAdded = new Db4objects.Db4o.Internal.TreeInt
				(iter.CurrentInt());
			tree = Tree.Add(tree, firstAdded);
			while (iter.MoveNext())
			{
				tree = tree.Add(new Db4objects.Db4o.Internal.TreeInt(iter.CurrentInt()));
			}
			return tree;
		}

		public int _key;

		public TreeInt(int a_key)
		{
			this._key = a_key;
		}

		public override int Compare(Tree a_to)
		{
			return _key - ((Db4objects.Db4o.Internal.TreeInt)a_to)._key;
		}

		internal virtual Tree DeepClone()
		{
			return new Db4objects.Db4o.Internal.TreeInt(_key);
		}

		public override bool Duplicates()
		{
			return false;
		}

		public static Db4objects.Db4o.Internal.TreeInt Find(Tree a_in, int a_key)
		{
			if (a_in == null)
			{
				return null;
			}
			return ((Db4objects.Db4o.Internal.TreeInt)a_in).Find(a_key);
		}

		public Db4objects.Db4o.Internal.TreeInt Find(int a_key)
		{
			int cmp = _key - a_key;
			if (cmp < 0)
			{
				if (((Tree)_subsequent) != null)
				{
					return ((Db4objects.Db4o.Internal.TreeInt)((Tree)_subsequent)).Find(a_key);
				}
			}
			else
			{
				if (cmp > 0)
				{
					if (((Tree)_preceding) != null)
					{
						return ((Db4objects.Db4o.Internal.TreeInt)((Tree)_preceding)).Find(a_key);
					}
				}
				else
				{
					return this;
				}
			}
			return null;
		}

		public virtual object Read(ByteArrayBuffer buffer)
		{
			return new Db4objects.Db4o.Internal.TreeInt(buffer.ReadInt());
		}

		public virtual void Write(ByteArrayBuffer buffer)
		{
			buffer.WriteInt(_key);
		}

		public static void Write(ByteArrayBuffer buffer, Db4objects.Db4o.Internal.TreeInt
			 tree)
		{
			Write(buffer, tree, tree == null ? 0 : tree.Size());
		}

		public static void Write(ByteArrayBuffer buffer, Db4objects.Db4o.Internal.TreeInt
			 tree, int size)
		{
			if (tree == null)
			{
				buffer.WriteInt(0);
				return;
			}
			buffer.WriteInt(size);
			tree.Traverse(new _IVisitor4_97(buffer));
		}

		private sealed class _IVisitor4_97 : IVisitor4
		{
			public _IVisitor4_97(ByteArrayBuffer buffer)
			{
				this.buffer = buffer;
			}

			public void Visit(object a_object)
			{
				((Db4objects.Db4o.Internal.TreeInt)a_object).Write(buffer);
			}

			private readonly ByteArrayBuffer buffer;
		}

		public virtual int OwnLength()
		{
			return Const4.IntLength;
		}

		internal virtual bool VariableLength()
		{
			return false;
		}

		internal virtual QCandidate ToQCandidate(QCandidates candidates)
		{
			QCandidate qc = new QCandidate(candidates, null, _key);
			qc._preceding = ToQCandidate((Db4objects.Db4o.Internal.TreeInt)((Tree)_preceding)
				, candidates);
			qc._subsequent = ToQCandidate((Db4objects.Db4o.Internal.TreeInt)((Tree)_subsequent
				), candidates);
			qc._size = _size;
			return qc;
		}

		public static QCandidate ToQCandidate(Db4objects.Db4o.Internal.TreeInt tree, QCandidates
			 candidates)
		{
			if (tree == null)
			{
				return null;
			}
			return tree.ToQCandidate(candidates);
		}

		public override string ToString()
		{
			return string.Empty + _key;
		}

		protected override Tree ShallowCloneInternal(Tree tree)
		{
			Db4objects.Db4o.Internal.TreeInt treeint = (Db4objects.Db4o.Internal.TreeInt)base
				.ShallowCloneInternal(tree);
			treeint._key = _key;
			return treeint;
		}

		public override object ShallowClone()
		{
			Db4objects.Db4o.Internal.TreeInt treeint = new Db4objects.Db4o.Internal.TreeInt(_key
				);
			return ShallowCloneInternal(treeint);
		}

		public static int MarshalledLength(Db4objects.Db4o.Internal.TreeInt a_tree)
		{
			if (a_tree == null)
			{
				return Const4.IntLength;
			}
			return a_tree.MarshalledLength();
		}

		public int MarshalledLength()
		{
			if (VariableLength())
			{
				IntByRef length = new IntByRef(Const4.IntLength);
				Traverse(new _IVisitor4_152(length));
				return length.value;
			}
			return MarshalledLength(Size());
		}

		private sealed class _IVisitor4_152 : IVisitor4
		{
			public _IVisitor4_152(IntByRef length)
			{
				this.length = length;
			}

			public void Visit(object obj)
			{
				length.value += ((Db4objects.Db4o.Internal.TreeInt)obj).OwnLength();
			}

			private readonly IntByRef length;
		}

		public int MarshalledLength(int size)
		{
			return Const4.IntLength + (size * OwnLength());
		}

		public override object Key()
		{
			return _key;
		}

		public override bool Equals(object obj)
		{
			Db4objects.Db4o.Internal.TreeInt other = (Db4objects.Db4o.Internal.TreeInt)obj;
			return other._key == _key;
		}
	}
}
