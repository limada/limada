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
using Db4objects.Db4o.Qlin;

namespace Db4objects.Db4o.Qlin
{
	/// <summary>support for the new experimental QLin ("Coolin") query interface.</summary>
	/// <remarks>
	/// support for the new experimental QLin ("Coolin") query interface.
	/// We would really like to have LINQ for Java instead.
	/// </remarks>
	/// <since>8.0</since>
	public interface IQLinable
	{
		/// <summary>
		/// starts a
		/// <see cref="IQLin">IQLin</see>
		/// query against a class.
		/// </summary>
		IQLin From(Type clazz);
	}
}
