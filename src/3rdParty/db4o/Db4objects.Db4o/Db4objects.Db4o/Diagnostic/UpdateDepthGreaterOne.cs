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
using Db4objects.Db4o.Diagnostic;

namespace Db4objects.Db4o.Diagnostic
{
	/// <summary>Diagnostic, if update depth greater than 1.</summary>
	/// <remarks>Diagnostic, if update depth greater than 1.</remarks>
	public class UpdateDepthGreaterOne : DiagnosticBase
	{
		private readonly int _depth;

		public UpdateDepthGreaterOne(int depth)
		{
			_depth = depth;
		}

		public override object Reason()
		{
			return "Db4o.configure().updateDepth(" + _depth + ")";
		}

		public override string Problem()
		{
			return "A global update depth greater than 1 is not recommended";
		}

		public override string Solution()
		{
			return "Increasing the global update depth to a value greater than 1 is only recommended for"
				 + " testing, not for production use. If individual deep updates are needed, consider using"
				 + " ExtObjectContainer#set(object, depth) and make sure to profile the performance of each call.";
		}
	}
}
