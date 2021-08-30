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
namespace Db4objects.Db4o.Config
{
	/// <summary>Configures the environment (set of services) used by db4o.</summary>
	/// <remarks>Configures the environment (set of services) used by db4o.</remarks>
	/// <seealso cref="Db4objects.Db4o.Foundation.IEnvironment">Db4objects.Db4o.Foundation.IEnvironment
	/// 	</seealso>
	/// <seealso cref="Db4objects.Db4o.Foundation.Environments.My(System.Type{T})">Db4objects.Db4o.Foundation.Environments.My(System.Type&lt;T&gt;)
	/// 	</seealso>
	public interface IEnvironmentConfiguration
	{
		/// <summary>Contributes a service to the db4o environment.</summary>
		/// <remarks>Contributes a service to the db4o environment.</remarks>
		/// <param name="service"></param>
		void Add(object service);
	}
}
