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
using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public class QLinField : QLinSubNode
	{
		private readonly IQuery _node;

		public QLinField(QLinRoot root, object expression) : base(root)
		{
			_node = root.Descend(expression);
		}

		public override IQLin Equal(object obj)
		{
			IConstraint constraint = _node.Constrain(obj);
			constraint.Equal();
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}

		public override IQLin StartsWith(string @string)
		{
			IConstraint constraint = _node.Constrain(@string);
			constraint.StartsWith(true);
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}

		public override IQLin Smaller(object obj)
		{
			IConstraint constraint = _node.Constrain(obj);
			constraint.Smaller();
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}

		public override IQLin Greater(object obj)
		{
			IConstraint constraint = _node.Constrain(obj);
			constraint.Greater();
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}
	}
}
