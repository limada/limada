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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public abstract class QEStringCmp : QEAbstract
	{
		private bool caseSensitive;

		/// <summary>for C/S messaging only</summary>
		public QEStringCmp()
		{
		}

		public QEStringCmp(bool caseSensitive_)
		{
			caseSensitive = caseSensitive_;
		}

		internal override bool Evaluate(QConObject constraint, QCandidate candidate, object
			 obj)
		{
			if (obj != null)
			{
				if (obj is ByteArrayBuffer)
				{
					obj = candidate.ReadString((ByteArrayBuffer)obj);
				}
				string candidateStringValue = obj.ToString();
				string stringConstraint = constraint.GetObject().ToString();
				if (!caseSensitive)
				{
					candidateStringValue = candidateStringValue.ToLower();
					stringConstraint = stringConstraint.ToLower();
				}
				return CompareStrings(candidateStringValue, stringConstraint);
			}
			return constraint.GetObject() == null;
		}

		public override bool SupportsIndex()
		{
			return false;
		}

		protected abstract bool CompareStrings(string candidate, string constraint);
	}
}
