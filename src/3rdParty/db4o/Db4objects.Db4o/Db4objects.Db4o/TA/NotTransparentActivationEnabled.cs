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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.TA
{
	public class NotTransparentActivationEnabled : DiagnosticBase
	{
		private ClassMetadata _class;

		public NotTransparentActivationEnabled(ClassMetadata clazz)
		{
			_class = clazz;
		}

		public override string Problem()
		{
			return "An object of class " + _class + " was stored. Instances of this class very likely are not subject to transparent activation.";
		}

		public override object Reason()
		{
			return _class;
		}

		public override string Solution()
		{
			return "Use a TA aware class with equivalent functionality or ensure that this class provides a sensible implementation of the "
				 + typeof(IActivatable).FullName + " interface and the implicit TA hooks, either manually or by applying instrumentation.";
		}
	}
}
