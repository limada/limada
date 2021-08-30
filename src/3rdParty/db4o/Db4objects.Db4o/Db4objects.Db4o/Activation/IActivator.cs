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
using Db4objects.Db4o.Activation;

namespace Db4objects.Db4o.Activation
{
	/// <summary>
	/// Activator interface.<br />
	/// <br /><br />
	/// <see cref="Db4objects.Db4o.TA.IActivatable">Db4objects.Db4o.TA.IActivatable</see>
	/// objects need to have a reference to
	/// an Activator implementation, which is called
	/// by Transparent Activation, when a request is received to
	/// activate the host object.
	/// </summary>
	/// <seealso><a href="http://developer.db4o.com/resources/view.aspx/reference/Object_Lifecycle/Activation/Transparent_Activation_Framework">Transparent Activation framework.</a>
	/// 	</seealso>
	public interface IActivator
	{
		/// <summary>Method to be called to activate the host object.</summary>
		/// <remarks>Method to be called to activate the host object.</remarks>
		/// <param name="purpose">
		/// for which purpose is the object being activated?
		/// <see cref="ActivationPurpose.Write">ActivationPurpose.Write</see>
		/// will cause the object
		/// to be saved on the next
		/// <see cref="Db4objects.Db4o.IObjectContainer.Commit()">Db4objects.Db4o.IObjectContainer.Commit()
		/// 	</see>
		/// operation.
		/// </param>
		void Activate(ActivationPurpose purpose);
	}
}
