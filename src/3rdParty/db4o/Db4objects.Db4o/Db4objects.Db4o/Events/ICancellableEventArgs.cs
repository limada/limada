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
namespace Db4objects.Db4o.Events
{
	/// <summary>Argument for events related to cancellable actions.</summary>
	/// <remarks>Argument for events related to cancellable actions.</remarks>
	/// <seealso cref="IEventRegistry">IEventRegistry</seealso>
	public interface ICancellableEventArgs
	{
		/// <summary>Queries if the action was already cancelled by some event listener.</summary>
		/// <remarks>Queries if the action was already cancelled by some event listener.</remarks>
		bool IsCancelled
		{
			get;
		}

		/// <summary>Cancels the action related to this event.</summary>
		/// <remarks>
		/// Cancels the action related to this event.
		/// Although the related action will be cancelled all the registered
		/// listeners will still receive the event.
		/// </remarks>
		void Cancel();
	}
}
