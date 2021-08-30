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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class LockedTree
	{
		private Tree _tree;

		private int _version;

		public virtual void Add(Tree tree)
		{
			Changed();
			_tree = _tree == null ? tree : _tree.Add(tree);
		}

		private void Changed()
		{
			_version++;
		}

		public virtual void Clear()
		{
			Changed();
			_tree = null;
		}

		public virtual Tree Find(int key)
		{
			return TreeInt.Find(_tree, key);
		}

		public virtual void Read(ByteArrayBuffer buffer, IReadable template)
		{
			Clear();
			_tree = new TreeReader(buffer, template).Read();
			Changed();
		}

		public virtual void TraverseLocked(IVisitor4 visitor)
		{
			int currentVersion = _version;
			Tree.Traverse(_tree, visitor);
			if (_version != currentVersion)
			{
				throw new InvalidOperationException();
			}
		}

		public virtual void TraverseMutable(IVisitor4 visitor)
		{
			Collection4 currentContent = new Collection4();
			TraverseLocked(new _IVisitor4_51(currentContent));
			IEnumerator i = currentContent.GetEnumerator();
			while (i.MoveNext())
			{
				visitor.Visit(i.Current);
			}
		}

		private sealed class _IVisitor4_51 : IVisitor4
		{
			public _IVisitor4_51(Collection4 currentContent)
			{
				this.currentContent = currentContent;
			}

			public void Visit(object obj)
			{
				currentContent.Add(obj);
			}

			private readonly Collection4 currentContent;
		}

		public virtual bool IsEmpty()
		{
			return _tree == null;
		}
	}
}
