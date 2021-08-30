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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public interface IPausableBlockingQueue4 : IBlockingQueue4
	{
		/// <summary>
		/// <p>
		/// Pauses the queue, making calls to
		/// <see cref="IQueue4.Next()">IQueue4.Next()</see>
		/// block
		/// until
		/// <see cref="Resume()">Resume()</see>
		/// is called.
		/// </summary>
		/// <returns>whether or not this call changed the state of the queue.</returns>
		bool Pause();

		/// <summary>
		/// <p>
		/// Resumes the queue, releasing blocked calls to
		/// <see cref="IQueue4.Next()">IQueue4.Next()</see>
		/// that can reach a next queue item..
		/// </summary>
		/// <returns>whether or not this call changed the state of the queue.</returns>
		bool Resume();

		bool IsPaused();

		/// <summary>
		/// <p>
		/// Returns the next element in queue if there is one available, returns null
		/// otherwise.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Returns the next element in queue if there is one available, returns null
		/// otherwise.
		/// <p>
		/// This method will not never block, regardless of the queue being paused or
		/// no elements are available.
		/// </remarks>
		/// <returns>next element, if available and queue not paused.</returns>
		object TryNext();
	}
}
