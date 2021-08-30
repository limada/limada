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
	/// <summary>
	/// Diagnostic if
	/// <see cref="Db4objects.Db4o.Config.IObjectClass.ObjectField(string)">Db4objects.Db4o.Config.IObjectClass.ObjectField(string)
	/// 	</see>
	/// was called on a
	/// field that does not exist.
	/// </summary>
	public class ObjectFieldDoesNotExist : DiagnosticBase
	{
		public readonly string _className;

		public readonly string _fieldName;

		public ObjectFieldDoesNotExist(string className, string fieldName)
		{
			_className = className;
			_fieldName = fieldName;
		}

		public override string Problem()
		{
			return "ObjectField was configured but does not exist.";
		}

		public override object Reason()
		{
			return _className + "." + _fieldName;
		}

		public override string Solution()
		{
			return "Check your configuration.";
		}
	}
}
