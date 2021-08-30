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
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config
{
	/// <summary>A provider for custom database names.</summary>
	/// <remarks>A provider for custom database names.</remarks>
	public interface INameProvider
	{
		/// <summary>
		/// Derives a name for the given
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// . This method will be called when
		/// database startup has completed, i.e. the method will see a completely initialized
		/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
		/// .
		/// Any code invoked during the startup process (for example
		/// <see cref="IConfigurationItem">IConfigurationItem</see>
		/// instances) will still
		/// see the default naming.
		/// </summary>
		string Name(IObjectContainer db);
	}
}
