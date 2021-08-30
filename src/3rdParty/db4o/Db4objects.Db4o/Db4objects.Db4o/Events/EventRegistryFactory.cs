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
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Callbacks;
using Db4objects.Db4o.Internal.Events;

namespace Db4objects.Db4o.Events
{
	/// <summary>
	/// Provides an interface for getting an
	/// <see cref="IEventRegistry">IEventRegistry</see>
	/// from an
	/// <see cref="Db4objects.Db4o.IObjectContainer">Db4objects.Db4o.IObjectContainer</see>
	/// .
	/// </summary>
	public class EventRegistryFactory
	{
		/// <summary>
		/// Returns an
		/// <see cref="IEventRegistry">IEventRegistry</see>
		/// for registering events with the specified container.
		/// </summary>
		public static IEventRegistry ForObjectContainer(IObjectContainer objectContainer)
		{
			if (null == objectContainer)
			{
				throw new ArgumentNullException();
			}
			IInternalObjectContainer container = ((IInternalObjectContainer)objectContainer);
			ICallbacks callbacks = container.Callbacks();
			if (callbacks is IEventRegistry)
			{
				return (IEventRegistry)callbacks;
			}
			if (callbacks is NullCallbacks)
			{
				EventRegistryImpl impl = container.NewEventRegistry();
				container.Callbacks(impl);
				return impl;
			}
			// TODO: create a MulticastingCallbacks and register both
			// the current one and the new one
			throw new ArgumentException();
		}
	}
}
